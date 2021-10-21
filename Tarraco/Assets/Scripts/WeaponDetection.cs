using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetection : MonoBehaviour
{
    [SerializeField]
    private Transform handTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {    
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Weapon") && (Input.GetKey("e")))
        {
            other.tag = "GrabbedWeapon";
            other.transform.parent = handTransform;
            other.transform.position = handTransform.position;
            other.transform.rotation = handTransform.rotation;
            other.GetComponent<Rigidbody>().isKinematic = true;
            //other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<WeaponScript>().weaponCollider.enabled = false;
            other.GetComponent<WeaponScript>().bladeCollider.enabled = true;
        }
    }
}
