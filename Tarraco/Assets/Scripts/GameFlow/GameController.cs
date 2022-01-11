using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject gameWin;
    public GameObject pauseMenu;

    public GameObject[] healthUIs;
    GameObject[] players = new GameObject[4];
    private bool[] p = { false, false, false, false };
    public bool inGame;
    private bool finished;
    private bool[] playerDeaths= {false, false, false, false};
    private int pCount;

    [SerializeField]
    private GameObject[] weapons;
    private Weapons[,] weaponList = 
        { { Weapons.None, Weapons.None }, { Weapons.None, Weapons.None }, 
          { Weapons.None, Weapons.None }, { Weapons.None, Weapons.None } };

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
        gameWin.SetActive(false);
        cam = Camera.main;
        pCount = 0;
        if (!p[0])
        {
            SkinSingleton.Instance.GetNewSkin(out meshes[0], out materials[0]);
            p[0] = true;
        }
        foreach (bool pl in p)
        {
            if (pl)
            {
                print("Spawning");
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
            case 2:
            case 3:
            case 4:
                inGame = false;
                cam.GetComponent<CameraControl>().ChangeToHub();
                break;
            case 6:
            case 7:
            case 8:
            case 9:
                inGame = true;
                cam.GetComponent<CameraControl>().ChangeToGame();
                break;
        }
        finished = false;
        AdditionalStarto();
    }

    protected virtual void AdditionalStarto()
    {

    }

    public void ChangeSkin(Characters pl)
    {
        int i = -1;
        switch(pl)
        {
            case Characters.Player1:
                i = 0;
                break;
            case Characters.Player2:
                i = 1;
                break;
            case Characters.Player3:
                i = 2;
                break;
            case Characters.Player4:
                i = 3;
                break;
        }
        if(i != -1)
        {
            SkinSingleton.Instance.GetNextSkin(materials[i], out meshes[i], out materials[i]);
            players[i].GetComponent<CharacterSkin>().SetSkin(meshes[i], materials[i]);
            //players[pCount - 1].GetComponent<PlayerController>().SetUp(hUI.gameObject, pCount);
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
            if ((players[i] != null))
            {
                gameOv = gameOv && playerDeaths[i];
            }
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
                SkinSingleton.Instance.GetNewSkin(out meshes[1], out materials[1]);
                SpawnPlayer();
            }
            if (!p[2] && Input.GetButtonDown("Jump3"))
            {
                SkinSingleton.Instance.GetNewSkin(out meshes[2], out materials[2]);
                SpawnPlayer();
            }
            if (!p[3] && Input.GetButtonDown("Jump4"))
            {
                SkinSingleton.Instance.GetNewSkin(out meshes[3], out materials[3]);
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
        players[pCount - 1].GetComponent<CharacterSkin>().SetSkin(meshes[pCount-1], materials[pCount-1]);
        players[pCount-1].GetComponent<PlayerController>().SetUp(hUI.gameObject, pCount);
        GenerateWeapons(players[pCount-1].GetComponent<PlayerController>().detector, pCount -1);
    }

    private void GenerateWeapons(WeaponDetection wd, int index)
    {
        GameObject weapon = SpawnWeapon(weaponList[index, 1]);
        if (weapon != null) wd.PickFromBegining(weapon.transform);
        weapon = SpawnWeapon(weaponList[index, 0]);
        if(weapon != null) wd.PickFromBegining(weapon.transform);
    }

    private GameObject SpawnWeapon(Weapons weapon)
    {
        int weaponIndex = 0;
        switch(weapon)
        {
            case Weapons.None:
                return null;
                break;
            case Weapons.Axe:
                weaponIndex = 0;
                break;
            case Weapons.BallChain:
                return null;
                break;
            case Weapons.BigShield:
                return null;
                break;
            case Weapons.Blowgun:
                return null;
                break;
            case Weapons.Bow:
                weaponIndex = 1;
                break;
            case Weapons.Dagger:
                return null;
                break;
            case Weapons.Discobolus:
                return null;
                break;
            case Weapons.Garrote:
                weaponIndex = 2;
                break;
            case Weapons.Halberd:
                return null;
                break;
            case Weapons.LongSword:
                return null;
                break;
            case Weapons.Slingshot:
                return null;
                break;
            case Weapons.Spear:
                weaponIndex = 3;
                break;
            case Weapons.Sword:
                weaponIndex = 4;
                break;
            case Weapons.SwordNShield:
                weaponIndex = 5;
                break;
        }
        return Instantiate(weapons[weaponIndex]);
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
            Time.timeScale -= Time.deltaTime * 2;
            if (Time.timeScale < 0.5f) Time.timeScale = 0;
            yield return null;
        }
        gameOver.SetActive(true);
        EventSystem eventSystem = EventSystem.current;
        Button[] select = gameOver.GetComponentsInChildren<Button>();
        eventSystem.SetSelectedGameObject(select[0].gameObject);
        //SetSelectedGameObject(gameObject, new BaseEventData(eventSystem));
        //print(EventSystem.current.firstSelectedGameObject.name);
    }

    public void Win()
    {
        if (finished) return;
        finished = true;
        StartCoroutine(Winning());
    }

    private IEnumerator Winning()
    {
        while (Time.timeScale > 0.5f)
        {
            Time.timeScale -= Time.deltaTime / 2;
            if (Time.timeScale < 0.5f) Time.timeScale = 0;
            yield return null;
        }
        gameWin.SetActive(true);
        EventSystem eventSystem = EventSystem.current;
        Button[] select = gameWin.GetComponentsInChildren<Button>();
        eventSystem.SetSelectedGameObject(select[0].gameObject);
    }

    public void Exit()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        gameObject.tag = "Untagged";
        Destroy(GameController.Instance.pauseMenu);
        DestroyImmediate(GameController.Instance.gameObject);
    }
    public void ResetStats()
    {
        foreach (GameObject g in healthUIs)
        {
            g.GetComponentInChildren<HealthHUD>().ResetLife();
        }
        OvationSingleton.Instance.ResetBars();
    }
    public virtual int NextLevel()
    {
        SaveWeapons();
        print("Almost xD");
        return 0;
    }

    public void SaveWeapons()
    {
        if (players[0] != null) players[0].GetComponent<PlayerController>().GetWeapons(out weaponList[0, 0], out weaponList[0, 1]);
        if (players[1] != null) players[1].GetComponent<PlayerController>().GetWeapons(out weaponList[1, 0], out weaponList[1, 1]);
        if (players[2] != null) players[2].GetComponent<PlayerController>().GetWeapons(out weaponList[2, 0], out weaponList[2, 1]);
        if (players[3] != null) players[3].GetComponent<PlayerController>().GetWeapons(out weaponList[3, 0], out weaponList[3, 1]);
    }
}
