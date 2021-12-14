using UnityEngine;

public class FeetContact : MonoBehaviour
{
    public PlayerController APR_Player;
    private ParticleSystem system;
    [SerializeField]
    private bool f;
    LayerMask g, e;
    private void Start()
    {
        system = GetComponent<ParticleSystem>();
        g = LayerMask.NameToLayer("Ground");
        e = LayerMask.NameToLayer("Enemies");
    }
    //Alert APR player when feet colliders enter ground object layer
    void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.layer == g || col.gameObject.layer == e))
        {
            system.Emit(1);
            if(f)
            {
                APR_Player.StepSource.clip = APR_Player.Steps[0];
                APR_Player.StepSource.Play();
            }
        }
        if (!APR_Player.isJumping && APR_Player.inAir)
        {
            if (col.gameObject.layer == g || col.gameObject.layer == e)
            {
                APR_Player.PlayerLanded();
            }
        }
    }
}