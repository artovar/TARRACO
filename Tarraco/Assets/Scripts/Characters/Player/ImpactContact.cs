using UnityEngine;


public class ImpactContact : MonoBehaviour
{
    public PlayerController APR_Player;
    public GameObject HealthHUD;

    //Alert APR Player when collision enters with specified force amount
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.IsChildOf(APR_Player.transform) || APR_Player.detector.IsOneOfMine(col.transform)) return;
        //Knockout by impact
        if (APR_Player.canBeKnockoutByImpact && col.relativeVelocity.magnitude > APR_Player.requiredForceToBeKO)
        {
            LayerMask layer = col.gameObject.layer;
            if (layer > LayerMask.NameToLayer("Arrow_1") && layer <= LayerMask.NameToLayer("Arrow_E") && col.collider.enabled)
            {
                (col.gameObject.AddComponent<FixedJoint>()).connectedBody = this.gameObject.GetComponent<Rigidbody>();
                col.rigidbody.velocity = Vector3.zero;
                col.collider.enabled = false;
            }

            APR_Player.ActivateRagdoll();

            if (APR_Player.SoundSource != null)
            {
                if (!APR_Player.SoundSource.isPlaying && APR_Player.Hits != null)
                {
                    int i = Random.Range(0, APR_Player.Hits.Length);
                    APR_Player.SoundSource.clip = APR_Player.Hits[i];
                    APR_Player.SoundSource.Play();
                }
            }

            //Damage
            if(APR_Player.Damage(1, Characters.Enemy)) HealthHUD.GetComponent<HealthHUD>().HurtHUD(1);
            //Debug.Log("AU!! ¡Qué daño! Me queda esta vida:" + APR_Player.life);
            if (APR_Player.IsDead())
            {
                //Debug.Log("Estas muerto");
            }
        }

        //Sound on impact & normal impact
        if (col.relativeVelocity.magnitude > APR_Player.ImpactForce)
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
}