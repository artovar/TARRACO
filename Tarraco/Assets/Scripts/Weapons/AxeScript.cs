using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour, WeaponScript
{
    [SerializeField]
    Collider[] onHandCol;
    [SerializeField]
    Collider[] onFloorCol;

    public void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        print("preparing");
        a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(-0.62f, -0.51f, 0.02f, 1);
        c.targetRotation = new Quaternion(1.31f, 0.5f, -0.5f, 1);
    }
    public void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        print("Hitting");
        a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(0.74f, 0.04f, 0f, 1);
        c.targetRotation = new Quaternion(0.2f, 0, 0, 1);
    }

    public void SetOnHandColliders()
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

    public void SetOnFloorColliders()
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
}
