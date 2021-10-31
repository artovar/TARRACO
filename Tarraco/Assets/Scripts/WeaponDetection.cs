using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARP.APR.Scripts;

public class WeaponDetection : MonoBehaviour
{
    [SerializeField]
    private APRController controller;
    [SerializeField]
    private Transform lHandTransform;
    [SerializeField]
    private Transform rHandTransform;
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
            if (weaponsStored < 2) return;
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
        weapon.GetComponent<WeaponScript>().GetWeapon(rHandTransform, lHandTransform);
    }

    void DropWeapon(Transform weapon)
    {
        weapon.GetComponent<WeaponScript>().DropWeapon(rHandTransform);
    }

    void SendToBack(Transform weapon)
    {
        weapon.GetComponent<WeaponScript>().SendToBack(backTransform);
    }

    void BringFromBack(Transform weapon)
    {
        weapon.GetComponent<WeaponScript>().BringFromBack(rHandTransform, lHandTransform);
    }
}
