using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarroteScript : WeaponScript
{
    public override void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(1.58f, -2.76f, .46f, 1f);
        c.targetRotation = new Quaternion(.37f, 0f, .27f, 1f);
    }
    public override void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, float force)
    {
        a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(.49f, -.31f, .46f, 1f);
        c.targetRotation = new Quaternion(0f, .1f, .27f, 1f);
        DealDamage();

        GetComponent<Rigidbody>().AddForceAtPosition(forcePoint.right * force, forcePoint.position, ForceMode.Impulse);
    }
}
