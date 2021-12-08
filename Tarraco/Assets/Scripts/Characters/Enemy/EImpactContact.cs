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
            if (col.gameObject.layer == LayerMask.NameToLayer("Arrow"))
            {
                (col.gameObject.AddComponent<FixedJoint>()).connectedBody = this.gameObject.GetComponent<Rigidbody>();
                col.rigidbody.velocity = Vector3.zero;
            }
            enemyController.ActivateRagdoll();

            //SUSTITUIR ESTO POR MUERTE

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

            //Damage
            enemyController.damage(enemyController.life);
            Debug.Log("AU!! ¡Qué daño! Me queda esta vida:" + enemyController.life);
            if (enemyController.isDead())
            {
                Debug.Log("Estas muerto");
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

            //Damage
            enemyController.damage(1);
            Debug.Log("AU!! ¡Qué daño! Me queda esta vida:" + enemyController.life);
            if (enemyController.isDead())
            {
                Debug.Log("Estas muerto");
            }
        }
    }
}