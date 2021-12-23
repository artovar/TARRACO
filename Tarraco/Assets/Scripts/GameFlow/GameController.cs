using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;

    public Texture2D cursor;
    [SerializeField]
    private GameObject[] playerPrefabs;
    public GameObject gameOver;

    /*private GameObject[] players;
    private GameObject secPlayer;
    */
    private bool p2, p3, p4, finished;
    private bool[] playerDeaths= {false, false, false, false};
    GameObject[] players = new GameObject[4];
    private Vector3[] points = { new Vector3(-.5f,0f,-.5f), new Vector3(-.5f,0f,.5f), new Vector3(.5f, 0f, .5f), new Vector3(.5f, 0f, -5f) };

    private int controllers;
    private int prevLen;

    private Camera cam;

    public GameObject[] healthUI;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
        cam = Camera.main;
        players[0] = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (finished) return;
        bool gameOv = true;
        for (int i = 0; i < playerDeaths.Length; i++)
        {
            if((players[i] != null)) gameOv = gameOv && playerDeaths[i];
        }
        if (gameOv)
        {
            GameOver();
            finished = true;
        }
        //Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        if (!p2 && Time.timeScale > 0)
        {
            if (Input.GetButtonDown("Jump2"))
            {
                players[1] = Instantiate(playerPrefabs[1], BetterSP(), Quaternion.identity);
                cam.GetComponent<CameraControl>().AddPlayer(players[1].GetComponent<PlayerController>().Root.transform, 2);
                p2 = true;
                healthUI[1].SetActive(true);
                HealthHUD hUI = healthUI[1].GetComponentInChildren<HealthHUD>();
                hUI.player = players[1];
                players[1].GetComponent<PlayerController>().SetUp(hUI.gameObject);
            }
        }
        if (!p3 && Time.timeScale > 0)
        {
            if (Input.GetButtonDown("Jump3"))
            {
                players[2] = Instantiate(playerPrefabs[2], BetterSP(), Quaternion.identity);
                cam.GetComponent<CameraControl>().AddPlayer(players[2].GetComponent<PlayerController>().Root.transform, 3);
                p3 = true;
                healthUI[2].SetActive(true);
                HealthHUD hUI = healthUI[2].GetComponentInChildren<HealthHUD>();
                hUI.player = players[2];
                players[2].GetComponent<PlayerController>().SetUp(hUI.gameObject);
            }
        }
        if (!p4 && Time.timeScale > 0)
        {
            if (Input.GetButtonDown("Jump4"))
            {
                players[3] = Instantiate(playerPrefabs[3], BetterSP(), Quaternion.identity);
                cam.GetComponent<CameraControl>().AddPlayer(players[3].GetComponent<PlayerController>().Root.transform, 4);
                p4 = true;
                healthUI[3].SetActive(true);
                HealthHUD hUI = healthUI[3].GetComponentInChildren<HealthHUD>();
                hUI.player = players[3];
                players[3].GetComponent<PlayerController>().SetUp(hUI.gameObject);
            }
        }
    }

    private Vector3 BetterSP()
    {
        float betterDistance = 0;
        Vector3 betterPoint = Vector3.zero;
        foreach (Vector3 point in points)
        {
            float shortestToPlayers = 100;
            float trying;
            foreach (GameObject thePlayer in players)
            {
                if(thePlayer != null)
                {
                    trying = (point - thePlayer.transform.position).magnitude;
                    if (trying < shortestToPlayers)
                    {
                        shortestToPlayers = trying;
                    }
                }
            }
            if (shortestToPlayers > betterDistance)
            {
                betterDistance = shortestToPlayers;
                betterPoint = point;
            }
        }
        return betterPoint;
    }
    void GameOver()
    {
        StartCoroutine(Slower());
    }
    private IEnumerator Slower()
    {
        while (Time.timeScale > 0.5f)
        {
            Time.timeScale -= Time.deltaTime;
            if (Time.timeScale < 0.5f) Time.timeScale = 0;
            yield return null;
        }
        gameOver.SetActive(true);
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(gameOver.GetComponentsInChildren<MenuButtonScript>()[0].gameObject);
        //SetSelectedGameObject(gameObject, new BaseEventData(eventSystem));
        //print(EventSystem.current.firstSelectedGameObject.name);
    }

    public void AddDeath(Characters c)
    {
        switch(c)
        {
            case Characters.Player1:
                playerDeaths[0] = true;
                break;
            case Characters.Player2:
                playerDeaths[1] = true;
                break;
            case Characters.Player3:
                playerDeaths[2] = true;
                break;
            case Characters.Player4:
                playerDeaths[3] = true;
                break;
        }
    }
}
