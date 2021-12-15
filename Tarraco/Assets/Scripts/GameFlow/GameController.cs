using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Texture2D cursor;
    [SerializeField]
    private GameObject[] playerPrefabs;

    /*private GameObject[] players;
    private GameObject secPlayer;
    */
    private bool p2, p3, p4;
    GameObject[] players = new GameObject[4];

    private int controllers;
    private int prevLen;

    private Camera cam;

    public GameObject[] healthUI;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        players[0] = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        if (!p2)
        {
            if (Input.GetButtonDown("Jump2"))
            {
                players[1] = Instantiate(playerPrefabs[1], Vector3.zero, Quaternion.identity);
                cam.GetComponent<CameraControl>().AddPlayer(players[1].GetComponent<PlayerController>().Root.transform, 2);
                p2 = true;
                healthUI[1].SetActive(true);
                HealthHUD hUI = healthUI[1].GetComponentInChildren<HealthHUD>();
                hUI.player = players[1];
                players[1].GetComponent<PlayerController>().SetUp(hUI.gameObject);
            }
        }
        if (!p3)
        {
            if (Input.GetButtonDown("Jump3"))
            {
                players[2] = Instantiate(playerPrefabs[2], Vector3.zero, Quaternion.identity);
                cam.GetComponent<CameraControl>().AddPlayer(players[2].GetComponent<PlayerController>().Root.transform, 3);
                p3 = true;
                healthUI[2].SetActive(true);
                HealthHUD hUI = healthUI[2].GetComponentInChildren<HealthHUD>();
                hUI.player = players[2];
                players[2].GetComponent<PlayerController>().SetUp(hUI.gameObject);
            }
        }
        if (!p4)
        {
            if (Input.GetButtonDown("Jump4"))
            {
                players[3] = Instantiate(playerPrefabs[3], Vector3.zero, Quaternion.identity);
                cam.GetComponent<CameraControl>().AddPlayer(players[3].GetComponent<PlayerController>().Root.transform, 4);
                p4 = true;
                healthUI[3].SetActive(true);
                HealthHUD hUI = healthUI[3].GetComponentInChildren<HealthHUD>();
                hUI.player = players[3];
                players[3].GetComponent<PlayerController>().SetUp(hUI.gameObject);
            }
        }
    }
}
