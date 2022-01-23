using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : CharacterClass
{
	[HideInInspector]
	public bool isDead = false;
	[HideInInspector]
	public bool alreadyDied = false;
	[HideInInspector]
	public float bowAccuracy = 0f;
	//Calcular Center of Mass con el arma

	//-------------------------------------------------------------
	//--Variables
	//-------------------------------------------------------------

	//Weapon gameObject
	public WeaponScript weapon;
	[SerializeField]
	private GameObject healingOrb;


	//Active Ragdoll Player parts
	public GameObject
		//
		Root, Body, Head,
		UpperRightArm, LowerRightArm,
		UpperLeftArm, LowerLeftArm,
		UpperRightLeg, LowerRightLeg,
		UpperLeftLeg, LowerLeftLeg,
		RightFoot, LeftFoot;

	//Rigidbody Hands
	public Rigidbody RightHand, LeftHand;

	//Center of mass point
	public Transform COMP;

	[Header("Input on this player")]
	//Enable controls

	[Header("Player Input Axis")]
	//Player Axis controls
	public float forwardBackward;
	public float leftRight;
	public float jump;
	public bool punchLeft;
	public bool attack;
	public bool drop;
	public bool interact;

	[Header("The Layer Only This Player Is On")]
	//Player layer name
	public string thisPlayerLayer = "Player_1";

	[Header("Movement Properties")]
	//Movement
	public float turnSpeed = 6f;
	public float jumpForce = 18f;

	[Header("Balance Properties")]
	//Balance
	public bool autoGetUpWhenPossible = true;
	public bool useStepPrediction = true;
	public float balanceHeight = 2.5f;
	public float balanceStrength = 5000f;
	public float coreStrength = 1500f;
	public float limbStrength = 500f;
	//Walking
	public float StepDuration = 0.2f;
	public float StepHeight = 1.7f;
	public float FeetMountForce = 25f;

	[Header("Reach Properties")]
	//Reach
	public float reachSensitivity = 25f;
	public float armReachStiffness = 2000f;

	[Header("Actions")]
	//Punch
	public bool canBeKnockoutByImpact = true;
	public float requiredForceToBeKO = 20f;
	public bool canPunch = true;
	public float punchForce = 15f;

	[Header("Audio")]
	//Impact sounds
	public float ImpactForce = 10f;
	public AudioClip[] Impacts;
	public AudioClip[] Hits;
	public AudioClip[] NoHits;
	public AudioClip[] Steps;
	public AudioClip DeathSound;
	public AudioSource SoundSource;
	public AudioSource StepSource;


	//Hidden variables
	private float
		timer, Step_R_timer, Step_L_timer,
		MouseYAxisArms, MouseXAxisArms, MouseYAxisBody;

	private bool
		WalkForward, WalkBackward,
		StepRight, StepLeft, Alert_Leg_Right,
		Alert_Leg_Left, balanced = true, GettingUp,
		ResetPose, isRagdoll, isKeyDown, moveAxisUsed,
		jumpAxisUsed, reachLeftAxisUsed, reachRightAxisUsed;

	[HideInInspector]
	public bool
		jumping, isJumping, inAir,
		attacking, punchingLeft;

	private Camera cam;
	private Vector3 Direction;
	private Vector3 CenterOfMassPoint;

	//Active Ragdoll Player Parts Array
	private GameObject[] APR_Parts;

	//Joint Drives on & off
	JointDrive
		//
		BalanceOn, PoseOn, CoreStiffness, ReachStiffness, DriveOff;

	//Original pose target rotation
	Quaternion
		//
		HeadTarget, BodyTarget,
		UpperRightArmTarget, LowerRightArmTarget,
		UpperLeftArmTarget, LowerLeftArmTarget,
		UpperRightLegTarget, LowerRightLegTarget,
		UpperLeftLegTarget, LowerLeftLegTarget;

	[Header("Player Editor Debug Mode")]
	//Debug
	public bool editorDebugMode;


	//-------------------------------------------------------------
	//--Calling Functions
	//-------------------------------------------------------------



	//---Setup---//
	//////////////
	void Awake()
	{
		PlayerSetup();
		YoureDead += OnDead;
		life = maxLife;
		system = Instantiate(particleObj).GetComponent<ParticleSystem>();
	}



	//---Updates---//
	////////////////
	void Update()
	{
		invTime -= Time.deltaTime;
		if (attacking && !Object.ReferenceEquals(weapon, null) && weapon.kind.Equals(Weapons.Bow)) chargingTime += Time.deltaTime*1.4f;
		if (!inAir)
		{
			PlayerMovement();

			if (canPunch)
			{
				PlayerPunch();
			}
		}
		PlayerReach();

		if (balanced && useStepPrediction)
		{
			StepPrediction();
			CenterOfMass();
		}

		if (!useStepPrediction)
		{
			ResetWalkCycle();
		}

		GroundCheck();
		CenterOfMass();
	}



	//---Fixed Updates---//
	//////////////////////
	void FixedUpdate()
	{
		Walking();

		PlayerRotation();
		ResetPlayerPose();

		PlayerGetUpJumping();
	}



	//-------------------------------------------------------------
	//--Functions
	//-------------------------------------------------------------



	//---Player Setup--//
	////////////////////
	void PlayerSetup()
	{
		cam = Camera.main;

		//Setup joint drives
		BalanceOn = new JointDrive();
		BalanceOn.positionSpring = balanceStrength;
		BalanceOn.positionDamper = 0;
		BalanceOn.maximumForce = Mathf.Infinity;

		PoseOn = new JointDrive();
		PoseOn.positionSpring = limbStrength;
		PoseOn.positionDamper = 0;
		PoseOn.maximumForce = Mathf.Infinity;

		CoreStiffness = new JointDrive();
		CoreStiffness.positionSpring = coreStrength;
		CoreStiffness.positionDamper = 0;
		CoreStiffness.maximumForce = Mathf.Infinity;

		ReachStiffness = new JointDrive();
		ReachStiffness.positionSpring = armReachStiffness;
		ReachStiffness.positionDamper = 0;
		ReachStiffness.maximumForce = Mathf.Infinity;

		DriveOff = new JointDrive();
		DriveOff.positionSpring = 25;
		DriveOff.positionDamper = 0;
		DriveOff.maximumForce = Mathf.Infinity;

		//Setup/reroute active ragdoll parts to array
		APR_Parts = new GameObject[]
		{
			//array index numbers
			
			//0
			Root,
			//1
			Body,
			//2
			Head,
			//3
			UpperRightArm,
			//4
			LowerRightArm,
			//5
			UpperLeftArm,
			//6
			LowerLeftArm,
			//7
			UpperRightLeg,
			//8
			LowerRightLeg,
			//9
			UpperLeftLeg,
			//10
			LowerLeftLeg,
			//11
			RightFoot,
			//12
			LeftFoot
		};

		//Setup original pose for joint drives
		BodyTarget = APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation;
		HeadTarget = APR_Parts[2].GetComponent<ConfigurableJoint>().targetRotation;
		UpperRightArmTarget = APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation;
		LowerRightArmTarget = APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation;
		UpperLeftArmTarget = APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation;
		LowerLeftArmTarget = APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation;
		UpperRightLegTarget = APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation;
		LowerRightLegTarget = APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation;
		UpperLeftLegTarget = APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation;
		LowerLeftLegTarget = APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation;
	}



	//---Ground Check---//
	/////////////////////
	void GroundCheck()
	{
		Ray ray = new Ray(APR_Parts[0].transform.position, -APR_Parts[0].transform.up);
		RaycastHit hit;

		//Balance when ground is detected
		if (Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")) && !inAir && !isJumping && !reachRightAxisUsed && !reachLeftAxisUsed)
		{
			if (!balanced && APR_Parts[0].GetComponent<Rigidbody>().velocity.magnitude < 1f)
			{
				if (autoGetUpWhenPossible)
				{
					balanced = true;
				}
			}
		}
		//Fall over when ground is not detected
		else if (!Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")))
		{
			if (balanced)
			{
				balanced = false;
			}
		}

		//Balance on/off
		if (balanced && isRagdoll)
		{
			DeactivateRagdoll();
		}
		else if (!balanced && !isRagdoll)
		{
			ActivateRagdoll();
		}
	}



	//---Step Prediction---//
	////////////////////////
	void StepPrediction()
	{
		//Reset variables when balanced
		if (!WalkForward && !WalkBackward)
		{
			StepRight = false;
			StepLeft = false;
			Step_R_timer = 0;
			Step_L_timer = 0;
			Alert_Leg_Right = false;
			Alert_Leg_Left = false;
		}

		//Check direction to walk when off balance
		//Backwards
		if (COMP.position.z < APR_Parts[11].transform.position.z && COMP.position.z < APR_Parts[12].transform.position.z)
		{
			WalkBackward = true;
		}
		else
		{
			if (!isKeyDown)
			{
				WalkBackward = false;
			}
		}

		//Forward
		if (COMP.position.z > APR_Parts[11].transform.position.z && COMP.position.z > APR_Parts[12].transform.position.z)
		{
			WalkForward = true;
		}
		else
		{
			if (!isKeyDown)
			{
				WalkForward = false;
			}
		}
	}



	//---Reset Walk Cycle---//
	/////////////////////////
	void ResetWalkCycle()
	{
		//Reset variables when not moving
		if (!WalkForward && !WalkBackward)
		{
			StepRight = false;
			StepLeft = false;
			Step_R_timer = 0;
			Step_L_timer = 0;
			Alert_Leg_Right = false;
			Alert_Leg_Left = false;
		}
	}



	float abs(float i)
	{
		return ((i < 0) ? -i : i);
	}

	//---Player Movement---//
	////////////////////////
	void PlayerMovement()
	{
		if (!isRagdoll)
		{
			Direction = new Vector3(leftRight, 0.0f, forwardBackward);
			APR_Parts[0].transform.GetComponent<Rigidbody>().velocity = Vector3.Lerp(APR_Parts[0].transform.GetComponent<Rigidbody>().velocity, (Direction * abs(moveSpeed)) + Vector3.up * APR_Parts[0].transform.GetComponent<Rigidbody>().velocity.y, 0.8f);
		}
		if (leftRight != 0 || forwardBackward != 0 && balanced)
		{
			if (!WalkForward && !moveAxisUsed)
			{
				WalkForward = true;
				moveAxisUsed = true;
				isKeyDown = true;
			}
		}

		else if (leftRight == 0 && forwardBackward == 0)
		{
			if (WalkForward && moveAxisUsed)
			{
				WalkForward = false;
				moveAxisUsed = false;
				isKeyDown = false;
			}
		}
	}



	//---Player Rotation---//
	////////////////////////
	void PlayerRotation()
	{
		//Camera Direction
		//Turn with camera
		//var lookPos = cam.transform.forward; CAMBIO
		if ((leftRight != 0 || forwardBackward != 0))
		{
			var lookPos = new Vector3(-leftRight, 0.0f, forwardBackward) * 5;
			lookPos.y = 0;
			var rotation = Quaternion.LookRotation(lookPos);
			//APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Slerp(APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation, rotation, Time.deltaTime * turnSpeed);
			APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.RotateTowards(APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation, rotation, Time.deltaTime * 20 * turnSpeed);
		}
	}



	//---Player GetUp & Jumping---//
	///////////////////////////////
	void PlayerGetUpJumping()
	{
		if (jump > 0)
		{
			if (!jumpAxisUsed)
			{
				if (balanced && !inAir)
				{
					jumping = true;
				}

				else if (!balanced)
				{
					DeactivateRagdoll();
				}
			}

			jumpAxisUsed = true;
		}

		else
		{
			jumpAxisUsed = false;
		}


		if (jumping)
		{
			isJumping = true;

			var v3 = APR_Parts[0].GetComponent<Rigidbody>().transform.up * jumpForce;
			v3.x = APR_Parts[0].GetComponent<Rigidbody>().velocity.x;
			v3.z = APR_Parts[0].GetComponent<Rigidbody>().velocity.z;
			APR_Parts[0].GetComponent<Rigidbody>().velocity = v3;
		}

		if (isJumping)
		{
			timer = timer + Time.fixedDeltaTime;

			if (timer > 0.2f)
			{
				timer = 0.0f;
				jumping = false;
				isJumping = false;
				inAir = true;
			}
		}
	}



	//---Player Landed---//
	//////////////////////
	public void PlayerLanded()
	{
		if (inAir && !isJumping && !jumping)
		{
			inAir = false;
			ResetPose = true;
		}
	}



	//---Player Reach--//
	////////////////////
	void PlayerReach()
	{
		//Reach Left
		if (drop && !punchingLeft)
		{

			if (!reachLeftAxisUsed)
			{
				//Adjust Left Arm joint strength
				APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
				APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
				APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
				APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;

				//Adjust body joint strength
				APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = CoreStiffness;
				APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = CoreStiffness;

				reachLeftAxisUsed = true;
			}

			if (MouseYAxisArms <= 1.2f && MouseYAxisArms >= -1.2f)
			{
				MouseYAxisArms = MouseYAxisArms + (Input.GetAxis("Mouse Y") / reachSensitivity);
			}

			else if (MouseYAxisArms > 1.2f)
			{
				MouseYAxisArms = 1.2f;
			}

			else if (MouseYAxisArms < -1.2f)
			{
				MouseYAxisArms = -1.2f;
			}

			//upper  left arm pose
			APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.58f - (MouseYAxisArms), -0.88f - (MouseYAxisArms), -0.8f, 1);
		}

		if (!drop && !punchingLeft)
		{
			if (reachLeftAxisUsed)
			{
				if (balanced)
				{
					APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
					APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
					APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
					APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

					APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
					APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
				}

				else if (!balanced)
				{
					APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
					APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
					APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
					APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
				}

				ResetPose = true;
				reachLeftAxisUsed = false;
			}
		}




		//Reach Right
		if (interact && !attacking)
		{

			if (!reachRightAxisUsed)
			{
				//Adjust Right Arm joint strength
				APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
				APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
				APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
				APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;

				//Adjust body joint strength
				APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = CoreStiffness;
				APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = CoreStiffness;

				reachRightAxisUsed = true;
			}

			if (MouseYAxisArms <= 1.2f && MouseYAxisArms >= -1.2f)
			{
				MouseYAxisArms = MouseYAxisArms + (Input.GetAxis("Mouse Y") / reachSensitivity);
			}

			else if (MouseYAxisArms > 1.2f)
			{
				MouseYAxisArms = 1.2f;
			}

			else if (MouseYAxisArms < -1.2f)
			{
				MouseYAxisArms = -1.2f;
			}

			//upper right arm pose
			APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.58f + (MouseYAxisArms), -0.88f - (MouseYAxisArms), 0.8f, 1);
		}

		if (!interact && !attacking)
		{
			if (reachRightAxisUsed)
			{
				if (balanced)
				{
					APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
					APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
					APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
					APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

					APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
					APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
				}

				else if (!balanced)
				{
					APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
					APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
					APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
					APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
				}

				ResetPose = true;
				reachRightAxisUsed = false;
			}
		}

	}

	public void Jump()
    {
		jump = 1;
		StartCoroutine(Jumping());
	}
	IEnumerator Jumping()
	{
		yield return new WaitForSeconds(.2f);
		jump = 0;
	}

	public void Attack()
    {
		attack = true;
		StartCoroutine(Attacking());
    }

	IEnumerator Attacking()
    {
		yield return new WaitForSeconds(.7f);
		attack = false;
	}
	//---Player Punch---//
	/////////////////////
	void PlayerPunch()
	{

		//punch right
		if (!attacking && attack)
		{
			attacking = true;
			if (!Object.ReferenceEquals(weapon, null))
			{
				weapon.PrepareHit(APR_Parts[1].GetComponent<ConfigurableJoint>(), APR_Parts[3].GetComponent<ConfigurableJoint>(), APR_Parts[4].GetComponent<ConfigurableJoint>());
			}
			else
			{
				//Right hand punch pull back pose
				APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
				APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.62f, -0.51f, 0.02f, 1);
				APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(1.31f, 0.5f, -0.5f, 1);
			}
		}

		if (attacking && !attack && !isRagdoll)
		{
			attacking = false;
			if (!Object.ReferenceEquals(weapon, null))
			{
				weapon.Hit(APR_Parts[1].GetComponent<ConfigurableJoint>(), APR_Parts[3].GetComponent<ConfigurableJoint>(), APR_Parts[4].GetComponent<ConfigurableJoint>(), punchForce);
				switch (weapon.kind)
				{
					case Weapons.Bow:
						//var lookPos = new Vector3(Root.transform.forward.x, 0.0f, Root.transform.forward.z);
						float xAdded = UnityEngine.Random.Range(-100f, 100f) * .002f;
						float zAdded = UnityEngine.Random.Range(-100f, 100f) * .002f;
						Vector3 lookPos = new Vector3(Root.transform.forward.x, 0f, Root.transform.forward.z);
						weapon.Shoot(lookPos.normalized + new Vector3(xAdded, 0f, zAdded)*(1 - bowAccuracy), Characters.Enemy, chargingTime * .7f);
						chargingTime = .45f;
						break;
					case Weapons.Discobolus:
						lookPos = new Vector3(Root.transform.forward.x, 0, Root.transform.forward.z);
						GetComponentInChildren<EWeaponDetection>().ThrowDisco(lookPos);
						break;
					case Weapons.Spear:
						lookPos = new Vector3(Root.transform.forward.x, 0, Root.transform.forward.z);
						weapon.GetComponent<SpearScript>().Hit(APR_Parts[1].GetComponent<ConfigurableJoint>(), APR_Parts[3].GetComponent<ConfigurableJoint>(), 
							APR_Parts[4].GetComponent<ConfigurableJoint>(), lookPos, punchForce);
						break;
					default:
						RightHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
						APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
						break;
				}
			}
			else
			{
				//Right hand punch release pose
				APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
				APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.74f, 0.04f, 0f, 1);
				APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.2f, 0, 0, 1);
				//Right hand punch force
			}

			RightHand.velocity = Vector3.zero;
			RightHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

			APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

			StartCoroutine(DelayCoroutine());
			IEnumerator DelayCoroutine()
			{
				yield return new WaitForSeconds(0.3f);
				if (!attack)
				{
					APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
					APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
				}
			}
		}


		//punch left
		if (!punchingLeft && punchLeft)
		{
			punchingLeft = true;

			//Left hand punch pull back pose
			//APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, 0.15f, 0, 1);
			//APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( -0.15f, 0.15f, 0, 1);
			//APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion( 0.62f, -0.51f, 0.02f, 1);
			APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(.15f, -.18f, -.46f, 1f);
			//APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.7f, -.06f, -.02f, -.7f);
			APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.77f, -0.19f, -.06f, -.61f);
		}

		if (punchingLeft && !punchLeft)
		{
			punchingLeft = false;
			if (!Object.ReferenceEquals(weapon, null) && weapon.kind.Equals(Weapons.SwordNShield))
			{

			}
			else
			{
				//Left hand punch release pose
				APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
				APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.74f, 0.04f, 0f, 1);
				APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.2f, 0, 0, 1);

				//Left hand punch force
				LeftHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);

				APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
			}
			StartCoroutine(DelayCoroutine());
			IEnumerator DelayCoroutine()
			{
				yield return new WaitForSeconds(0.3f);
				if (!punchLeft)
				{
					APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
					APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;
				}
			}
		}
	}



	//---Player Walking---//
	///////////////////////
	void Walking()
	{
		if (!inAir && balanced)
		{
			if (WalkForward)
			{
				//right leg
				if (APR_Parts[11].transform.position.z < APR_Parts[12].transform.position.z && !StepLeft && !Alert_Leg_Right)
				{
					StepRight = true;
					Alert_Leg_Right = true;
					Alert_Leg_Left = true;
				}

				//left leg
				if (APR_Parts[11].transform.position.z > APR_Parts[12].transform.position.z && !StepRight && !Alert_Leg_Left)
				{
					StepLeft = true;
					Alert_Leg_Left = true;
					Alert_Leg_Right = true;
				}
			}

			if (WalkBackward)
			{
				//right leg
				if (APR_Parts[11].transform.position.z > APR_Parts[12].transform.position.z && !StepLeft && !Alert_Leg_Right)
				{
					StepRight = true;
					Alert_Leg_Right = true;
					Alert_Leg_Left = true;
				}

				//left leg
				if (APR_Parts[11].transform.position.z < APR_Parts[12].transform.position.z && !StepRight && !Alert_Leg_Left)
				{
					StepLeft = true;
					Alert_Leg_Left = true;
					Alert_Leg_Right = true;
				}
			}

			//Step right
			if (StepRight)
			{
				Step_R_timer += Time.fixedDeltaTime;

				//Right foot force down
				APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);

				//walk simulation
				if (WalkForward)
				{
					APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * StepHeight, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
					APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * StepHeight * 2, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.w);

					APR_Parts[9].GetComponent<ConfigurableJoint>().GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * StepHeight / 2, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
				}

				if (WalkBackward)
				{
					APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x - 0.00f * StepHeight, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
					APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.x - 0.07f * StepHeight * 2, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation.w);

					APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x + 0.02f * StepHeight / 2, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
				}


				//step duration
				if (Step_R_timer > StepDuration)
				{
					Step_R_timer = 0;
					StepRight = false;

					if (WalkForward || WalkBackward)
					{
						StepLeft = true;
					}
				}
			}
			else
			{
				//reset to idle
				APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation, UpperRightLegTarget, (8f) * Time.fixedDeltaTime);
				APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[8].GetComponent<ConfigurableJoint>().targetRotation, LowerRightLegTarget, (17f) * Time.fixedDeltaTime);

				//feet force down
				APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
				APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
			}


			//Step left
			if (StepLeft)
			{
				Step_L_timer += Time.fixedDeltaTime;

				//Left foot force down
				APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);

				//walk simulation
				if (WalkForward)
				{
					APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x + 0.09f * StepHeight, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
					APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.x - 0.09f * StepHeight * 2, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.w);

					APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x - 0.12f * StepHeight / 2, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
				}

				if (WalkBackward)
				{
					APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.x - 0.00f * StepHeight, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation.w);
					APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.x - 0.07f * StepHeight * 2, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation.w);

					APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.x + 0.02f * StepHeight / 2, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.y, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.z, APR_Parts[7].GetComponent<ConfigurableJoint>().targetRotation.w);
				}


				//Step duration
				if (Step_L_timer > StepDuration)
				{
					Step_L_timer = 0;
					StepLeft = false;

					if (WalkForward || WalkBackward)
					{
						StepRight = true;
					}
				}
			}
			else
			{
				//reset to idle
				APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[9].GetComponent<ConfigurableJoint>().targetRotation, UpperLeftLegTarget, (7f) * Time.fixedDeltaTime);
				APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Lerp(APR_Parts[10].GetComponent<ConfigurableJoint>().targetRotation, LowerLeftLegTarget, (18f) * Time.fixedDeltaTime);

				//feet force down
				APR_Parts[11].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
				APR_Parts[12].GetComponent<Rigidbody>().AddForce(-Vector3.up * FeetMountForce * Time.deltaTime, ForceMode.Impulse);
			}
		}
	}



	//---Activate Ragdoll---//
	/////////////////////////
	public void ActivateRagdoll()
	{
		isRagdoll = true;
		balanced = false;

		//Root
		APR_Parts[0].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
		APR_Parts[0].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
		//head
		APR_Parts[2].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
		APR_Parts[2].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
		//arms
		if (!reachRightAxisUsed)
		{
			APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
			APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
			APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
			APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
		}

		if (!reachLeftAxisUsed)
		{
			APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
			APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
			APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
			APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
		}
		//legs
		APR_Parts[7].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
		APR_Parts[7].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
		APR_Parts[8].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
		APR_Parts[8].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
		APR_Parts[9].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
		APR_Parts[9].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
		APR_Parts[10].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
		APR_Parts[10].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
		APR_Parts[11].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
		APR_Parts[11].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
		APR_Parts[12].GetComponent<ConfigurableJoint>().angularXDrive = DriveOff;
		APR_Parts[12].GetComponent<ConfigurableJoint>().angularYZDrive = DriveOff;
	}




	//---Deactivate Ragdoll---//
	///////////////////////////
	void DeactivateRagdoll()
	{
		isRagdoll = false;
		balanced = true;

		//Root
		APR_Parts[0].GetComponent<ConfigurableJoint>().angularXDrive = BalanceOn;
		APR_Parts[0].GetComponent<ConfigurableJoint>().angularYZDrive = BalanceOn;
		//head
		APR_Parts[2].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
		APR_Parts[2].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
		//arms
		if (!reachRightAxisUsed)
		{
			APR_Parts[3].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
			APR_Parts[3].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
			APR_Parts[4].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
			APR_Parts[4].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
		}

		if (!reachLeftAxisUsed)
		{
			APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
			APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
			APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
			APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
		}
		//legs
		APR_Parts[7].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
		APR_Parts[7].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
		APR_Parts[8].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
		APR_Parts[8].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
		APR_Parts[9].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
		APR_Parts[9].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
		APR_Parts[10].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
		APR_Parts[10].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
		APR_Parts[11].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
		APR_Parts[11].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;
		APR_Parts[12].GetComponent<ConfigurableJoint>().angularXDrive = PoseOn;
		APR_Parts[12].GetComponent<ConfigurableJoint>().angularYZDrive = PoseOn;

		ResetPose = true;
	}



	//---Reset Player Pose---//
	//////////////////////////
	void ResetPlayerPose()
	{
		if (ResetPose && !jumping)
		{
			APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = BodyTarget;
			APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
			APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
			APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
			APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;

			MouseYAxisArms = 0;

			ResetPose = false;
		}
	}



	//---Calculating Center of mass point---//
	/////////////////////////////////////////
	void CenterOfMass()
	{
		if (Object.ReferenceEquals(weapon, null))
		{
			CenterOfMassPoint =

				(APR_Parts[0].GetComponent<Rigidbody>().mass * APR_Parts[0].transform.position +
					APR_Parts[1].GetComponent<Rigidbody>().mass * APR_Parts[1].transform.position +
					APR_Parts[2].GetComponent<Rigidbody>().mass * APR_Parts[2].transform.position +
					APR_Parts[3].GetComponent<Rigidbody>().mass * APR_Parts[3].transform.position +
					APR_Parts[4].GetComponent<Rigidbody>().mass * APR_Parts[4].transform.position +
					APR_Parts[5].GetComponent<Rigidbody>().mass * APR_Parts[5].transform.position +
					APR_Parts[6].GetComponent<Rigidbody>().mass * APR_Parts[6].transform.position +
					APR_Parts[7].GetComponent<Rigidbody>().mass * APR_Parts[7].transform.position +
					APR_Parts[8].GetComponent<Rigidbody>().mass * APR_Parts[8].transform.position +
					APR_Parts[9].GetComponent<Rigidbody>().mass * APR_Parts[9].transform.position +
					APR_Parts[10].GetComponent<Rigidbody>().mass * APR_Parts[10].transform.position +
					APR_Parts[11].GetComponent<Rigidbody>().mass * APR_Parts[11].transform.position +
					APR_Parts[12].GetComponent<Rigidbody>().mass * APR_Parts[12].transform.position)

				/

				(APR_Parts[0].GetComponent<Rigidbody>().mass + APR_Parts[1].GetComponent<Rigidbody>().mass +
					APR_Parts[2].GetComponent<Rigidbody>().mass + APR_Parts[3].GetComponent<Rigidbody>().mass +
					APR_Parts[4].GetComponent<Rigidbody>().mass + APR_Parts[5].GetComponent<Rigidbody>().mass +
					APR_Parts[6].GetComponent<Rigidbody>().mass + APR_Parts[7].GetComponent<Rigidbody>().mass +
					APR_Parts[8].GetComponent<Rigidbody>().mass + APR_Parts[9].GetComponent<Rigidbody>().mass +
					APR_Parts[10].GetComponent<Rigidbody>().mass + APR_Parts[11].GetComponent<Rigidbody>().mass +
					APR_Parts[12].GetComponent<Rigidbody>().mass);
		}
		else
		{
			CenterOfMassPoint =

				(APR_Parts[0].GetComponent<Rigidbody>().mass * APR_Parts[0].transform.position +
					APR_Parts[1].GetComponent<Rigidbody>().mass * APR_Parts[1].transform.position +
					APR_Parts[2].GetComponent<Rigidbody>().mass * APR_Parts[2].transform.position +
					APR_Parts[3].GetComponent<Rigidbody>().mass * APR_Parts[3].transform.position +
					APR_Parts[4].GetComponent<Rigidbody>().mass * APR_Parts[4].transform.position +
					APR_Parts[5].GetComponent<Rigidbody>().mass * APR_Parts[5].transform.position +
					APR_Parts[6].GetComponent<Rigidbody>().mass * APR_Parts[6].transform.position +
					APR_Parts[7].GetComponent<Rigidbody>().mass * APR_Parts[7].transform.position +
					APR_Parts[8].GetComponent<Rigidbody>().mass * APR_Parts[8].transform.position +
					APR_Parts[9].GetComponent<Rigidbody>().mass * APR_Parts[9].transform.position +
					APR_Parts[10].GetComponent<Rigidbody>().mass * APR_Parts[10].transform.position +
					APR_Parts[11].GetComponent<Rigidbody>().mass * APR_Parts[11].transform.position +
					APR_Parts[12].GetComponent<Rigidbody>().mass * APR_Parts[12].transform.position +
					weapon.GetComponent<Rigidbody>().mass * weapon.transform.position)

				/

				(APR_Parts[0].GetComponent<Rigidbody>().mass + APR_Parts[1].GetComponent<Rigidbody>().mass +
					APR_Parts[2].GetComponent<Rigidbody>().mass + APR_Parts[3].GetComponent<Rigidbody>().mass +
					APR_Parts[4].GetComponent<Rigidbody>().mass + APR_Parts[5].GetComponent<Rigidbody>().mass +
					APR_Parts[6].GetComponent<Rigidbody>().mass + APR_Parts[7].GetComponent<Rigidbody>().mass +
					APR_Parts[8].GetComponent<Rigidbody>().mass + APR_Parts[9].GetComponent<Rigidbody>().mass +
					APR_Parts[10].GetComponent<Rigidbody>().mass + APR_Parts[11].GetComponent<Rigidbody>().mass +
					APR_Parts[12].GetComponent<Rigidbody>().mass + weapon.GetComponent<Rigidbody>().mass);
		}
		COMP.position = CenterOfMassPoint;
	}

	public void OnDead(object s, System.EventArgs e) {
		ActivateRagdoll();
		StartCoroutine(Kill());
		IEnumerator Kill()
		{
			yield return new WaitForSeconds(4f);
			foreach (Collider c in GetComponentsInChildren<Collider>())
            {
				c.enabled = false;
			}
			foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>())
			{
				r.velocity = Vector3.zero;
				r.useGravity = false;
				r.velocity = Vector3.down;
			}
			if (character.Equals(Characters.Enemy))
			{
				isDead = true;
				if (UnityEngine.Random.Range(0, 100) < 25) Destroy(Instantiate(healingOrb, Root.transform.position, Quaternion.identity), 20f);
			}
			yield return new WaitForSeconds(3f);
			Destroy(this.gameObject);
		}
	}


	public bool IsRagdoll()
    {
		return isRagdoll;
    }

	//-------------------------------------------------------------
	//--Debug
	//-------------------------------------------------------------



	//---Editor Debug Mode---//
	//////////////////////////
	void OnDrawGizmos()
	{
		if (editorDebugMode)
		{
			Debug.DrawRay(Root.transform.position, -Root.transform.up * balanceHeight, Color.green);

			if (useStepPrediction)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(COMP.position, 0.3f);
			}
		}
	}

}