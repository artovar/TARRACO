using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : WeaponScript
{
    public Transform spear;
    public override void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        /*a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(-0.62f, -0.51f, 0.02f, 1);
        c.targetRotation = new Quaternion(1.31f, 0.5f, -0.5f, 1);*/
        a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(-0.46f, .12f, 0.46f, 1f);
        c.targetRotation = new Quaternion(0.69f, .27f, .15f, 1f);
        spear.position -= spear.forward;
    }
    public override void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, float force)
    {
        a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(.12f, -1.36f, .46f, 1f);
        c.targetRotation = new Quaternion(-.66f, .43f, .48f, 1f);
        GetComponent<Rigidbody>().AddForceAtPosition((forcePoint.right + Vector3.up) * 1.2f * force, forcePoint.position, ForceMode.Impulse);
        spear.position += spear.forward;
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
        spear.position = transform.position;
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