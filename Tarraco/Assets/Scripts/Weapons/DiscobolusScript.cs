using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscobolusScript : WeaponScript
{
    Rigidbody rb;
  
    private Vector3 axisToRotate;
    private bool thrown = false;
    private bool stop = false;
    private Vector3 dir;

    private bool rotated;
    private float rotacion = 0f;
    private float rotacionEx = 0f;
    private float anglesRotated = 0f;

    private float rotationSpeed = 10f;
    private bool throwing;

    [SerializeField]
    private Collider onAirCollider;

    public void PrepareThrowing()
    {
        throwing = true;
    }

    public override void DropWeapon(Transform rHand)
    {
        transform.position = rHand.position + rHand.right * .2f;

        rb.useGravity = true;
        transform.GetComponent<FixedJoint>().connectedBody = null;
        transform.GetComponent<FixedJoint>().breakForce = 0f;

        SetOnFloorColliders();
        if(!throwing)
        {
            transform.GetComponent<Rigidbody>().useGravity = true;
            transform.GetComponent<Rigidbody>().velocity = rHand.GetComponent<Rigidbody>().velocity;
            transform.tag = "Weapon";
            StartCoroutine(DestroyWeapon());
        }
        throwing = false;
    }

    public override void SetOnHandColliders()
    {
        onAirCollider.enabled = false;
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
        onAirCollider.enabled = false;
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

    public override void SendToBack(Transform back)
    {
        transform.position = back.position;
        //shield.position = back.position + back.forward / 2f;
        transform.rotation = back.rotation * Quaternion.Euler(0, 0, 90);
        transform.GetComponent<FixedJoint>().connectedBody = back.GetComponent<Rigidbody>();
    }

    public override void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c) {
        a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(-0.360000014f, -0.939999998f, 0.560000002f, 1.38f);
        c.targetRotation = new Quaternion(0.709999979f, -0.610000014f, 0.839999974f, 1f);
    }

    public override void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, float force) {
        a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        b.targetRotation = new Quaternion(0.150000006f, -0.439999998f, 0.649999976f, 0.360000014f);
        c.targetRotation = new Quaternion(-0.7f, 0.5f, 0.8f, 1f);

    }

    public override void MakeCurve(Vector3 direction)
    {
        transform.tag = "ThrownWeapon";
        rb.useGravity = false;
        thrown = true;
        //transform.rotation = Quaternion.identity;
        axisToRotate = transform.position + direction * 4;
        //rb.centerOfMass = axisToRotate;
        onAirCollider.enabled = true;
        
        rotacionEx = Mathf.Asin((transform.position - axisToRotate).normalized.x);
        if (axisToRotate.z >= transform.position.z) 
        {
            if(rotacionEx >= 0)
            {
                rotacionEx = Mathf.PI - rotacionEx;
            }
            else
            {
                rotacionEx = Mathf.PI - rotacionEx;
            }
        }
        rotacionEx += Mathf.PI;
        anglesRotated = 0;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (thrown) {
            anglesRotated += Time.fixedDeltaTime * rotationSpeed;
            rotacionEx -= Time.fixedDeltaTime * rotationSpeed;
            float x = axisToRotate.x - Mathf.Sin(rotacionEx) * 4;
            float y = axisToRotate.y;
            float z = axisToRotate.z - Mathf.Cos(rotacionEx) * 4;

            //print(axisToRotate);
            Vector3 auxPosition = transform.position;

            //transform.RotateAround(axisToRotate, -Vector3.up, 300 * Time.fixedDeltaTime);
            //rb.MoveRotation(transform.rotation * Quaternion.Euler(0, 180 * Time.fixedDeltaTime, 0));
            transform.position += new Vector3(x,y,z) - transform.position;

            rotated = true;

            //print(anglesRotated);
            if(anglesRotated >= Mathf.PI*1.65f || stop)
            {
                rotated = false;
                stop = false;
                onAirCollider.enabled = false;
                tag = "Weapon";
                owner = Characters.None;
                thrown = false;
                rb.useGravity = true;
                //SetOnFloorColliders();
                //rb.velocity = (transform.position - auxPosition).normalized * 500;
                GetComponent<Rigidbody>().AddForce((transform.position - auxPosition).normalized * 40, ForceMode.Impulse);
                DestroyAfterSpawning();
            }
        }
        if(rotated)
        {
            rotacion += 360 * Time.fixedDeltaTime * 2;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotacion, 0), 360 * Time.fixedDeltaTime * 2);
        }
        //GetComponent<Rigidbody>().AddForce(Vector3.up * 50);
    }

    private void OnCollisionEnter(Collision other) {

        if(anglesRotated > .5f)
        {
            rotated = false;
        }
        PlayerController p = other.gameObject.GetComponentInParent<PlayerController>();

        if(other.gameObject.GetComponent<ShieldDetector>() != null && thrown)
        {
            stop = true;
        }

        if(p != null && this.gameObject.CompareTag("ThrownWeapon")) {
            if(p.waitForDisco) {
                //thrown = false;
                //p.detector.PickDisco(this.transform);
                //p.waitForDisco = false;
            }
        }
    }
}
