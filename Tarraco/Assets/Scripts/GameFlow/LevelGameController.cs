using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGameController : GameController
{
    [SerializeField]
    private OvationBar mainBar;

    [SerializeField]
    private int baseHubIndex;
    [SerializeField]
    private int midHubIndex;
    [SerializeField]
    private int level1Index;
    [SerializeField]
    private int totalLevels;
    [SerializeField]
    private int creditsIndex;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip musicHub;
    [SerializeField]
    private AudioClip[] musicLevels;
    [SerializeField]
    private AudioClip musicBoss;

    private int currentLevel = 0;
    protected override void AdditionalStarto()
    {
        mainBar.transform.parent.gameObject.SetActive(true);
        mainBar.gameObject.SetActive(true);
        mainBar.EnableBar();
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.B))
        {
            print("Killed Boss");
            OvationSingleton.Instance.Win(Characters.None, mainBar.GetText());
        }
    }

    public override int NextLevel()
    {
        SaveWeapons();
        int index = SceneManager.GetActiveScene().buildIndex;
        //print(index);
        int nextLevel = 0;
        if (index == baseHubIndex)
        {
            currentLevel = 1;
            nextLevel = level1Index;
            //return level1Index;
        }
        else if (index == midHubIndex)
        {
            currentLevel++;
            nextLevel = level1Index + currentLevel - 1;
            //return level1Index + currentLevel - 1;
        }
        else if (index >= level1Index && index < level1Index + totalLevels - 1)
        {
            nextLevel = midHubIndex;
            //return midHubIndex;
        }
        else if (index == level1Index + totalLevels - 1)
        {
            nextLevel = creditsIndex;
            //return creditsIndex;
        }
        /*else 
        {
            return 0;
        }*/
        if (nextLevel == midHubIndex) {
            audioSource.clip = musicHub;
        } else if (nextLevel >= level1Index && nextLevel < level1Index + totalLevels - 1) {
            print("Musica del nivel: "+currentLevel);
            audioSource.clip = musicLevels[currentLevel - 1];
        }
        audioSource.Play();
        audioSource.loop = true;
        return nextLevel;
    }

    public void SpawnBoss()
    {
        audioSource.clip = musicBoss;
        audioSource.Play();
        audioSource.loop = true;
        print("SpawningBoss");
    }
}
