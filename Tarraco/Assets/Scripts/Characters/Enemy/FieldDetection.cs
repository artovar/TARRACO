using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDetection : MonoBehaviour
{
    public EnemyController enemy;
    private List<Transform> players = new List<Transform>();
    private Transform followedPlayer;
    float distance = 100;
    float nextDistance = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerChest"))
        {
            //print("New player detected");
            GameObject player = other.gameObject;
            //enemy.Detect(player);
            players.Add(player.transform);
        }
    }    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerChest"))
        {
            //print("Player leaving");
            GameObject player = other.gameObject;
            //enemy.Detect(player);
            players.Remove(player.transform);
            if (players.Count == 0) distance = 100;
        }
    }
    private void Update()
    {
        foreach(Transform t in players)
        {
            nextDistance = (transform.position - t.position).magnitude;
            if (nextDistance < distance) 
            {
                distance = nextDistance;
                followedPlayer = t;
            }
        }
        if (enemy.player != followedPlayer) enemy.Detect(followedPlayer.gameObject);
    }
}
