using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARP.APR.Scripts;

public class FieldDetection : MonoBehaviour
{
    public EnemyController enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
