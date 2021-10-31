
using UnityEngine;

public class EnemyFeetContact : MonoBehaviour
{
    public BasicEnemyScript enemyController;

    //Alert APR player when feet colliders enter ground object layer
    void OnCollisionEnter(Collision col)
    {
        if (!enemyController.isJumping && enemyController.inAir)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                enemyController.PlayerLanded();
            }
        }
    }
}
