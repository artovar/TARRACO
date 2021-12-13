using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDetector : MonoBehaviour
{
    [SerializeField]
    private LayerMask mask;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("ArrowE"))
        {
            print("Trying");
            col.collider.enabled = false;
            (col.gameObject.AddComponent<FixedJoint>()).connectedBody = GetComponent<Rigidbody>();
            col.rigidbody.velocity = Vector3.zero;
        }
    }
}
