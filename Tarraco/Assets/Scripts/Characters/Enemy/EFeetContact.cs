using UnityEngine;

public class EFeetContact : MonoBehaviour
{
    public BasicEnemyController APR_Player;
    [SerializeField]
    private bool f;
    LayerMask g, e;
    private void Start()
    {
        g = LayerMask.NameToLayer("Ground");
        e = LayerMask.NameToLayer("Enemies");
    }

    //Alert APR player when feet colliders enter ground object layer
    void OnCollisionEnter(Collision col)
    {
        if (f && (col.gameObject.layer == g || col.gameObject.layer == e))
        {
            APR_Player.StepSource.clip = APR_Player.Steps[0];
            APR_Player.StepSource.Play();
        }
        if (!APR_Player.isJumping && APR_Player.inAir)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                APR_Player.PlayerLanded();
            }
        }
    }
}