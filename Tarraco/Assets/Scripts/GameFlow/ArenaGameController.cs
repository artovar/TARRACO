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

    private void Start() {
        audioSource = GetComponent<AudioSource>();
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
        //print(index);

        int nextLevel = 0;
        if (index == baseHubIndex || index == midHubIndex)
        {
            nextLevel = firstArenaIndex + chosenArena - 1;
            //return firstArenaIndex + chosenArena - 1;
        }
        else if (index >= firstArenaIndex && index <= firstArenaIndex + totalArenas - 1)
        {
            nextLevel = midHubIndex;
            //return midHubIndex;
        }
        /*else
        {
            return 0;
        }*/
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
}
