using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterClass
{
	 /*
	public Vector3 rota1;
	public Vector3 rota3;
	public Vector3 rota4;
	 */
	public int id;
	//Calcular Center of Mass con el arma

	//-------------------------------------------------------------
	//--Variables
	//-------------------------------------------------------------

	//Weapon gameObject
	[HideInInspector]
	public WeaponScript weapon;
	public WeaponDetection detector;


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
	public string forwardBackward = "Vertical";
	public string leftRight = "Horizontal";
	public string jump = "Jump";
	public string left = "Left";
	public string attack = "Right";
	public string drop = "Drop";
	public string interact = "Interact";
	public string lookX = "Look X";
	public string lookY = "Look Y";
	public string dash = "Dash";

	[Header("The Layer Only This Player Is On")]
	//Player layer name
	public string thisPlayerLayer = "Player_1";

	[Header("Movement Properties")]
	//Movement
	public float turnSpeed = 6f;
	public float jumpForce = 18f;
	public float dashForce = 10f;
	public const float dashCDDef = .7f; 
	private float dashCD;

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
	public AudioClip[] Steps;
	public AudioClip[] Jummy;
	public AudioClip DeathSound;
	public AudioSource SoundSource;
	public AudioSource StepSource;


	//Hidden variables
	private float
		timer, Step_R_timer, Step_L_timer,
		MouseYAxisArms, MouseXAxisArms, MouseYAxisBody,
		x, y, cX, cY;

	private bool
		WalkForward, WalkBackward,
		StepRight, StepLeft, Alert_Leg_Right,
		Alert_Leg_Left, balanced = true, GettingUp,
		ResetPose, isRagdoll, usingLeft, ableToStop = true,
		jumpAxisUsed, reachLeftAxisUsed, reachRightAxisUsed;

	[HideInInspector]
	public bool usingController;

	[HideInInspector]
	public bool
		jumping, isJumping, inAir,
		attacking, punchingLeft;

	private Camera cam;
	private Vector3 Direction;
	private Vector3 CenterOfMassPoint;
	private Vector3 pPos;
	private float hitCoolDown;

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

	[HideInInspector]
	public bool waitForDisco = false;
	
	private bool metralletaCheat = false;
	private bool lockedKeyb = false;



	//-------------------------------------------------------------
	//--Calling Functions
	//-------------------------------------------------------------

	 /*
	void TestQuaternion(ConfigurableJoint j1, ConfigurableJoint j3, ConfigurableJoint j4, Vector3 rot1, Vector3 rot3, Vector3 rot4)
    {
		//j1.targetRotation = Quaternion.Euler(rot1);
		j3.targetRotation = Quaternion.Euler(rot3);
		j4.targetRotation = Quaternion.Euler(rot4);
    }
	 */

	//---Setup---//
	//////////////
	void Awake()
	{
		PlayerSetup();
		YoureDead += OnDead;
		life = maxLife;
	}

    private void Start()
    {
		system = Instantiate(particleObj).GetComponent<ParticleSystem>();
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Respawn"))
        {
			g.GetComponent<SpawnPoint>().AddPlayer(Root.transform);
		}
		//StressManagerSingleton.Instance.SetBar(id, Root.transform);
    }
    public void SetUp(GameObject hUI, int i, int playerN, bool lockedKeyboard = false)
	{
		id = i;
		bool addSetup = false;
		string added = "";
		if(lockedKeyboard)
		{
			lockedKeyb = true;
			if (id == 1)
			{
				added = "0";
				forwardBackward += added;
				leftRight += added;
				jump += added;
				left += added;
				attack += added;
				drop += added;
				interact += added;
				dash += added;
				detector.SetUp(added);
			}
			else if (id == 5)
			{
				addSetup = true;
				added = "1";
				forwardBackward += added;
				leftRight += added;
				jump += added;
				left += added;
				attack += added;
				drop += added;
				interact += added;
				lookX += added;
				lookY += added;
				dash += added;
			}
		}
		if (id > 1 && id < 5)
		{
			addSetup = true;
			added = "" + id;
			forwardBackward += added;
			leftRight += added;
			jump += added;
			left += added;
			attack += added;
			drop += added;
			interact += added;
			lookX += added;
			lookY += added;
			dash += added;
		}
		if(addSetup)
        {
			usingController = true;
			LayerMask layer = 10;
			switch (playerN)
			{
				case 1:
					character = Characters.Player2;
					layer = 11;
					break;
				case 2:
					character = Characters.Player3;
					layer = 12;
					break;
				case 3:
					character = Characters.Player4;
					layer = 13;
					break;
			}
			foreach (Transform g in GetComponentsInChildren<Transform>())
			{
				g.gameObject.layer = layer;
			}
			detector.SetUp(added);
		}
		hUD = hUI.GetComponent<HealthHUD>();
		detector.GetComponent<WeaponDetection>().healthUI = hUI.GetComponent<HealthHUD>();
	}

	public void GetWeapons(out Weapons w1, out Weapons w2)
    {
		detector.GetWeapons(out w1, out w2);
    }

	void ChangeControls()
    {
		if(!lockedKeyb)
        {
			float toControl = Input.GetAxis("Horizontal1") + Input.GetAxis("Vertical1") + Input.GetAxis(lookX) + Input.GetAxis(lookY);
			if (usingController && (Abs(Input.GetAxis("Mouse X")) + Abs(Input.GetAxis("Mouse Y")) != 0))
            {
				usingController = false;
            }
			else if(!usingController && (toControl != 0))
            {
				usingController = true;
			}
        }
    }

	//---Updates---//
	////////////////
	void Update()
	{
		ChangeControls();
		if (IsDead())
		{
			detector.DropAllWeapons();
			this.enabled = false;
		}
		invTime -= Time.deltaTime;
		dashCD -= Time.deltaTime;
		if (attacking && !Object.ReferenceEquals(weapon, null) && weapon.kind.Equals(Weapons.Bow)) chargingTime += Time.deltaTime * 1.4f;
		x = Input.GetAxis(leftRight);
		y = Input.GetAxis(forwardBackward);
		if(usingController)
        {
			cX = Input.GetAxis(lookY);
			cY = -Input.GetAxis(lookX);
		}
		else
		{
			Vector3 dir = pPos.normalized;
			cX = dir.x;
			cY = dir.y;
		}
		if (Input.GetKeyDown(KeyCode.Y)) metralletaCheat = !metralletaCheat;
		if(hitCoolDown > 0)
        {
			hitCoolDown -= Time.deltaTime;
        }
		if (!inAir)
		{
			PlayerMovement();
		}

		PlayerDash();

		if (canPunch)
		{
			PlayerPunch();
		}
		//PlayerReach();

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
		//TestQuaternion(APR_Parts[1].GetComponent<ConfigurableJoint>(), APR_Parts[3].GetComponent<ConfigurableJoint>(), APR_Parts[4].GetComponent<ConfigurableJoint>(), rota1, rota3, rota4);
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
		if (Physics.Raycast(ray, out hit, balanceHeight, (1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Enemies"))) 
			&& !inAir && !isJumping && !reachRightAxisUsed && !reachLeftAxisUsed)
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
		else if (!Physics.Raycast(ray, out hit, balanceHeight, (1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Enemies"))))
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
		/*
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
		float x = Input.GetAxis(leftRight);
		float y = Input.GetAxis(forwardBackward);
		float cX = Input.GetAxis(lookY);
		float cY = Input.GetAxis(lookY);
		if (x * cX > 0)
        {
			WalkForward = true;
			WalkBackward = false;
        }
		else if (x * cX < 0)
		{
			WalkForward = false;
			WalkBackward = true;
		}
		else if (y * cY > 0)
		{
			WalkForward = true;
			WalkBackward = false;
		}
		else if (y * cY < 0)
		{
			WalkForward = false;
			WalkBackward = true;
		}*/
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



	float Abs(float i)
	{
		return ((i < 0) ? -i : i);
	}

	//---Player Movement---//
	////////////////////////
	void PlayerMovement()
	{
		/*
		float x = Input.GetAxis(leftRight);
		float y = Input.GetAxis(forwardBackward);
		float cX = Input.GetAxis(lookY);
		float cY = -Input.GetAxis(lookX);*/
		
		if(!isRagdoll)
		{
			Direction = new Vector3(x, 0.0f, y).normalized;
			APR_Parts[0].transform.GetComponent<Rigidbody>().velocity = Vector3.Lerp(APR_Parts[0].transform.GetComponent<Rigidbody>().velocity, (Direction * Abs(moveSpeed)) + Vector3.up * APR_Parts[0].transform.GetComponent<Rigidbody>().velocity.y, 0.8f);
		}
		if (x != 0 || y != 0 && balanced)
		{

			if (x * cX > 0 || y * cY > 0)
			{
				WalkForward = true;
				WalkBackward = false;
			}
			else if (x * cX < 0 || y * cY < 0)
			{
				WalkForward = false;
				WalkBackward = true;
			}
			else
			{
				WalkForward = true;
				WalkBackward = true;
			}/*
			if (!WalkForward && !moveAxisUsed)
			{
				WalkForward = true;
				moveAxisUsed = true;
				isKeyDown = true;
			}*/
		}

		else if (x == 0 && y == 0)
		{
			WalkForward = false;
			WalkBackward = false;
			//moveAxisUsed = false;
			//isKeyDown = false;
		}
	}


	void PlayerDash()
    {
		if(Input.GetButtonDown(dash) && !isRagdoll && dashCD <= -dashCDDef)
		{
			ActivateRagdoll();
			SetInvencibleTime();
			dashCD = dashCDDef;
			Root.GetComponent<Rigidbody>().AddForce(Root.transform.forward * dashForce, ForceMode.Impulse);
			Head.GetComponent<Rigidbody>().AddForce(Head.transform.forward * 1.5f * dashForce, ForceMode.Impulse);
		}
    }



	//---Player Rotation---//
	////////////////////////
	void PlayerRotation()
	{
		if (usingController)
        {
			if(Input.GetAxis(lookX) != 0 || Input.GetAxis(lookY) != 0)
            {
				pPos = new Vector3(Input.GetAxis(lookY), -Input.GetAxis(lookX), 0f);
			}
			else if (x!= 0 || y != 0)
            {
				pPos = new Vector3(x, y, 0f);
            }
			usingController = true;
		}
		else
		{
			pPos = Input.mousePosition ;
			pPos -= cam.WorldToScreenPoint(Root.transform.position);
		}



		//Camera Direction
		//Turn with camera
		//var lookPos = cam.transform.forward; CAMBIO
		if (/*(Input.GetAxis(leftRight) != 0 || Input.GetAxis(forwardBackward) != 0)*/!isRagdoll/**/)
		{
			var lookPos = new Vector3(-pPos.x, 0.0f, pPos.y);
			//new Vector3(-Input.GetAxis(leftRight), 0.0f, Input.GetAxis(forwardBackward)) * 5;
			var rotation = Quaternion.identity;
			if (lookPos != Vector3.zero)
			{
				rotation = Quaternion.LookRotation(lookPos) * Quaternion.Euler(0,-cam.transform.rotation.eulerAngles.y,0);
			}
			//APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Slerp(APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation, rotation, Time.deltaTime * turnSpeed);
			APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation = Quaternion.RotateTowards(APR_Parts[0].GetComponent<ConfigurableJoint>().targetRotation, rotation, Time.deltaTime * turnSpeed);
		}
	}



	//---Player GetUp & Jumping---//
	///////////////////////////////
	void PlayerGetUpJumping()
	{
		if (Input.GetAxis(jump) > 0)
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
			ResetPose = true;
			inAir = false;
		}
	}



	//---Player Reach--//
	////////////////////
	void PlayerReach()
	{
		/*
		//Body Bending
		if (1 != 1)
		{
			if (MouseYAxisBody <= 0f && MouseYAxisBody >= -0.1f)
			{
				MouseYAxisBody = MouseYAxisBody + (Input.GetAxis("Mouse Y") / reachSensitivity);
			}

			else if (MouseYAxisBody > 0f)
			{
				MouseYAxisBody = 0f;
			}

			else if (MouseYAxisBody < -0.1f)
			{
				MouseYAxisBody = -0.1f;
			}

			APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(MouseYAxisBody, 0, 0, 1);
		}
		*/

		//Reach Left
		if (Input.GetButton(drop) && !punchingLeft)
		{

			if (!reachLeftAxisUsed)
			{
				//Adjust Left Arm joint strength
				APR_Parts[5].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
				APR_Parts[5].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;
				APR_Parts[6].GetComponent<ConfigurableJoint>().angularXDrive = ReachStiffness;
				APR_Parts[6].GetComponent<ConfigurableJoint>().angularYZDrive = ReachStiffness;

				//Adjust body joint strength
				//APR_Parts[1].GetComponent<ConfigurableJoint>().angularXDrive = CoreStiffness;
				//APR_Parts[1].GetComponent<ConfigurableJoint>().angularYZDrive = CoreStiffness;

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
		/*
		if (!Input.GetButton(drop) && !punchingLeft)
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


		/*
		//Reach Right
		if (Input.GetButton(reachRight) && !attacking)
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
		if (!Input.GetButton(reachRight) && !attacking)
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
		*/

	}


	public void PrepareHit()
	{
		switch(weapon.kind)
        {
			case Weapons.Bow:
				chargingTime = .45f;
				weapon.PrepareHit(APR_Parts[1].GetComponent<ConfigurableJoint>(), APR_Parts[3].GetComponent<ConfigurableJoint>(), APR_Parts[4].GetComponent<ConfigurableJoint>());
				weapon.GetComponent<BowScript>().PrepareLeftHand(APR_Parts[5].GetComponent<ConfigurableJoint>(), APR_Parts[6].GetComponent<ConfigurableJoint>());
				break;
			default:
				weapon.PrepareHit(APR_Parts[1].GetComponent<ConfigurableJoint>(), APR_Parts[3].GetComponent<ConfigurableJoint>(), APR_Parts[4].GetComponent<ConfigurableJoint>());
				break;
        }
	}
	//---Player Punch---//
	/////////////////////
	private IEnumerator JustDid(float tts)
    {
		yield return new WaitForSeconds(tts);
		ableToStop = true;
    }
	void PlayerPunch()
	{

		//punch right
		if (ableToStop && !attacking && !inAir && !isRagdoll && (Input.GetButton(attack) || Input.GetAxis(attack) > 0) && hitCoolDown <= 0)
		{
			ableToStop = false;
			attacking = true;
			float timeToStart = .15f;
			if (!Object.ReferenceEquals(weapon, null))
			{
				PrepareHit();
				timeToStart = weapon.timeToHit;
			}
			else
			{
				//Right hand punch pull back pose
				APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
				APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.62f, -0.51f, 0.02f, 1);
				APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(1.31f, 0.5f, -0.5f, 1);
			}
			StartCoroutine(JustDid(timeToStart));
		}

		if (ableToStop && attacking && ((!Input.GetButton(attack) && Input.GetAxis(attack) == 0) || (metralletaCheat && ((Input.GetButton(attack) || Input.GetAxis(attack) > 0)))))
		{
			attacking = false;
			if (!Object.ReferenceEquals(weapon, null))
			{
				hitCoolDown = weapon.weaponCoolDown;
				if (!metralletaCheat)
				{
					//invTime = .1f;
					weapon.Hit(APR_Parts[1].GetComponent<ConfigurableJoint>(), APR_Parts[3].GetComponent<ConfigurableJoint>(), APR_Parts[4].GetComponent<ConfigurableJoint>(), punchForce);
				}
				Vector3 lookPos;
				switch (weapon.kind)
				{
					case Weapons.Bow:
						if (metralletaCheat)
						{
							chargingTime = 1.2f;
							hitCoolDown = .001f;
						}
						else
						{
							ResetLeftArm();
						}
						//var lookPos = new Vector3(pPos.x, 0.0f, pPos.y);lookPos.normalized
						lookPos = new Vector3(Root.transform.forward.x, 0f, Root.transform.forward.z);
						weapon.Shoot(lookPos.normalized, character, chargingTime);
						if (!metralletaCheat) chargingTime = .45f;
						break;
					case Weapons.Axe:
						RightHand.AddForce(APR_Parts[0].transform.forward * punchForce*2, ForceMode.Impulse);
						APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce*2, ForceMode.Impulse);
						break;
					case Weapons.Discobolus:
						lookPos = new Vector3(Root.transform.forward.x, 0, Root.transform.forward.z);
						detector.ThrowDisco(lookPos);
						waitForDisco = true;
						break;
					case Weapons.Spear:
						lookPos = new Vector3(Root.transform.forward.x, 0, Root.transform.forward.z);
						weapon.GetComponent<SpearScript>().Hit(APR_Parts[1].GetComponent<ConfigurableJoint>(), 
							APR_Parts[3].GetComponent<ConfigurableJoint>(), APR_Parts[4].GetComponent<ConfigurableJoint>(), lookPos, punchForce);
						break;
					default:
						RightHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
						APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
						break;
				}
			}
			else
			{
				hitCoolDown = .5f;
				invTime = .1f;
				//Right hand punch release pose
				APR_Parts[1].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
				APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.74f, 0.04f, 0f, 1);
				APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = new Quaternion(0.2f, 0, 0, 1);
				//Right hand punch force
				RightHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
				APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
				APR_Parts[1].GetComponent<Rigidbody>().AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
			}
			StartCoroutine(DelayCoroutine());
			IEnumerator DelayCoroutine()
			{
				yield return new WaitForSeconds(0.3f);
				if (!metralletaCheat || (!Input.GetButton(attack) && Input.GetAxis(attack) == 0))
				{
					APR_Parts[3].GetComponent<ConfigurableJoint>().targetRotation = UpperRightArmTarget;
					APR_Parts[4].GetComponent<ConfigurableJoint>().targetRotation = LowerRightArmTarget;
				}
			}
		}

		//punch left
		if(!punchingLeft && (Input.GetButton(left) || (!usingLeft && Input.GetAxis(left) > 0)))
		{
			usingLeft = true;
			punchingLeft = true;
			if (!Object.ReferenceEquals(weapon, null))
			{
				switch (weapon.kind)
				{
					case Weapons.SwordNShield:
						weapon.GetComponent<SwordAndShield>().ShieldDefense(APR_Parts[5].GetComponent<ConfigurableJoint>(), APR_Parts[6].GetComponent<ConfigurableJoint>());
						break;
					case Weapons.Bow:
						break;
				};
			}
		}
        
		if(punchingLeft && (!Input.GetButton(left) && Input.GetAxis(left) == 0))
		{
			usingLeft = false;
			punchingLeft = false;
            if(!Object.ReferenceEquals(weapon, null))
            {
                switch (weapon.kind)
				{
					case Weapons.SwordNShield:
						break;
					case Weapons.Bow:
						break;
				};
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
				yield return new WaitForSeconds(0.05f);
				if(!Input.GetButton(left))
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
			float local11z = APR_Parts[11].transform.localPosition.z;
			float local12z = APR_Parts[12].transform.localPosition.z;
			float local11x = APR_Parts[11].transform.localPosition.x;
			float local12x = APR_Parts[12].transform.localPosition.x;
			if (WalkForward)
			{
				Alert_Leg_Left = false;
				Alert_Leg_Right = false;
				//right leg
				if ((local11z < local12z || local11x > 0) && !StepLeft && !Alert_Leg_Right)
				{
					StepRight = true;
					Alert_Leg_Right = true;
					Alert_Leg_Left = true;
				}

				//left leg
				if ((local11z > local12z || local12x < 0) && !StepRight && !Alert_Leg_Left)
				{
					StepLeft = true;
					Alert_Leg_Left = true;
					Alert_Leg_Right = true;
				}
			}

			if (WalkBackward)
			{
				//right leg
				if ((local11z > local12z || local11x > 0) && !StepLeft && !Alert_Leg_Right)
				{
					StepRight = true;
					Alert_Leg_Right = true;
					Alert_Leg_Left = true;
				}

				//left leg
				if ((local11z < local12z || local12x < 0) && !StepRight && !Alert_Leg_Left)
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


	public bool IsRagdoll()
    {
		return isRagdoll;
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
		if (ResetPose && !jumping && !attacking && !(hitCoolDown > 0))
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

	public void OnDead(object s, System.EventArgs e) {
		ActivateRagdoll();
		StartCoroutine(Kill());
		IEnumerator Kill()
		{
			yield return new WaitForSeconds(3f);
			foreach (Collider c in GetComponentsInChildren<Collider>())
			{
				c.enabled = false;
			}
			foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>())
			{
				r.velocity = Vector3.zero;
				r.useGravity = false;
				r.velocity = Vector3.down * 2;
			}
			cam.gameObject.GetComponent<CameraControl>().RemovePlayer(character);
			Destroy(system);
			yield return new WaitForSeconds(2f);
			Destroy(this.gameObject);
		}
	}

	public void ResetLeftArm()
	{
		APR_Parts[5].GetComponent<ConfigurableJoint>().targetRotation = UpperLeftArmTarget;
		APR_Parts[6].GetComponent<ConfigurableJoint>().targetRotation = LowerLeftArmTarget;
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