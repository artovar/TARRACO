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

    private bool overSomething;
    [SerializeField]
    private ButtonHelp help;
    private float stillHelp = .1f;

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
        stillHelp -= Time.deltaTime;
        if (!GameController.Instance.inGame && overSomething)
        {
            help.Show(controller.usingController);
            overSomething = false;
        }
        else if(stillHelp <= 0)
        {
            help.Hide();
        }

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
        if(col.CompareTag("Interact"))
        {
            overSomething = true;
            stillHelp = .1f;
            if (!picking)
            {
                return;
            }
            picking = false;
            if(col.gameObject.GetComponent<Switcher>() != null)
            {
                Switcher switcher = col.gameObject.GetComponent<Switcher>();
                switcher.Interact();
                return;
            }
            GameController.Instance.ChangeSkin(controller.character);
        }
        else if (col.CompareTag("Weapon") || col.CompareTag("ThrownWeapon"))
        {
            overSomething = true;
            stillHelp = .1f;
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

            //Sound
            if (controller.SoundSource != null) {
                if (!controller.SoundSource.isPlaying && controller.Jummy != null)
                {
                    int i = Random.Range(0, controller.Jummy.Length);
                    controller.SoundSource.clip = controller.Jummy[i];
                    controller.SoundSource.Play();
                }
            }
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

    void Throw(Transform weapon)
    {
        if (weapon == null) return;
        switch (weaponsStored)
        {
            case 0:
                if (mainWeapon != null)
                {
                    ThrowWeapon(mainWeapon);
                    print("Error with mainWeapon");
                }
                if (backWeapon != null)
                {
                    ThrowWeapon(backWeapon);
                    print("Error with backWeapon");
                }
                mainWeapon = null;
                backWeapon = null;
                break;
            case 1:
                ThrowWeapon(mainWeapon);
                mainWeapon = null;
                weaponsStored--;
                break;
            case 2:
                ThrowWeapon(mainWeapon);
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

    void ThrowWeapon(Transform weapon)
    {
        weapon.GetComponent<WeaponScript>().ThrowWeapon(rHandTransform);
    }

    void SendToBack(Transform weapon)
    {
        weapon.GetComponent<WeaponScript>().SendToBack(backTransform);
    }

    void BringFromBack(Transform weapon)
    {
        weapon.GetComponent<WeaponScript>().BringFromBack(rHandTransform, lHandTransform);
    }

    public void DropAllWeapons()
    {
        //Esto no es duplicidad, es para soltar si tiene dos
        if(weaponsStored > 0)
        {
            Drop(mainWeapon);
        }
        if(weaponsStored > 0) 
        {
            Drop(mainWeapon);
        }
    }
    public void PickFromBegining(Transform newWeapon)
    {
        Pick(newWeapon);
    }

    public void GetWeapons(out Weapons w1, out Weapons w2)
    {
        //Esto no es duplicidad, es para soltar si tiene dos
        if (weaponsStored > 0)
        {
            w1 = mainWeapon.GetComponent<WeaponScript>().kind;
        }
        else
        {
            w1 = Weapons.None;
            w2 = Weapons.None;
        }
        if (weaponsStored > 1)
        {
            w2 = backWeapon.GetComponent<WeaponScript>().kind;
        }
        else
        {
            w2 = Weapons.None;
        }
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
    public void PickDisco(Transform weapon) {
        Pick(weapon);
    }
}