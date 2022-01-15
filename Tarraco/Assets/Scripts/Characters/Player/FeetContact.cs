using UnityEngine;

public class FeetContact : MonoBehaviour
{
    public PlayerController APR_Player;
    private ParticleSystem system;
    [SerializeField]
    private bool f;
    LayerMask g, e;

    ParticleSystemRenderer spritePrint;
    private void Start()
    {
        system = GetComponentInChildren<ParticleSystem>();
        spritePrint = GetComponentInChildren<ParticleSystemRenderer>();
        g = LayerMask.NameToLayer("Ground");
        e = LayerMask.NameToLayer("Enemies");
    }
    //Alert APR player when feet colliders enter ground object layer
    void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.layer == g || col.gameObject.layer == e))
        {
            footprint(col.gameObject);
            //system.Emit(1);
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
    void footprint(GameObject floor) {
        //Cogemos el color
        Color footColor = floor.GetComponent<MeshRenderer>().material.color;
        Color printColor = (footColor + new Color(0.3f, 0.3f, 0.3f, 0.3f))/2;

        var main = system.main;        
        main.startColor = printColor;

        //Direccion de la huella
        //spritePrint.normalDirection = ;

        system.Emit(1);
    }
}