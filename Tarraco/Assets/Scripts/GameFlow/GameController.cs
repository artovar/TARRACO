using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject mainPlayer;
    List<GameObject> players = new List<GameObject>();
    private int controllers;
    private int prevLen;

    // Start is called before the first frame update
    void Start()
    {
        mainPlayer = GameObject.FindGameObjectWithTag("Player");
        players.Add(mainPlayer);
        prevLen = Input.GetJoystickNames().Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (controllers == 0 && mainPlayer.GetComponent<PlayerController>().usingController)
        {
            controllers++;
        }
        if (Input.GetJoystickNames().Length > prevLen)
        {
            controllers++;
        }
        if (players.Count < 2)
        {
            if (Input.GetButtonDown("Jump2"))
            {
                players.Add(Instantiate(playerPrefab, Vector3.zero, Quaternion.identity));
                players[players.Count].GetComponent<PlayerController>().id = players.Count;
                players[players.Count].GetComponent<PlayerController>().SetUp();

            }
        }
    }
}
