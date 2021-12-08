using UnityEngine;


public class EImpactContact : MonoBehaviour
{
    public BasicEnemyController enemyController;

    //Alert APR Player when collision enters with specified force amount
    void OnCollisionEnter(Collision col)
    {
        if (col.Equals(enemyController.weapon)) return;
        //Knockout by impact
        if (enemyController.canBeKnockoutByImpact && col.relativeVelocity.magnitude > enemyController.requiredForceToBeKO)
        {
            enemyController.ActivateRagdoll();
            Destroy(enemyController.gameObject, 2f);

            if (enemyController.SoundSource != null)
            {
                if (!enemyController.SoundSource.isPlaying && enemyController.Hits != null)
                {
                    int i = Random.Range(0, enemyController.Hits.Length);
                    enemyController.SoundSource.clip = enemyController.Hits[i];
                    enemyController.SoundSource.Play();
                }
            }
        }

        //Sound on impact
        if (col.relativeVelocity.magnitude > enemyController.ImpactForce)
        {

            if (enemyController.SoundSource != null)
            {
                if (!enemyController.SoundSource.isPlaying && enemyController.Impacts != null)
                {
                    int i = Random.Range(0, enemyController.Impacts.Length);
                    enemyController.SoundSource.clip = enemyController.Impacts[i];
                    enemyController.SoundSource.Play();
                }
            }
        }
    }
}