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
        if (inGame) return;
        screen = GameObject.FindGameObjectWithTag("Heal").GetComponent<HubScreen>();
        screen.SetUP();
        screen.DisplayArena(chosenArena);
        screen.DisplayMode(modeSelected);
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
                if(playerDeaths[i]) revivePlayers[i] = true;
            }
        }
        if (gameOv)
        {
            //Win();
            //finished = true;
        }

        for(int o = 0; o < revivePlayers.Length; o++)
        {
            if(revivePlayers[o])
            {
                revivePlayers[o] = false;
                playerDeaths[o] = false;
                StartCoroutine(Revive());
                IEnumerator Revive()
                {
                    yield return new WaitForSeconds(10);
                    RevivePlayer(o);
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
                if (playerDeaths[i]) revivePlayers[i] = true;
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
                    yield return new WaitForSeconds(10);
                    RevivePlayer(o);
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
        switch (modeSelected)
        {
            case ModesEnum.KingOfTheHill:
                OvationSingleton.Instance.pointsToWin = 1;
                playersSpawnPoints = kothSpawnPoints;
                break;
            case ModesEnum.FreeForAll:
                OvationSingleton.Instance.pointsToWin = 3;
                playersSpawnPoints = arenaSpawnPoints;
                break;
            case ModesEnum.AgainsAI:
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

    public override int NextLevel()
    {
        SaveWeapons();
        int index = SceneManager.GetActiveScene().buildIndex;
        print(index);
        if (index == baseHubIndex || index == midHubIndex)
        {
            return firstArenaIndex + chosenArena - 1;
        }
        else if (index >= firstArenaIndex && index <= firstArenaIndex + totalArenas - 1)
        {
            playersSpawnPoints = arenaSpawnPoints;
            return midHubIndex;
        }
        else
        {
            return 0;
        }
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
}
