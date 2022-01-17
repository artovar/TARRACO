using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDetector : MonoBehaviour
{
    AudioSource source;
    [SerializeField]
    AudioClip arrowHit;
    [SerializeField]
    AudioClip defense;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision col)
    {
        bool playAudio = false;
        LayerMask layer = col.gameObject.layer;
        if (layer >= LayerMask.NameToLayer("Arrow_1") && layer <= LayerMask.NameToLayer("Arrow_E"))
        {
            playAudio = true;
            source.clip = arrowHit;
            col.collider.enabled = false;
            (col.gameObject.AddComponent<FixedJoint>()).connectedBody = GetComponent<Rigidbody>();
            col.rigidbody.velocity = Vector3.zero;
        }
        else if(col.gameObject.CompareTag("GrabbedWeapon") && layer != gameObject.layer)
        {
            source.clip = defense;
            playAudio = true;
        }
        if(playAudio && !source.isPlaying)
        {
            source.Play();
        }
    }
}
