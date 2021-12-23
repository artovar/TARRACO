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
            Characters from = Characters.None;
            LayerMask layer = col.gameObject.layer;
            if (layer >= 16 && layer <= 19 && damageTaken != 0)
            {
                switch(layer)
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
            int damage = 1 * damageTaken;
            WeaponScript wp = col.gameObject.GetComponentInParent<WeaponScript>();
            if (wp != null)
            {
                from = wp.owner;
                switch (wp.kind)
                {
                    case Weapons.Spear:
                        damage = wp.damageDealed * (damageTaken + 1);
                        break;
                    default:
                        damage = wp.damageDealed * damageTaken;
                        break;
                }
            }
            else
            {
                switch(col.gameObject.layer)
                {
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                        from = col.gameObject.GetComponentInParent<CharacterClass>().character;
                        damage = damageTaken;
                        break;
                }
            }
            //Damage
            if(enemyController.Damage(damage, from)) enemyController.ActivateRagdoll();

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