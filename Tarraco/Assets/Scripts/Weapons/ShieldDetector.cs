using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        LayerMask layer = col.gameObject.layer;
        if (layer >= LayerMask.NameToLayer("Arrow_1") && layer <= LayerMask.NameToLayer("Arrow_E"))
        {
            col.collider.enabled = false;
            (col.gameObject.AddComponent<FixedJoint>()).connectedBody = GetComponent<Rigidbody>();
            col.rigidbody.velocity = Vector3.zero;
        }
    }
}
