using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public BasicEnemyController enemyScript;
    public GameObject player;
    Transform p;
    private bool started;
    private float cd;
    private int estado; //de momento no se usa, pero usa 1 si está atacando, 2 si acaba de atacar y 3 si está herido. 0 de lo contrario (buscando)
    // Start is called before the first frame update
    void Start()
    {
        started = false;
        cd = 5;
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

    // Update is called once per frame
    void Update()
    {
        if(!started)
        {
            return;
        }
        enemyScript.jump = 0;
        if (cd < 0)
        {
            enemyScript.jump = 1;
        }
        if (Vector3.Distance(player.transform.position, enemyScript.Root.transform.position) < 4)
        {
            //Atacar
            estado = 1; //"Atacando"
        }
        else
        {
            Vector3 direction = ((p.position + (enemyScript.Root.transform.position - p.position).normalized * 3) - enemyScript.Root.transform.position);
            Vector2 dir = new Vector2(direction.x, direction.z).normalized;
            enemyScript.forwardBackward = dir.y;
            enemyScript.leftRight = dir.x;

        }

        Debug.Log(Vector3.Distance(player.transform.position, enemyScript.Root.transform.position));
        
    }
}
