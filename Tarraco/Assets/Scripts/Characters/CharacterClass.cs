using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class CharacterClass : MonoBehaviour
{
    public Characters character;
    public HealthHUD hUD;
    public int life;
    public int maxLife;
    public float moveSpeed;
    [SerializeField]
    protected const float invTimeDef = .7f;
    protected float invTime = 0;
    public event EventHandler YoureDead;
    private bool alreadyDead;

    public bool Damage(int amount, Characters from) {
        if (invTime > 0)
        {
            return false;
        }
        if(amount != 0) invTime = invTimeDef;
        life -= amount;
        if (life < 0) life = 0;
        if (IsDead())
        {
            if(!alreadyDead) OvationSingleton.Instance.IncreaseMeter(10f, from);
            alreadyDead = true;
            YoureDead(this, EventArgs.Empty);
        }
        if(!character.Equals(Characters.Enemy)) hUD.GetComponent<HealthHUD>().HurtHUD(1);
        return true;
    }

    public void Heal(int amount) {
        life += amount;
        invTime = invTimeDef / 2;
        if (life > maxLife) life = maxLife;
    }

    public bool IsDead() {
        return life <= 0;
    }
}