using System.Collections;
using UnityEngine;


public class ImpactContact : MonoBehaviour
{
    public PlayerController APR_Player;
    private bool alreadyDead;

    //Alert APR Player when collision enters with specified force amount
    void OnCollisionEnter(Collision col)
    {
        LayerMask layer = col.gameObject.layer;
        if (col.gameObject.CompareTag("Weapon") || layer == APR_Player.gameObject.layer || APR_Player.detector.IsOneOfMine(col.transform)) return;
        //Knockout by impact
        bool also = col.gameObject.CompareTag("ThrownWeapon");
        if ((APR_Player.canBeKnockoutByImpact && col.relativeVelocity.magnitude > APR_Player.requiredForceToBeKO) || (also && col.gameObject.GetComponent<WeaponScript>().owner != APR_Player.character))
        {
            Characters from = Characters.Enemy;
            if(layer >= 10 && layer <= 13)
            {
                switch (layer)
                {
                    case 10:
                        from = Characters.Player1;
                        break;
                    case 11:
                        from = Characters.Player2;
                        break;
                    case 12:
                        from = Characters.Player3;
                        break;
                    case 13:
                        from = Characters.Player4;
                        break;
                }
            }
            if (layer >= 16 && layer <= 20 && col.collider.enabled)
            {
                switch (layer)
                {
                    case 16:
                        from = Characters.Player1;
                        break;
                    case 17:
                        from = Characters.Player2;
                        break;
                    case 18:
                        from = Characters.Player3;
                        break;
                    case 19:
                        from = Characters.Player4;
                        break;
                }
                (col.gameObject.AddComponent<FixedJoint>()).connectedBody = this.gameObject.GetComponent<Rigidbody>();
                col.rigidbody.velocity = Vector3.zero;
                col.collider.enabled = false;
            }

            //Damage
            bool dead = false;
            if (GameController.Instance.inGame)
            {
                WeaponScript wp = col.gameObject.GetComponentInParent<WeaponScript>();
                if (from.Equals(Characters.Enemy) && wp != null)
                {
                    from = wp.owner;
                }

                APR_Player.ActivateRagdoll();
                dead = APR_Player.Damage(1, from, col.contacts[0].point);
            }
            //Debug.Log("AU!! ¡Qué daño! Me queda esta vida:" + APR_Player.life);

            if (APR_Player.SoundSource != null)
            {
                if (!APR_Player.SoundSource.isPlaying && APR_Player.Hits != null)
                {
                    int i = Random.Range(0, APR_Player.Hits.Length);
                    APR_Player.SoundSource.clip = APR_Player.Hits[i];
                    APR_Player.SoundSource.Play();
                }
            }
            if (APR_Player.IsDead())
            {
                if(!alreadyDead)
                {
                    alreadyDead = true;
                    StartCoroutine(PlayEngineSound());
                }
            }
        }
        //Sound on impact & normal impact
        if ((col.relativeVelocity.magnitude > APR_Player.ImpactForce) || (also && col.gameObject.GetComponent<WeaponScript>().owner != APR_Player.character))
        {
            //Sound
            if (APR_Player.SoundSource != null)
            {
                if (!APR_Player.SoundSource.isPlaying && APR_Player.Impacts != null)
                {
                    int i = Random.Range(0, APR_Player.Impacts.Length);
                    APR_Player.SoundSource.clip = APR_Player.Impacts[i];
                    APR_Player.SoundSource.Play();
                }
            }

            if (APR_Player.IsDead())
            {
            }
        }
    }
    IEnumerator PlayEngineSound()
    {
        yield return new WaitForSeconds(APR_Player.SoundSource.clip.length);
        APR_Player.SoundSource.clip = APR_Player.DeathSound;
        APR_Player.SoundSource.Play();
    }
}