using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BowScript : WeaponScript
{
    [SerializeField]
    float arrowForce;
    [SerializeField]
    Collider[] onHandCol;
    [SerializeField]
    Collider[] onFloorCol;
    [SerializeField]
    GameObject arrow;
    [SerializeField]
    GameObject arrowE;

    public override void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        /*a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
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
        this.gameObject.layer = LayerMask.NameToLayer("Weapons");

        foreach (Transform g in GetComponentsInChildren<Transform>())
        {
            g.gameObject.layer = LayerMask.NameToLayer("Weapons");
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
        this.gameObject.layer = LayerMask.NameToLayer("Player_1");

        foreach (Transform g in GetComponentsInChildren<Transform>())
        {
            g.gameObject.layer = LayerMask.NameToLayer("Player_1");
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

    public override void Shoot(Vector3 direction, Characters cType)
    {
        GameObject arrowClone;
        switch (cType)
        {
            case Characters.Enemy:
                arrowClone = Instantiate(arrowE, transform.position, Quaternion.LookRotation(direction, Vector3.up));
                break;
            default:
                arrowClone = Instantiate(arrow, transform.position, Quaternion.LookRotation(direction, Vector3.up));
                break;
        };
        arrowClone.GetComponent<Rigidbody>().velocity = direction * arrowForce;
        arrow.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Destroy(arrowClone, 3);
    }
    public override void SendToBack(Transform back)
    {
        transform.position = back.position + back.forward/2;
        transform.rotation = back.rotation * Quaternion.Euler(0,0,180);
        GetComponent<FixedJoint>().connectedBody = back.GetComponent<Rigidbody>();
    }
}
