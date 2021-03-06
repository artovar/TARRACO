using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWeaponDetection : MonoBehaviour
{
    [SerializeField]
    private BasicEnemyController controller;
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
        if (!controller.IsDead() && weaponsStored < 1)
        {
            picking = true;
        }
        else if (controller.IsDead() && weaponsStored > 0)
        {
            Drop(mainWeapon);
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
        if (Input.GetButtonDown("Change")) //Switch between Weapons
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
        }
    }

    public void SwitchWeapon()
    {
        if (weaponsStored < 2) return;
        SendToBack(mainWeapon);
        BringFromBack(backWeapon);
        Transform aux = mainWeapon;
        mainWeapon = backWeapon;
        backWeapon = aux;
        controller.weapon = mainWeapon.GetComponent<WeaponScript>();
    }
    public Weapons GetBackWeaponType()
    {
        if (weaponsStored < 2) return Weapons.None;
        else return backWeapon.GetComponent<WeaponScript>().kind;
    }
    public Weapons GetMainWeaponType()
    {
        if (weaponsStored < 1) return Weapons.None;
        else return mainWeapon.GetComponent<WeaponScript>().kind;
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
        weapon.localScale = controller.transform.localScale;
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
        weapon.localScale = Vector3.one;
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
    public void ThrowDisco(Vector3 direction)
    {
        if (weaponsStored > 0 && mainWeapon.GetComponent<WeaponScript>().kind.Equals(Weapons.Discobolus))
        {
            //Throw(mainWeapon);
            DiscobolusScript disco = mainWeapon.gameObject.GetComponent<DiscobolusScript>();
            disco.PrepareThrowing();
            Drop(mainWeapon);
            //disco.transform.rotation = Quaternion.identity;
            disco.MakeCurve(direction);
        }
    }
    public void PickBow(Transform weapon)
    {
        if (!weapon.GetComponent<WeaponScript>().kind.Equals(Weapons.Bow)) return;
        Pick(weapon);
    }
    public void PickGarrote(Transform weapon)
    {
        if (!weapon.GetComponent<WeaponScript>().kind.Equals(Weapons.Garrote)) return;
        Pick(weapon);
    }
}