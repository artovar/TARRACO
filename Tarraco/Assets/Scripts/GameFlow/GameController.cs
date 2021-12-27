using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;

    [SerializeField]
    public Texture2D cursor;

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Mesh[] meshes;
    [SerializeField]
    private Material[] materials;

    public GameObject gameOver;
    public GameObject pauseMenu;

    public GameObject[] healthUIs;
    GameObject[] players = new GameObject[4];
    private bool[] p = { true, false, false, false };
    public bool inGame;
    private bool finished;
    private bool[] playerDeaths= {false, false, false, false};
    private int pCount;

    private Vector3[] playersSpawnPoints = { new Vector3(-.5f,0f,-.5f), new Vector3(-.5f,0f,.5f), new Vector3(.5f, 0f, .5f), new Vector3(.5f, 0f, -5f) };

    private Camera cam;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Starto();
    }

    // Start is called before the first frame update
    public void Starto()
    {
        gameOver.SetActive(false);
        cam = Camera.main;
        pCount = 0;
        foreach(bool pl in p)
        {
            if (pl)
            {
                SpawnPlayer();
            }
        }
        for (int i = 0; i < playerDeaths.Length; i++) 
        {
            playerDeaths[i] = false;
        }
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
            case 3:
                inGame = false;
                cam.GetComponent<CameraControl>().ChangeToHub();
                break;
            case 2:
            case 4:
                inGame = true;
                cam.GetComponent<CameraControl>().ChangeToGame();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
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

        if(!inGame)
        {
            if (!p[1] && Input.GetButtonDown("Jump2"))
            {
                SpawnPlayer();
            }
            if (!p[2] && Input.GetButtonDown("Jump3"))
            {
                SpawnPlayer();
            }
            if (!p[3] && Input.GetButtonDown("Jump4"))
            {
                SpawnPlayer();
            }
        }
    }

    //SPAWNPOINT

    protected void SpawnPlayer()
    {
        pCount++;
        players[pCount-1] = Instantiate(playerPrefab, BestSP(), Quaternion.identity);
        cam.GetComponent<CameraControl>().AddPlayer(players[pCount-1].GetComponent<PlayerController>().Root.transform, pCount);
        p[pCount-1] = true;
        healthUIs[pCount-1].SetActive(true);
        HealthHUD hUI = healthUIs[pCount-1].GetComponentInChildren<HealthHUD>();
        hUI.player = players[pCount-1];
        //DESCOMENTAR CUANDO ESTE EL SKINMANAGER
        //Mesh mesh;
        //Material mat;
        //players[pCount-1].GetComponent<CharacterSkin>().SetSkin(SkinManagerSingleton.Instance.GetNextSkin(mesh, mat));
        players[pCount - 1].GetComponent<CharacterSkin>().SetSkin(meshes[pCount-1], materials[pCount-1]);
        players[pCount-1].GetComponent<PlayerController>().SetUp(hUI.gameObject, pCount);
    }

    private Vector3 BestSP()
    {
        float betterDistance = 0;
        Vector3 betterPoint = Vector3.zero;
        foreach (Vector3 point in playersSpawnPoints)
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

    //DEATH COUNT

    public void AddDeath(Characters c)
    {
        switch (c)
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

    //GAME OVER

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

    public void Exit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        gameObject.tag = "Untagged";
        Destroy(GameController.Instance.pauseMenu);
        DestroyImmediate(GameController.Instance.gameObject);
    }
}
