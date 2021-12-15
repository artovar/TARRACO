using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SpawnPoint : MonoBehaviour
{
    public enum Mode
    {
        LEVEL,
        ARENA
    };
    public Mode mode;
    public GameObject thePlayer;
    public GameObject[] enemyPrefab;
    public Material[] materialsForSpartan;
    public GameObject[] weaponPrefab;
    public GameObject[] points;
    public int secondsSpawn = 5;
    private int deathCount = 0;
    private IEnumerator coroutine;
    
    public int maxEnemies;
    private int maxDef;
    private List<EnemyController> enemies = new List<EnemyController>();

    // Start is called before the first frame update
    void Start()
    {
        maxDef = maxEnemies;
        coroutine = spawnEnemy(secondsSpawn);
        StartCoroutine(coroutine);
    }

    private IEnumerator spawnEnemy (float time)  {
        while(true) {
            int i = 0;
            List<int> deadGuys = new List<int>();
            foreach (EnemyController e in enemies)
            {
                if (e.IsDead())
                {
                    deathCount++;
                    deadGuys.Add(i); // ESTO PUEDE DAR PROBLEMAS SI EL TIEMPO DE MUERTE ES MENOR QUE EL INTERVALO DE SPAWN
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
                int e = UnityEngine.Random.Range(0, enemyPrefab.Length);
                GameObject enemy = enemyPrefab[e];
                if(e == 0) enemy.GetComponentInChildren<SkinnedMeshRenderer>().material = materialsForSpartan[UnityEngine.Random.Range(0, materialsForSpartan.Length)];
                GameObject sp = BetterSP();
                //Instanciamos el prefab del enemido en el punto
                enemies.Add(Instantiate(enemy, sp.transform.position, Quaternion.identity).GetComponent<EnemyController>());
                enemies[enemies.Count - 1].MoveTowardsInSpawn(sp.transform.forward);
                Instantiate(weaponPrefab[UnityEngine.Random.Range(0, weaponPrefab.Length)], sp.transform.position, Quaternion.identity);
            }
            maxEnemies = (int) (maxDef * Mathf.Log(1 + deathCount));
            yield return new WaitForSeconds(time);
        }
    }

    private float Distance(GameObject p1, GameObject p2) {
        float x = (p1.transform.position.x - p2.transform.position.x);
        float y = (p1.transform.position.y - p2.transform.position.y);
        float z = (p1.transform.position.z - p2.transform.position.z);
        return (float)Mathf.Sqrt(x*x + y*y + z*z);
    }

    private GameObject BetterSP() {
        float betterDistance = 0;
        GameObject betterPoint = null;
        foreach(GameObject point in points) {
            if (Distance(point, thePlayer) > betterDistance) {
                betterDistance = Distance(point, thePlayer);
                betterPoint = point;
            }
        }
        return betterPoint;
    }
}
