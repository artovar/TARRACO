using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAndShield : WeaponScript
{
    [SerializeField]
    Collider[] onHandCol;
    [SerializeField]
    Collider[] onFloorCol;

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
    public override void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        /*a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(0.74f, 0.04f, 0f, 1);
        c.targetRotation = new Quaternion(0.2f, 0, 0, 1);*/
        a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(0.150000006f, -0.439999998f, 0.649999976f, 0.360000014f);
        c.targetRotation = new Quaternion(-0.439999998f, 0.5f, 0.439999998f, 1f);
    }

    public override void SetOnHandColliders()
    {
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
        foreach (Collider c in onHandCol)
        {
            c.enabled = false;
        }
        foreach (Collider c in onFloorCol)
        {
            c.enabled = true;
        }
    }
    public override void GetWeapon(Transform rHand, Transform lHand)
    {
        tag = "GrabbedWeapon";
        sword.position = rHand.position;
        sword.rotation = rHand.rotation;
        shield.position = lHand.position;
        shield.rotation = lHand.rotation*Quaternion.Euler(50,90,0);
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
        tag = "Weapon";
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
    }

    public override void SendToBack(Transform back)
    {
        sword.position = back.position;
        shield.position = back.position;
        sword.rotation = back.rotation;
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
}
