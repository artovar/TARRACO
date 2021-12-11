using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class CharacterClass : MonoBehaviour
{
    [HideInInspector]
    public int life;
    public int maxLife;
    public float moveSpeed;
    protected float invTimeDef = .7f;
    protected float invTime = 0;

    public event EventHandler YoureDead;

    public bool Damage(int amount) {
        if (invTime > 0)
        {
            print("They tried");
            return false;
        }
        else
        {
            print("They did");
        }
        invTime = invTimeDef;
        life -= amount;
        if (life < 0) life = 0;
        if (IsDead()) YoureDead(this, EventArgs.Empty);
        print("Just damaged");
        return true;
    }

    public void Heal(int amount) {
        life += amount;
        if (life > maxLife) life = maxLife;
    }

    public bool IsDead() {
        return life <= 0;
    }
}