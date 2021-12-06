using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class CharacterClass : MonoBehaviour
{
    public int life;
    public int maxLife;
    public float moveSpeed;

    public event EventHandler youreDead;

    private void Start() {
        life = maxLife;
    }

    public void damage(int amount) {
        life -= amount;

        if(isDead()) youreDead(this, EventArgs.Empty);
    }

    public void heal(int amount) {
        if (amount+life >= maxLife) {
            life = maxLife;
        } else {
            life += amount;
        }
    }

    public bool isDead() {
        return life <= 0;
    }
}