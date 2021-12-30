using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAndShield : WeaponScript
{
    [SerializeField]
    Transform sword;
    [SerializeField]
    Transform shield;
    [SerializeField]
    Transform swordPoint;
    [SerializeField]
    Transform shieldPoint;


    public override void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        /* a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
         b.targetRotation = new Quaternion(-0.62f, -0.51f, 0.02f, 1);
         c.targetRotation = new Quaternion(1.31f, 0.5f, -0.5f, 1);*/
        a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(-0.360000014f, -0.939999998f, 0.560000002f, 1.38f);
        c.targetRotation = new Quaternion(0.709999979f, -0.610000014f, 0.839999974f, 1f);
    }
    public override void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, float force)
    {
        /*a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(0.150000006f, -0.439999998f, 0.649999976f, 0.360000014f);
        c.targetRotation = new Quaternion(-0.439999998f, 0.5f, 0.439999998f, 1f);*/

        a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(.79f, -.23f, .33f, .46f);
        c.targetRotation = new Quaternion(0f, -.63f, .53f, .56f);
        sword.GetComponent<Rigidbody>().AddForceAtPosition(forcePoint.right * force, forcePoint.position, ForceMode.Impulse);
    }

    public override void SetOnHandColliders()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Weapons");

        foreach (Transform g in GetComponentsInChildren<Transform>())
        {
            g.gameObject.layer = LayerMask.NameToLayer("Weapons");
        }
        switch(owner)
        {
            case Characters.Enemy:
                shield.GetComponentInChildren<ShieldDetector>().gameObject.layer = LayerMask.NameToLayer("ShieldLayer_E");
                break;
            default:
                shield.GetComponentInChildren<ShieldDetector>().gameObject.layer = LayerMask.NameToLayer("ShieldLayer");
                break;
        }
        foreach (Collider c in onHandCol)
        {
            c.enabled = true;
        }
        foreach (Collider c in onFloorCol)
        {
            c.enabled = false;
        }
    }

    public override void SetOnFloorColliders()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");

        foreach (Transform g in GetComponentsInChildren<Transform>())
        {
            g.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        foreach (Collider c in onHandCol)
        {
            c.enabled = false;
        }
        foreach (Collider c in onFloorCol)
        {
            c.enabled = true;
        }
    }
    public override void GetWeapon(Transform rHand, Transform lHand, Characters character)
    {
        for (int i = 0; i < dropQueue.Count; i++)
        {
            if (dropQueue[i])
            {
                dropQueue[i] = false;
                break;
            }
        }
        owner = character;
        tag = "GrabbedWeapon";
        sword.position = rHand.position;
        sword.rotation = rHand.rotation;
        shield.position = lHand.position;
        shield.rotation = lHand.rotation * Quaternion.Euler(50, 90, 0);
        sword.gameObject.AddComponent<FixedJoint>();
        shield.gameObject.AddComponent<FixedJoint>();
        sword.GetComponent<FixedJoint>().connectedBody = rHand.GetComponent<Rigidbody>();
        shield.GetComponent<FixedJoint>().connectedBody = lHand.GetComponent<Rigidbody>();
        sword.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
        shield.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
        sword.GetComponent<Rigidbody>().useGravity = false;
        shield.GetComponent<Rigidbody>().useGravity = false;
        transform.GetComponent<Rigidbody>().useGravity = false;
        SetOnHandColliders();
    }
    public override void DropWeapon(Transform rHand)
    {
        transform.position = rHand.position;

        sword.GetComponent<FixedJoint>().connectedBody = null;
        shield.GetComponent<FixedJoint>().connectedBody = null;
        Destroy(sword.GetComponent<FixedJoint>());
        Destroy(shield.GetComponent<FixedJoint>());
        Destroy(sword.GetComponent<Rigidbody>());
        Destroy(shield.GetComponent<Rigidbody>());

        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hitInfo, 1, 1 << LayerMask.NameToLayer("Ground")))
        {
            transform.position = hitInfo.point + Vector3.up * .1f;
        }
        else
        {
            transform.position -= Vector3.up * .1f;
        }
        transform.rotation = Quaternion.Euler(0, 0, 90);
        sword.position = swordPoint.position;
        sword.rotation = transform.rotation * Quaternion.Euler(0, 0, 90);
        shield.position = shieldPoint.position;
        shield.rotation = transform.rotation * Quaternion.Euler(0, 0, 90);

        transform.GetComponent<Rigidbody>().useGravity = true;
        SetOnFloorColliders();
        tag = "Weapon";
        StartCoroutine(DestroyWeapon());
    }

    public override void SendToBack(Transform back)
    {
        sword.position = back.position; 
        sword.rotation = back.rotation;
        shield.position = back.position + back.forward /2f;
        shield.rotation = back.rotation * Quaternion.Euler(0, 0, 90);
        sword.GetComponent<FixedJoint>().connectedBody = back.GetComponent<Rigidbody>();
        shield.GetComponent<FixedJoint>().connectedBody = back.GetComponent<Rigidbody>();
    }
    public override void BringFromBack(Transform rHand, Transform lHand)
    {
        sword.position = rHand.position;
        sword.rotation = rHand.rotation;
        shield.position = lHand.position;
        shield.rotation = lHand.rotation * Quaternion.Euler(50, 90, 0);
        sword.GetComponent<FixedJoint>().connectedBody = rHand.GetComponent<Rigidbody>();
        shield.GetComponent<FixedJoint>().connectedBody = lHand.GetComponent<Rigidbody>();
    }

    public void ShieldDefense(ConfigurableJoint a, ConfigurableJoint b)
    {
        a.targetRotation = new Quaternion(-.64f, -.21f, -.28f, 1f);
        b.targetRotation = new Quaternion(-0.73f, -0.37f, -0.43f, 1f);
    }
}