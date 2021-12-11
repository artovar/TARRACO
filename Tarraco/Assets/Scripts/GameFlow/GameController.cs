using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject mainPlayer;
    private GameObject secPlayer;
    private bool p2;
    List<GameObject> players = new List<GameObject>();
    private int controllers;
    private int prevLen;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        mainPlayer = GameObject.FindGameObjectWithTag("Player");
        players.Add(mainPlayer);
        prevLen = Input.GetJoystickNames().Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!p2)
        {
            if (Input.GetButtonDown("Jump2"))
            {
                secPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                cam.GetComponent<CameraControl>().AddPlayer2(secPlayer.GetComponent<PlayerController>().Root.transform);
                p2 = true;
            }
        }
    }
}
