using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaHubStart : MonoBehaviour
{
    private int players;
    private float countDown = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up*15);
        if(players > 0)
        {
            countDown -= Time.deltaTime;
            print((int)countDown);
            if(countDown <= 0)
            {
                LoadGame();
            }
        }
        else
        {
            countDown = 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Foot")) players++;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Foot"))players--;
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
