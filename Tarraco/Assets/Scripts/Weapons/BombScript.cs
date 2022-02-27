using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : WeaponScript
{
    public float delay = 3f;
    public float radius = 5f;

    public GameObject explosionParticles;

    private float conuntdown;
    private bool hasExploted = false;

    private void Start() {
        conuntdown = delay;
    }

    private void Update() {
        conuntdown -= Time.deltaTime;
        if(conuntdown <= 0 && !hasExploted && transform.tag.Equals("ThrownWeapon")) {
            Explode();
        }
    }

    public override void PrepareHit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c) {
        a.targetRotation = new Quaternion(-0.15f, -0.15f, 0, 1);
        b.targetRotation = new Quaternion(-0.360000014f, -0.939999998f, 0.560000002f, 1.38f);
        c.targetRotation = new Quaternion(0.709999979f, -0.610000014f, 0.839999974f, 1f);
    }
    public override void Hit(ConfigurableJoint a, ConfigurableJoint b, ConfigurableJoint c, float force) {}

    private void Explode() {
        Instantiate(explosionParticles, transform.position, transform.rotation);

        //Get nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 15488);

        foreach(Collider c in colliders) {
            //Damage the characters around
            CharacterClass character = c.GetComponentInParent<CharacterClass>();

            if(character != null) {
                character.Damage(1, owner, transform.position);
                print("boom: "+c.gameObject.name);
            }
        }
        //Add force
        //Damage

        Destroy(this.gameObject);
    }
}
