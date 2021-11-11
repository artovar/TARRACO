using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SpawnPoint : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject enemyPrefab;
    public GameObject[] points;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        coroutine = spawnEnemy(5);
        StartCoroutine(coroutine);
    }

    private IEnumerator spawnEnemy (float time)  {
        while(true) {
            Debug.Log("Nuevo enemigo");
            GameObject sp = betterSP();
            //Instanciamos el prefab del enemido en el punto
            GameObject newEnemy = Instantiate(enemyPrefab, sp.transform.position, sp.transform.rotation);
            yield return new WaitForSeconds(time);
        }
    }

    private float distance(GameObject p1, GameObject p2) {
        float x = (p1.transform.position.x - p2.transform.position.x);
        float y = (p1.transform.position.y - p2.transform.position.y);
        float z = (p1.transform.position.z - p2.transform.position.z);
        return (float)Math.Sqrt(x*x + y*y + z*z);
    }

    private GameObject betterSP() {
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
