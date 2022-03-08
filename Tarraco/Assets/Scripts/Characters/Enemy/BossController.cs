using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    public EWeaponDetection detector;

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

        float dist = Vector3.Distance(player.transform.position, enemyScript.Root.transform.position);
        if (!enemyScript.attack && dist > 9f)
        {
            if (detector.GetBackWeaponType().Equals(Weapons.Bow))
            {
                detector.SwitchWeapon();
            }
            Vector3 direction;
            direction = (playerTransform.position - enemyScript.Root.transform.position);
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
        else if (!enemyScript.attack && dist <= 4f)
        {
            if (detector.GetMainWeaponType().Equals(Weapons.Bow))
            {
                detector.SwitchWeapon();
            }
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
    public void GetBow(Transform bow)
    {
        detector.PickBow(bow);
    }
    public void GetGarrote(Transform garrote)
    {
        detector.PickGarrote(garrote);
    }
}
