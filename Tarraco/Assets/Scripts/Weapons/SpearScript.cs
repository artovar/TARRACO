using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : WeaponScript
{
    public Transform spear;
    public override void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(-0.46f, .12f, 0.46f, 1f);
        c.targetRotation = new Quaternion(0.69f, .27f, .15f, 1f);
        spear.position -= spear.forward * .5f;
    }
    public override void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, float force)
    {
        //Vacio adrede
    }
    public void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, Vector3 dir, float force)
    {
        a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(.12f, -1.36f, .46f, 1f);
        c.targetRotation = new Quaternion(-.66f, .43f, .48f, 1f);
        DealDamage();

        GetComponent<Rigidbody>().AddForceAtPosition((dir + Vector3.up) * 1.2f * force, forcePoint.position, ForceMode.Impulse);
        spear.position += spear.forward * .5f;
    }
}