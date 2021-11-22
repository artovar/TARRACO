using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public BasicEnemyScript enemyScript;
    public GameObject player;
    Transform p;
    private bool started;
    private float cd;
    // Start is called before the first frame update
    void Start()
    {
        started = false;
        cd = 5;
    }

    public void Detect(GameObject jugador)
    {
        started = true;
        player = jugador;
        p = player.GetComponent<ARP.APR.Scripts.APRController>().Root.transform;
        enemyScript.jump = 1;
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
        Vector3 direction = ((p.position + (enemyScript.Root.transform.position - p.position).normalized*3) - enemyScript.Root.transform.position);
        Vector2 dir = new Vector2(direction.x, direction.z).normalized;
        enemyScript.forwardBackward = dir.y;
        enemyScript.leftRight = dir.x;
    }
}
