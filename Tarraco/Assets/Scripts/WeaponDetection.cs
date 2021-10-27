using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARP.APR.Scripts;

public class WeaponDetection : MonoBehaviour
{
    [SerializeField]
    private APRController controller;
    [SerializeField]
    private Transform handTransform;
    [SerializeField]
    private Transform backTransform;
    private bool picking;
    private float pickingCoyoteTime;
    private int weaponsStored;
    private Transform mainWeapon;
    private Transform backWeapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            picking = true;
            print("I'm trying");
        }
        if (picking)
        {
            pickingCoyoteTime += Time.deltaTime;
            if(pickingCoyoteTime > .1f)
            {
                picking = false;
                pickingCoyoteTime = 0;
            }
        }
        if (Input.GetButtonDown("Drop")) 
        {
            print(Input.GetAxisRaw("Drop") + " Oh no");
            Drop(mainWeapon);
            print(weaponsStored);
        }
        else if(Input.GetButtonDown("Change")) //Switch between Weapons
        {
            SendToBack(mainWeapon);
            BringFromBack(backWeapon);
            Transform aux = mainWeapon;
            mainWeapon = backWeapon;
            backWeapon = aux;
            controller.weapon = mainWeapon.GetComponent<WeaponScript>();
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Weapon"))
        {
            if (!picking)
            {
                return;
            }
            picking = false;
            pickingCoyoteTime = 0;
            Pick(col.transform);
            print(weaponsStored);
        }
    }

    void Pick(Transform weapon)
    {
        switch (weaponsStored)
        {
            case 0:
                mainWeapon = weapon.transform;
                GetWeapon(weapon);
                weaponsStored++;
                break;
            case 1:
                backWeapon = mainWeapon;
                SendToBack(backWeapon);
                mainWeapon = weapon.transform;
                GetWeapon(weapon);
                weaponsStored++;
                break;
            case 2:
                DropWeapon(mainWeapon);
                mainWeapon = weapon.transform;
                GetWeapon(weapon);
                break;
        }
        controller.weapon = mainWeapon.GetComponent<WeaponScript>();
    }

    void Drop(Transform weapon)
    {
        switch (weaponsStored)
        {
            case 0:
                if (mainWeapon != null)
                {
                    DropWeapon(mainWeapon);
                    print("Error with mainWeapon");
                }
                if (backWeapon != null)
                {
                    DropWeapon(backWeapon);
                    print("Error with backWeapon");
                }
                mainWeapon = null;
                backWeapon = null;
                break;
            case 1:
                DropWeapon(mainWeapon);
                mainWeapon = null;
                weaponsStored--;
                break;
            case 2:
                DropWeapon(mainWeapon);
                mainWeapon = backWeapon;
                BringFromBack(backWeapon);
                backWeapon = null;
                weaponsStored--;
                break;
        }
        if(mainWeapon == null)
        {
            controller.weapon = null;
        }
        else
        {
            controller.weapon = mainWeapon.GetComponent<WeaponScript>();
        }
    }


    void GetWeapon(Transform weapon)
    {
        weapon.tag = "GrabbedWeapon";
        weapon.position = handTransform.position;
        weapon.rotation = handTransform.rotation;
        weapon.gameObject.AddComponent<FixedJoint>();
        weapon.GetComponent<FixedJoint>().connectedBody = handTransform.GetComponent<Rigidbody>();
        weapon.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
        weapon.GetComponent<Rigidbody>().useGravity = false;
        weapon.GetComponent<WeaponScript>().SetOnHandColliders();
    }

    void DropWeapon(Transform weapon)
    {
        weapon.tag = "Weapon";
        weapon.position = handTransform.position;
        RaycastHit hitInfo;
        Ray ray = new Ray(weapon.position, Vector3.down);
        if (Physics.Raycast(ray, out hitInfo, 1, 1 << LayerMask.NameToLayer("Ground")))
        {
            weapon.position = hitInfo.point + Vector3.up * .2f;
            weapon.rotation = new Quaternion(0f, weapon.rotation.y, 0f, weapon.rotation.w);
        }
        else
        {
            weapon.position -= Vector3.up * .1f;
            weapon.rotation = new Quaternion(0f, weapon.rotation.y, 0f, weapon.rotation.w);
        }
        weapon.GetComponent<FixedJoint>().connectedBody = null;
        weapon.GetComponent<FixedJoint>().breakForce = 0f;
        weapon.GetComponent<Rigidbody>().useGravity = true;
        weapon.GetComponent<WeaponScript>().SetOnFloorColliders();
    }

    void SendToBack(Transform weapon)
    {
        weapon.position = backTransform.position;
        weapon.rotation = backTransform.rotation;
        weapon.GetComponent<FixedJoint>().connectedBody = backTransform.GetComponent<Rigidbody>();
    }

    void BringFromBack(Transform weapon)
    {
        weapon.position = handTransform.position;
        weapon.rotation = handTransform.rotation;
        weapon.GetComponent<FixedJoint>().connectedBody = handTransform.GetComponent<Rigidbody>();
    }
}
