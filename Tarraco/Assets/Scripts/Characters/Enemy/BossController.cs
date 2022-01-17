using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public bool isBoss;
    public BasicEnemyController enemyScript;
    public GameObject player;
    Transform playerTransform;

    private bool foundSomeone;

    private float attackCD;
    private float originalSpeed;

    private bool slowed;
    private bool alreadyDead;

    [HideInInspector]
    public int estado; //de momento no se usa, pero usa 1 si está atacando, 2 si acaba de atacar y 3 si está herido. 0 de lo contrario (buscando)

    // Start is called before the first frame update
    void Start()
    {
        foundSomeone = false;
        attackCD = 0;
        originalSpeed = enemyScript.moveSpeed;

        estado = -1; //"Haciendo el tonto"
    }

    public void Detect(GameObject jugador)
    {
        foundSomeone = true;
        player = jugador;
        playerTransform = player.GetComponentInParent<PlayerController>().Root.transform;

        estado = 0; //"Buscando"
    }

    public void MoveTowardsInSpawn(Vector3 dir)
    {
        enemyScript.forwardBackward = dir.x;
        enemyScript.leftRight = dir.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyScript.IsDead())
        {
            if (!alreadyDead && isBoss && enemyScript.isDead)
            {
                alreadyDead = true;
                GameController.Instance.Win();
            }
            return;
        }
        if (IsRagdoll())
        {
            attackCD -= Time.deltaTime;
            if (attackCD <= 0)
            {
                attackCD = 1;
                enemyScript.Jump();
            }
            return;
        }
        if (!foundSomeone)
        {
            return;
        }

        attackCD -= Time.deltaTime;

        if (player == null) return;

        if (enemyScript.attack && !slowed)
        {
            enemyScript.Slow(.1f);
            slowed = true;
        }
        else if (!enemyScript.attack && slowed)
        {
            enemyScript.Speed(.1f);
            slowed = false;
        }

        if (!Object.ReferenceEquals(enemyScript.weapon, null)
            && enemyScript.weapon.kind.Equals(Weapons.Bow)
            && Vector3.Distance(player.transform.position, enemyScript.Root.transform.position) < 12f)
        {
            Vector3 direction;
            if (enemyScript.attack)
            {
                direction = (playerTransform.position - enemyScript.Root.transform.position);
            }
            else
            {
                direction = ((playerTransform.position + (enemyScript.Root.transform.position - playerTransform.position).normalized * 7) - enemyScript.Root.transform.position);
            }
            Vector2 dir = new Vector2(direction.x, direction.z).normalized;
            enemyScript.forwardBackward = dir.y;
            enemyScript.leftRight = dir.x;
            //Atacar
            if (attackCD <= 0 && !enemyScript.IsRagdoll())
            {
                enemyScript.Attack();
                attackCD = 3;
            }

            estado = 1; //"Atacando"
        }
        else if (!enemyScript.attack && Vector3.Distance(player.transform.position, enemyScript.Root.transform.position) < 4f)
        {
            //Atacar
            if (attackCD <= 0 && !enemyScript.IsRagdoll())
            {
                enemyScript.Attack();
                attackCD = 3;
            }
            estado = 1; //"Atacando"
        }
        else if (attackCD > 2.5f || attackCD < 1f)
        {
            Vector3 direction = (playerTransform.position - enemyScript.Root.transform.position) - enemyScript.Root.transform.right;
            Vector2 dir = new Vector2(direction.x, direction.z).normalized;
            enemyScript.forwardBackward = dir.y;
            enemyScript.leftRight = dir.x;
        }
        else
        {
            enemyScript.forwardBackward = 0;
            enemyScript.leftRight = 0;
        }
    }
    public bool IsRagdoll()
    {
        return enemyScript.IsRagdoll();
    }
    public bool IsDead()
    {
        return enemyScript.IsDead();
    }
}
