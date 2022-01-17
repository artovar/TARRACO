using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaGameController : GameController
{
    [SerializeField]
    private int baseHubIndex;
    [SerializeField]
    private int midHubIndex;
    [SerializeField]
    private int firstArenaIndex;
    [SerializeField]
    private int totalArenas;
    [SerializeField]
    private int creditsIndex;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip musicHub;
    [SerializeField]
    private AudioClip[] musicArenaModes;
    [SerializeField]
    public ChronoScript crono;

    [HideInInspector]
    public ModesEnum modeSelected;
    private int chosenArena = 1;

    protected Vector3[] kothSpawnPoints =
        { new Vector3(-14f,0f, -10f), new Vector3(-14f,0f,10f),
          new Vector3(14f, 0f, 10f), new Vector3(14f, 0f, -10f) };
    protected Vector3[] arenaSpawnPoints =
        { new Vector3(-.5f,0f,-.5f), new Vector3(-.5f,0f,.5f),
          new Vector3(.5f, 0f, .5f), new Vector3(.5f, 0f, -5f) };

    private Boolean[] revivePlayers = { false, false, false, false };

    private HubScreen screen;

    protected override void AdditionalStarto()
    {
        if (inGame)
        {
            if(modeSelected == ModesEnum.FreeForAll)
            {
                ChronoScript.SetClock(5, 0, 0);
                crono.gameObject.SetActive(true);
                crono.StartClock();
            }
            return;
        }
        screen = GameObject.FindGameObjectWithTag("Heal").GetComponent<HubScreen>();
        screen.SetUP();
        screen.DisplayArena(chosenArena);
        screen.DisplayMode(modeSelected);
        ChronoScript.ResetClock();
        crono.gameObject.SetActive(false);
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        switch(modeSelected) {
            case ModesEnum.AgainsAI:
                base.Update();
                break;
            case ModesEnum.FreeForAll:
                FFAUpdate();
                break;
            case ModesEnum.KingOfTheHill:
                KOTHUpdate();
                break;
        }
    }
    void FFAUpdate()
    {
        if (finished) return;
        bool gameOv = true;
        for (int i = 0; i < playerDeaths.Length; i++)
        {
            if ((players[i] != null))
            {
                gameOv = gameOv && playerDeaths[i];
                if (playerDeaths[i])
                {
                    print("I want to revive player " + i);
                    revivePlayers[i] = true;
                }
            }
        }
        if (gameOv)
        {
            //Win();
            //finished = true;
        }

        for (int o = 0; o < revivePlayers.Length; o++)
        {
            if (revivePlayers[o])
            {
                revivePlayers[o] = false;
                playerDeaths[o] = false;
                StartCoroutine(Revive());
                IEnumerator Revive()
                {
                    int x = o;
                    yield return new WaitForSeconds(6);
                    RevivePlayer(x);
                }
            }
        }


        if (!inGame)
        {
            if (!p[1] && Input.GetButtonDown("Jump2"))
            {
                RuntimeAnimatorController cont;
                SkinSingleton.Instance.GetNewSkin(out meshes[1], out materials[1], out cont);
                SpawnPlayer();
                healthUIs[1].GetComponentInChildren<HealthHUD>().ChangeSkin(cont);
            }
            if (!p[2] && Input.GetButtonDown("Jump3"))
            {
                RuntimeAnimatorController cont;
                SkinSingleton.Instance.GetNewSkin(out meshes[2], out materials[2], out cont);
                SpawnPlayer();
                healthUIs[2].GetComponentInChildren<HealthHUD>().ChangeSkin(cont);
            }
            if (!p[3] && Input.GetButtonDown("Jump4"))
            {
                RuntimeAnimatorController cont;
                SkinSingleton.Instance.GetNewSkin(out meshes[3], out materials[3], out cont);
                SpawnPlayer();
                healthUIs[3].GetComponentInChildren<HealthHUD>().ChangeSkin(cont);
            }
        }
    }
    void KOTHUpdate()
    {
        if (finished) return;
        bool gameOv = true;
        for (int i = 0; i < playerDeaths.Length; i++)
        {
            if ((players[i] != null))
            {
                gameOv = gameOv && playerDeaths[i];
                if (playerDeaths[i])
                {
                    print("I want to revive player " + i);
                    revivePlayers[i] = true;
                }
            }
        }
        if (gameOv)
        {
            //Win();
            //finished = true;
        }

        for (int o = 0; o < revivePlayers.Length; o++)
        {
            if (revivePlayers[o])
            {
                revivePlayers[o] = false;
                playerDeaths[o] = false;
                StartCoroutine(Revive());
                IEnumerator Revive()
                {
                    int x = o;
                    yield return new WaitForSeconds(6);
                    RevivePlayer(x);
                }
            }
        }

        if (!inGame)
        {
            if (!p[1] && Input.GetButtonDown("Jump2"))
            {
                RuntimeAnimatorController cont;
                SkinSingleton.Instance.GetNewSkin(out meshes[1], out materials[1], out cont);
                SpawnPlayer();
                healthUIs[1].GetComponentInChildren<HealthHUD>().ChangeSkin(cont);
            }
            if (!p[2] && Input.GetButtonDown("Jump3"))
            {
                RuntimeAnimatorController cont;
                SkinSingleton.Instance.GetNewSkin(out meshes[2], out materials[2], out cont);
                SpawnPlayer();
                healthUIs[2].GetComponentInChildren<HealthHUD>().ChangeSkin(cont);
            }
            if (!p[3] && Input.GetButtonDown("Jump4"))
            {
                RuntimeAnimatorController cont;
                SkinSingleton.Instance.GetNewSkin(out meshes[3], out materials[3], out cont);
                SpawnPlayer();
                healthUIs[3].GetComponentInChildren<HealthHUD>().ChangeSkin(cont);
            }
        }
    }

    public void ChangeMode()
    {
        modeSelected++;
        if (modeSelected > ModesEnum.AgainsAI) modeSelected = 0;
        screen.DisplayMode(modeSelected);
        SelectMode(modeSelected);
    }
    private void SelectMode(ModesEnum selectedMode)
    {
        switch (selectedMode)
        {
            case ModesEnum.KingOfTheHill:
                OvationSingleton.Instance.GetComponent<ArenaOvationSingleton>().SetReductionRate(0.1f);
                OvationSingleton.Instance.pointsToWin = 1;
                playersSpawnPoints = kothSpawnPoints;
                break;
            case ModesEnum.FreeForAll:
                OvationSingleton.Instance.GetComponent<ArenaOvationSingleton>().SetReductionRate(1.5f);
                OvationSingleton.Instance.pointsToWin = 100;
                playersSpawnPoints = arenaSpawnPoints;
                break;
            case ModesEnum.AgainsAI:
                OvationSingleton.Instance.GetComponent<ArenaOvationSingleton>().SetReductionRate(1.5f);
                OvationSingleton.Instance.pointsToWin = 1;
                playersSpawnPoints = arenaSpawnPoints;
                break;
        }
    }

    public void ChangeArena()
    {
        chosenArena++;
        if (chosenArena > totalArenas) chosenArena = 1;
        screen.DisplayArena(chosenArena);
    }

    public override int BackToHubIndex()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index >= firstArenaIndex && index <= firstArenaIndex + totalArenas - 1)
        {
            playersSpawnPoints = arenaSpawnPoints;
            return midHubIndex;
        }
        return -1;
    }
    public override int NextLevel()
    {
        StopAllCoroutines();
        SaveWeapons();
        int index = SceneManager.GetActiveScene().buildIndex;

        int nextLevel = 0;
        if (index == baseHubIndex || index == midHubIndex)
        {
            SelectMode(modeSelected);
            nextLevel = firstArenaIndex + chosenArena - 1;
        }
        else if (index >= firstArenaIndex && index <= firstArenaIndex + totalArenas - 1)
        {
            playersSpawnPoints = arenaSpawnPoints;
            nextLevel = midHubIndex;
        }
        if (nextLevel == midHubIndex) {
           audioSource.clip = musicHub;
        } else if (nextLevel >= firstArenaIndex && nextLevel <= firstArenaIndex + totalArenas - 1) {
            audioSource.clip = musicArenaModes[chosenArena-1];
            print("Arena: "+chosenArena);
        }
        audioSource.Play();
        audioSource.loop = true;
        return nextLevel;
    }
    protected override Vector3 BestSP()
    {
        float betterDistance = 0;
        Vector3 betterPoint = Vector3.zero;
        foreach (Vector3 point in playersSpawnPoints)
        {
            float shortestToPlayers = 100;
            float trying;
            foreach (GameObject thePlayer in players)
            {
                if (thePlayer != null)
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

    public void CalculateFFAWinner()
    {
        int highest = 0;
        int current = 0;
        int id = 0;
        int finalid = 0;
        foreach (OvationBar o in OvationSingleton.Instance.GetBars())
        {
            id++;
            if (o == null) continue;
            current = o.GetScore();
            if (current > highest)
            {
                finalid = id;
                highest = current;
            }
        }
        if(finalid != 0)
        {
            OvationSingleton.Instance.Win(Characters.Player1 + finalid - 1, OvationSingleton.Instance.GetBars()[finalid - 1].GetText());
            return;
        }

        highest = 0;
        current = 0;
        id = 0;
        finalid = 0;
        foreach(GameObject g in healthUIs)
        {
            id++;
            if (!g.activeSelf)
            {
                continue;
            }
            current = g.GetComponentInChildren<HealthHUD>().GetLife();
            if(current > highest)
            {
                finalid = id;
                highest = current;
            }
        }
        if (finalid != 0)
        {
            OvationSingleton.Instance.Win(Characters.Player1 + finalid - 1, OvationSingleton.Instance.GetBars()[finalid - 1].GetText());
            return;
        }
        else
        {
            print("Empate");
        }
    }
}
