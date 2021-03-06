using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class CharacterClass : MonoBehaviour
{
    public GameObject particleObj;
    [HideInInspector]
    public ParticleSystem system;
    public Characters character;
    public HealthHUD hUD;
    [HideInInspector]
    public int life;
    public int maxLife;
    public float moveSpeed;
    [SerializeField]
    const float invTimeDef = .7f;
    protected float invTime = 0;
    public event EventHandler YoureDead;
    private bool alreadyDead;

    private int feetInTrap;

    protected float chargingTime = .1f;

    public bool Damage(int amount, Characters from, Vector3 point, float invT = invTimeDef) {
        if (invTime > 0 || Time.timeScale != 1)
        {
            return false;
        }
        var sMain = system.main;
        if (amount < 1)
        {
            sMain.startColor = Color.grey + Color.white;
            system.transform.position = point;
            system.Emit(13);
        }
        else
        {
            sMain.startColor = new Color(.5f, 0f, 0f);
            system.transform.position = point;
            system.Emit(13);
        }
        switch(character)
        {
            case Characters.Enemy:
                SetInvencibleTime(.4f);
                break;
            default:
                SetInvencibleTime(.9f);
                break;
        }
        life -= amount;
        if (life < 0) life = 0;
        if (life == 0 && !alreadyDead)
        {
            OvationSingleton.Instance.IncreaseMeter(8f, from);
            alreadyDead = true;
        }
        if (IsDead())
        {
            YoureDead(this, EventArgs.Empty);
        }
        if (!character.Equals(Characters.Enemy))
        {
            hUD.GetComponent<HealthHUD>().HurtHUD(amount);
            if (life == 0)
            {
                GameController.Instance.AddDeath(character);
                foreach(MonoBehaviour m in GetComponentsInChildren<MonoBehaviour>())
                {
                    if (m != this) m.enabled = false;
                }
                return true;
            }
        }
        return false;
    }

    public void Heal(int amount) {
        life += amount;
        SetInvencibleTime(.3f);
        if (life > maxLife) life = maxLife;
    }

    public bool IsDead() {
        return life <= 0;
    }

    public void SetSpeed(float perc)
    {
        moveSpeed *= perc;
    }

    public void FootIn(float percPerFoot)
    {
        if (feetInTrap >= 2) return;
        feetInTrap++;
        moveSpeed *= percPerFoot;
    }    
    public void FootOut(float percPerFoot)
    {
        if (feetInTrap <= 0) return;
        feetInTrap--;
        moveSpeed /= percPerFoot;
    }

    public void Slow(float perc)
    {
        moveSpeed *= perc;
    }
    public void Speed(float perc)
    {
        moveSpeed /= perc;
    }
    public void SetInvencibleTime(float time = invTimeDef)
    {
        invTime = time;
    }
}