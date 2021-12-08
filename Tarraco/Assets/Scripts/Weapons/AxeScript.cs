using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : WeaponScript
{
    [SerializeField]
    Collider[] onHandCol;
    [SerializeField]
    Collider[] onFloorCol;

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
}