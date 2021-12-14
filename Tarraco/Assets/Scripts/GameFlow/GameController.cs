using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Texture2D cursor;
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject mainPlayer;
    private GameObject secPlayer;
    private bool p2;
    List<GameObject> players = new List<GameObject>();
    private int controllers;
    private int prevLen;
    private Camera cam;
    public GameObject[] healthUI;
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
        //Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        if (!p2)
        {
            if (Input.GetButtonDown("Jump2"))
            {
                secPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                cam.GetComponent<CameraControl>().AddPlayer2(secPlayer.GetComponent<PlayerController>().Root.transform);
                p2 = true;
                healthUI[1].SetActive(true);
                HealthHUD hUI = healthUI[1].GetComponentInChildren<HealthHUD>();
                hUI.player = secPlayer;
                secPlayer.GetComponent<PlayerController>().SetUp(hUI.gameObject);
            }
        }
    }
}
