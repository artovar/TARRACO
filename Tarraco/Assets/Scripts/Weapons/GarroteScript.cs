using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarroteScript : WeaponScript
{
    public override void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        /*a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(-0.62f, -0.51f, 0.02f, 1);
        c.targetRotation = new Quaternion(1.31f, 0.5f, -0.5f, 1);*/
        a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(1.58f, -2.76f, .46f, 1f);
        c.targetRotation = new Quaternion(.37f, 0f, .27f, 1f);
    }
    public override void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, float force)
    {
        a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(.49f, -.31f, .46f, 1f);
        c.targetRotation = new Quaternion(0f, .1f, .27f, 1f);

        /*a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(.79f, -.23f, .33f, .46f);
        c.targetRotation = new Quaternion(0f, -.63f, .53f, .56f);*/

        GetComponent<Rigidbody>().AddForceAtPosition(forcePoint.right * force, forcePoint.position, ForceMode.Impulse);
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
