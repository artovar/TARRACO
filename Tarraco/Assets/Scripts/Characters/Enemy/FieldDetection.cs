using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDetection : MonoBehaviour
{
    public EnemyController enemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerChest"))
        {
            GameObject player = other.gameObject;
            enemy.Detect(player);
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
