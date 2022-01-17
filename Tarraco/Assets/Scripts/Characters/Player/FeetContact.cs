using UnityEngine;

public class FeetContact : MonoBehaviour
{
    public GameObject footParticle;
    private Transform theFoot;
    public PlayerController APR_Player;
    private ParticleSystem system;
    [SerializeField]
    private bool f;
    LayerMask g, e;

    ParticleSystemRenderer spritePrint;
    private void Start()
    {
        g = LayerMask.NameToLayer("Ground");
        e = LayerMask.NameToLayer("Enemies");

        theFoot = Instantiate(footParticle).transform;
        
        system = theFoot.GetComponent<ParticleSystem>();
        spritePrint = theFoot.GetComponent<ParticleSystemRenderer>();
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
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, -Vector3.up, out hit)) {
            //print("No hay hit");
            return;
        }
        Renderer ren = hit.transform.GetComponentInParent<MeshRenderer>();
        if(ren == null) {
            //print("no hay renderer");
            return;
        }
        
        Texture2D tex = ren.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord2;

        Color footColor;
        if (tex != null) {
            footColor = tex.GetPixel((int)(pixelUV.x * tex.width), (int)(pixelUV.y * tex.height));
        } else {
            //print("sin tex");
            footColor = ren.material.color;
        }
        Color printColor = (footColor + new Color(0.3f, 0.3f, 0.3f, 0.3f))/2;

        var main = system.main;
        main.startColor = printColor;

        //Direccion de la huella
        //spritePrint.normalDirection = ;
        theFoot.position = transform.position; //- Vector3.up * 0.6f;
        //theFoot.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        main.startRotationY = APR_Player.Root.transform.rotation.eulerAngles.y;

        system.Emit(1);
    }
}