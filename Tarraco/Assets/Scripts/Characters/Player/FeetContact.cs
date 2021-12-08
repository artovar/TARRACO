using UnityEngine;

public class FeetContact : MonoBehaviour
{
    public PlayerController APR_Player;

    //Alert APR player when feet colliders enter ground object layer
    void OnCollisionEnter(Collision col)
    {
        if (!APR_Player.isJumping && APR_Player.inAir)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground") || col.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                APR_Player.PlayerLanded();
            }
        }
    }
}