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
    public float timeToHit;
    public Transform forcePoint;
    protected List<bool> dropQueue = new List<bool>();
    protected bool destroyed;

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
        transform.position = rHand.position + rHand.right * .2f;
        transform.GetComponent<FixedJoint>().connectedBody = null;
        transform.GetComponent<FixedJoint>().breakForce = 0f;
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().velocity = rHand.GetComponent<Rigidbody>().velocity;
        SetOnFloorColliders();
        transform.tag = "Weapon";
        StartCoroutine(DestroyWeapon());
    }
    public void DestroyAfterSpawning()
    {
        StartCoroutine(DestroyWeapon());
    }
    public virtual void ThrowWeapon(Transform rHand)
    {
        transform.position = rHand.position + rHand.right * .2f;
        transform.GetComponent<Rigidbody>().AddForceAtPosition(forcePoint.forward * 50 * 1.5f, forcePoint.position, ForceMode.Impulse);
        
        //transform.GetComponent<FixedJoint>().connectedBody = null;
        transform.GetComponent<FixedJoint>().breakForce = 0f;
        transform.GetComponent<Rigidbody>().useGravity = true;
        //transform.GetComponent<Rigidbody>().velocity = Vector3.forward;
        
        //SetOnFloorColliders();
        transform.tag = "Weapon";
        //StartCoroutine(DestroyWeapon());
    }
    protected IEnumerator DestroyWeapon()
    {
        dropQueue.Add(true);
        yield return new WaitForSeconds(12f);
        if(tag.Equals("Weapon") && dropQueue[0] && GameController.Instance.inGame)
        {
            foreach(Collider col in onFloorCol)
            {
                col.enabled = false;
            }
            foreach(Collider col in onHandCol)
            {
                col.enabled = false;
            }
            destroyed = true;
        }
        dropQueue.RemoveAt(0);
        if (destroyed)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = Vector3.down;
            yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
        }
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
    public virtual void Shoot(Vector3 direction, Characters cType, float mult) {}
    public virtual void MakeCurve(Vector3 direction) {}
}