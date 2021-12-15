using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BowScript : WeaponScript
{
    [SerializeField]
    float arrowForce;
    [SerializeField]
    GameObject[] arrows;

    public override void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c)
    {
        /*a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(-0.360000014f, -0.939999998f, 0.560000002f, 1.38f);
        c.targetRotation = new Quaternion(0.709999979f, -0.610000014f, 0.839999974f, 1f);*/

        a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(.7f, -0.05f, 0.05f, 0.7f);
        c.targetRotation = new Quaternion(.34f, -0.3f, .06f, .89f);
        GetComponentInChildren<LineRenderer>().SetPosition(1, new Vector3(.2f,0,1));
    }
    public override void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, float force)
    {
        a.targetRotation = new Quaternion(-0.15f, 0.15f, 0, 1);
        /*b.targetRotation = new Quaternion(0.150000006f, -0.439999998f, 0.649999976f, 0.360000014f);
        c.targetRotation = new Quaternion(-0.439999998f, 0.5f, 0.439999998f, 1f);*/
        b.targetRotation = new Quaternion(.7f, .4f, 0.05f, 0.7f);
        c.targetRotation = new Quaternion(.34f, -0.3f, .06f, .89f);
        GetComponentInChildren<LineRenderer>().SetPosition(1, new Vector3(0, 0, 1));

        GetComponent<Rigidbody>().AddForce(transform.up * force * 1.25f, ForceMode.Impulse);
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

    public override void Shoot(Vector3 direction, Characters cType)
    {
        GameObject arrowClone;
        switch (cType)
        {
            case Characters.Player1:
                arrowClone = Instantiate(arrows[0], transform.position, Quaternion.LookRotation(direction, Vector3.up));
                break;
            case Characters.Player2:
                arrowClone = Instantiate(arrows[1], transform.position, Quaternion.LookRotation(direction, Vector3.up));
                break;
            case Characters.Player3:
                arrowClone = Instantiate(arrows[2], transform.position, Quaternion.LookRotation(direction, Vector3.up));
                break;
            case Characters.Player4:
                arrowClone = Instantiate(arrows[3], transform.position, Quaternion.LookRotation(direction, Vector3.up));
                break;
            default:
                float desviation = Random.Range(-0.05f, 0.05f);
                if (desviation > 0)
                {
                    arrowClone = Instantiate(arrows[4], transform.position, Quaternion.LookRotation(direction + Vector3.forward * desviation, Vector3.up));
                }
                else
                {
                    arrowClone = Instantiate(arrows[4], transform.position, Quaternion.LookRotation(direction + Vector3.right * desviation, Vector3.up));
                }
                break;
        };
        arrowClone.GetComponent<Rigidbody>().velocity = direction * arrowForce;
        arrowClone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Destroy(arrowClone, 3);
    }
    public override void SendToBack(Transform back)
    {
        transform.position = back.position + back.forward/2;
        transform.rotation = back.rotation * Quaternion.Euler(0,0,180);
        GetComponent<FixedJoint>().connectedBody = back.GetComponent<Rigidbody>();
    }
    public void PrepareLeftHand(ConfigurableJoint l5, ConfigurableJoint l6)
    {
        l5.targetRotation = new Quaternion(-.64f, -.21f, -.28f, 1f);
        l6.targetRotation = new Quaternion(-0.73f, -0.37f, -0.43f, 1f);
    }
    public void StopShooting()
    {
        GetComponentInChildren<LineRenderer>().SetPosition(1, new Vector3(0, 0, 1));
    }
}
