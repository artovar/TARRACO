using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SpawnPoint : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject[] enemyPrefab;
    public GameObject[] points;
    public int secondsSpawn = 5;
    private IEnumerator coroutine;
    
    public int maxEnemies;
    private List<EnemyController> enemies = new List<EnemyController>();

    // Start is called before the first frame update
    void Start()
    {
        coroutine = spawnEnemy(secondsSpawn);
        StartCoroutine(coroutine);
    }

    private IEnumerator spawnEnemy (float time)  {
        while(true) {
            int i = 0;
            List<int> deadGuys = new List<int>();
            foreach (EnemyController e in enemies)
            {
                if (e.IsRagdoll())
                {
                    deadGuys.Add(i);
                }
                i++;
            }
            i = 0;
            foreach(int d in deadGuys)
            {
                enemies.RemoveAt(d - i);
                i++;
            }
            if(enemies.Count < maxEnemies)
            {
                GameObject enemy = enemyPrefab[UnityEngine.Random.Range(0, enemyPrefab.Length)];
                GameObject sp = BetterSP();
                //Instanciamos el prefab del enemido en el punto
                enemies.Add(Instantiate(enemy, sp.transform.position, Quaternion.identity).GetComponent<EnemyController>());
                enemies[enemies.Count - 1].MoveTowardsInSpawn(sp.transform.forward);
            }
            yield return new WaitForSeconds(time);
        }
    }

    private float distance(GameObject p1, GameObject p2) {
        float x = (p1.transform.position.x - p2.transform.position.x);
        float y = (p1.transform.position.y - p2.transform.position.y);
        float z = (p1.transform.position.z - p2.transform.position.z);
        return (float)Mathf.Sqrt(x*x + y*y + z*z);
    }

    private GameObject BetterSP() {
        float betterDistance = 0;
        GameObject betterPoint = null;
        foreach(GameObject point in points) {
            if (distance(point, thePlayer) > betterDistance) {
                betterDistance = distance(point, thePlayer);
                betterPoint = point;
            }
        }
        return betterPoint;
    }
}
