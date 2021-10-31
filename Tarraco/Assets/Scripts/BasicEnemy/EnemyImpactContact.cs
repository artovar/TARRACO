using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactContact : MonoBehaviour
{
    public BasicEnemyScript enemyScript;

    //Alert APR Player when collision enters with specified force amount
    void OnCollisionEnter(Collision col)
    {

        //Knockout by impact
        if (enemyScript.canBeKnockoutByImpact && col.relativeVelocity.magnitude > enemyScript.requiredForceToBeKO)
        {
            enemyScript.ActivateRagdoll();

            if (enemyScript.SoundSource != null)
            {
                if (!enemyScript.SoundSource.isPlaying && enemyScript.Hits != null)
                {
                    int i = Random.Range(0, enemyScript.Hits.Length);
                    enemyScript.SoundSource.clip = enemyScript.Hits[i];
                    enemyScript.SoundSource.Play();
                }
            }
        }

        //Sound on impact
        if (col.relativeVelocity.magnitude > enemyScript.ImpactForce)
        {

            if (enemyScript.SoundSource != null)
            {
                if (!enemyScript.SoundSource.isPlaying && enemyScript.Impacts != null)
                {
                    int i = Random.Range(0, enemyScript.Impacts.Length);
                    enemyScript.SoundSource.clip = enemyScript.Impacts[i];
                    enemyScript.SoundSource.Play();
                }
            }
        }
    }
}
