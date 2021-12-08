using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public BasicEnemyController enemyScript;
    public GameObject player;
    Transform p;
    private bool started;
    private float attackCD;
    [HideInInspector]
    public int estado; //de momento no se usa, pero usa 1 si está atacando, 2 si acaba de atacar y 3 si está herido. 0 de lo contrario (buscando)
    // Start is called before the first frame update
    void Start()
    {
        started = false;
        attackCD = 1;
        estado = -1; //"Haciendo el tonto"
    }

    public void Detect(GameObject jugador)
    {
        started = true;
        player = jugador;
        p = player.GetComponentInParent<PlayerController>().Root.transform;
        enemyScript.jump = 1;
        estado = 0; //"Buscando"
    }
    public bool IsRagdoll()
    {
        return enemyScript.IsRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        if(!started)
        {
            return;
        }
        attackCD -= Time.deltaTime;
        if (!enemyScript.attack && Vector3.Distance(player.transform.position, enemyScript.Root.transform.position) < 3.4f)
        {
            //Atacar
            if (attackCD < 0 && !enemyScript.IsRagdoll())
            {
                enemyScript.Attack();
                attackCD = 3;
            }
            estado = 1; //"Atacando"
        }
        else if (attackCD > 2.5f || attackCD < 1.3f)
        {
            Vector3 direction = ((p.position + (enemyScript.Root.transform.position - p.position).normalized * 2) - enemyScript.Root.transform.position);
            Vector2 dir = new Vector2(direction.x, direction.z).normalized;
            enemyScript.forwardBackward = dir.y;
            enemyScript.leftRight = dir.x;
        }
        else
        {
            enemyScript.forwardBackward = 0;
            enemyScript.leftRight = 0;
        }

        //Debug.Log(Vector3.Distance(player.transform.position, enemyScript.Root.transform.position));
    }
}
