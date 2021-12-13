using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    [SerializeField]
    protected Collider[] onHandCol;
    [SerializeField]
    protected Collider[] onFloorCol;
    public Weapons kind;
    public Characters owner;
    public int damageDealed;
    public float weaponCoolDown;
    public Transform forcePoint;
    protected List<bool> dropQueue = new List<bool>();

    public abstract void SetOnHandColliders();
    public abstract void SetOnFloorColliders();
    public abstract void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c);
    public abstract void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, float force);
    public virtual void GetWeapon(Transform rHand, Transform lHand, Characters character)
    {
        for (int i = 0; i < dropQueue.Count; i++)
        {
            if (dropQueue[i])
            {
                dropQueue[i] = false;
                break;
            }
        }
        owner = character;
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
        StartCoroutine(DestroyWeapon());
    }
    protected IEnumerator DestroyWeapon()
    {
        print("I want to destroy this");
        dropQueue.Add(true);
        yield return new WaitForSeconds(7f);
        if(tag.Equals("Weapon") && dropQueue[0])
        {
            foreach(Collider col in onFloorCol)
            {
                col.enabled = false;
            }
            foreach(Collider col in onHandCol)
            {
                col.enabled = false;
            }
            Destroy(this.gameObject);
        }
        dropQueue.RemoveAt(0);
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