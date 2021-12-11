using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public Weapons kind;
    public float weaponCoolDown;
    public abstract void SetOnHandColliders();
    public abstract void SetOnFloorColliders();
    public abstract void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c);
    public abstract void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c);
    public virtual void GetWeapon(Transform rHand, Transform lHand)
    {
        tag = "GrabbedWeapon";
        transform.position = rHand.position;
        transform.rotation = rHand.rotation;
        transform.gameObject.AddComponent<FixedJoint>();
        transform.GetComponent<FixedJoint>().connectedBody = rHand.GetComponent<Rigidbody>();
        transform.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
        transform.GetComponent<Rigidbody>().useGravity = false;
        SetOnHandColliders();
    }
    public virtual void DropWeapon(Transform rHand)
    {
        transform.position = rHand.position;
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hitInfo, 1, 1 << LayerMask.NameToLayer("Ground")))
        {
            transform.position = hitInfo.point + Vector3.up * .2f;
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
        }
        else
        {
            transform.position -= Vector3.up * .1f;
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
        }
        transform.GetComponent<FixedJoint>().connectedBody = null;
        transform.GetComponent<FixedJoint>().breakForce = 0f;
        transform.GetComponent<Rigidbody>().useGravity = true;
        SetOnFloorColliders();
        transform.tag = "Weapon";
    }
    public virtual void SendToBack(Transform back)
    {
        transform.position = back.position;
        transform.rotation = back.rotation;
        GetComponent<FixedJoint>().connectedBody = back.GetComponent<Rigidbody>();
    }
    public virtual void BringFromBack(Transform rHand, Transform lHand)
    {
        transform.position = rHand.position;
        transform.rotation = rHand.rotation;
        GetComponent<FixedJoint>().connectedBody = rHand.GetComponent<Rigidbody>();
    }
    public virtual void Shoot(Vector3 direction, Characters cType) {}
}