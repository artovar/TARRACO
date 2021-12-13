using UnityEngine;


public class EImpactContact : MonoBehaviour
{
    public BasicEnemyController enemyController;
    [SerializeField]
    private int damageTaken;

    //Alert APR Player when collision enters with specified force amount
    void OnCollisionEnter(Collision col)
    {
        if (!Object.ReferenceEquals(enemyController.weapon, null) && (col.transform.IsChildOf(enemyController.transform) || col.transform.IsChildOf(enemyController.weapon.transform))) return;
        //Knockout by impact
        if (enemyController.canBeKnockoutByImpact && col.relativeVelocity.magnitude > enemyController.requiredForceToBeKO)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Arrow"))
            {
                (col.gameObject.AddComponent<FixedJoint>()).connectedBody = this.gameObject.GetComponent<Rigidbody>();
                col.rigidbody.velocity = Vector3.zero;
                col.collider.enabled = false;
            }
            enemyController.ActivateRagdoll();

            //SUSTITUIR ESTO POR MUERTE

            if (enemyController.SoundSource != null)
            {
                if (!enemyController.SoundSource.isPlaying && enemyController.Hits != null)
                {
                    int i = Random.Range(0, enemyController.Hits.Length);
                    enemyController.SoundSource.clip = enemyController.Hits[i];
                    enemyController.SoundSource.Play();
                }
            }
            Characters from = Characters.Player1;
            int damage = 1;
            WeaponScript wp = col.gameObject.GetComponentInParent<WeaponScript>();
            if (wp != null)
            {
                from = wp.owner;
                damage = wp.damageDealed * damageTaken;
            }
            //Damage
            enemyController.Damage(damage, from);
            //Debug.Log("AU!! ¡Qué daño! Me queda esta vida:" + enemyController.life);

            if (enemyController.IsDead())
            {
                //Debug.Log("Enemigo asesinado");
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