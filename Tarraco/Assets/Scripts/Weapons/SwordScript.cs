using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : WeaponScript
{
    public override void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(-0.36f, -0.94f, 0.56f, 1f);
        c.targetRotation = new Quaternion(0.71f, -0.61f, 0.27f, 1f);
    }
    public override void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, float force)
    {
        a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(.79f, -.23f, .33f, .46f);
        c.targetRotation = new Quaternion(0f, -.63f, .53f, .56f);
        DealDamage();

        GetComponent<Rigidbody>().AddForceAtPosition(forcePoint.right * force, forcePoint.position, ForceMode.Impulse);
    }
}