using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetection : MonoBehaviour
{
    public HealthHUD healthUI;
    [SerializeField]
    private PlayerController controller;
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

    private string interact = "Interact"; 
    private string drop = "Drop"; 
    private string change = "Change"; 

    public bool IsOneOfMine(Transform t)
    {
        return ((weaponsStored > 0 && t.IsChildOf(mainWeapon)) || (weaponsStored > 1 && t.IsChildOf(backWeapon)));
    }

    public void SetUp()
    {
        if (controller.id != 1)
        {
            interact = "Interact" + controller.id;
            drop = "Drop" + controller.id;
            change = "Change" + controller.id;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(interact))
        {
            picking = true;
        }
        if (picking)
        {
            pickingCoyoteTime += Time.deltaTime;
            if (pickingCoyoteTime > .1f)
            {
                picking = false;
                pickingCoyoteTime = 0;
            }
        }
        if (Input.GetButtonDown(drop))
        {
            Drop(mainWeapon);
        }
        else if (Input.GetButtonDown(change)) //Switch between Weapons
        {
            if (weaponsStored < 2) return;
            SendToBack(mainWeapon);
            BringFromBack(backWeapon);
            Transform aux = mainWeapon;
            mainWeapon = backWeapon;
            backWeapon = aux;
            controller.weapon = mainWeapon.GetComponent<WeaponScript>();
            if (controller.attacking)
            {
                controller.PrepareHit();
                if (backWeapon.GetComponent<WeaponScript>().kind == Weapons.Bow)
                {
                    controller.ResetLeftArm();
                    backWeapon.GetComponent<BowScript>().StopShooting();
                }
            }
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
            if (controller.attacking)
            {
                controller.PrepareHit();
                /*if (backWeapon.GetComponent<WeaponScript>().kind == Weapons.Bow)
                {
                    controller.ResetLeftArm();
                    backWeapon.GetComponent<BowScript>().StopShooting();
                }*/
            }
        }
        else if (col.CompareTag("Heal") && controller.life < controller.maxLife && !controller.IsDead())
        {
            col.enabled = false;
            Destroy(col.gameObject);
            controller.Heal(1);
            healthUI.HealHUD(1);
        }
    }

    void Pick(Transform weapon)
    {
        if (weapon == null) return;
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
        if (weapon == null) return;
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
        if (mainWeapon == null)
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
        weapon.GetComponent<WeaponScript>().GetWeapon(rHandTransform, lHandTransform, controller.character);
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