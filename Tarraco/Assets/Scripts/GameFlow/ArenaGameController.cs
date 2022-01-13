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
        base.Update();

    }

    public void ChangeMode()
    {
        modeSelected++;
        if (modeSelected > ModesEnum.AgainsAI) modeSelected = 0;
        screen.DisplayMode(modeSelected);
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
            return midHubIndex;
        }
        else
        {
            return 0;
        }
    }
}
