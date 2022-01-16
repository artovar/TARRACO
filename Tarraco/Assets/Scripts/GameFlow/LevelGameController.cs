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

    [SerializeField]
    public SpawnPoint spawn;


    private int currentLevel = 0;
    protected override void AdditionalStarto()
    {
        mainBar.transform.parent.gameObject.SetActive(true);
        mainBar.gameObject.SetActive(true);
        mainBar.EnableBar();
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

    public override int BackToHubIndex()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index >= level1Index && index < level1Index + totalLevels - 1)
        {
            currentLevel--;
            return midHubIndex;
        }
        return -1;
    }

    public override int NextLevel()
    {
        StopAllCoroutines();
        SaveWeapons();
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == baseHubIndex)
        {
            currentLevel = 1;
            return level1Index;
        }
        else if (index == midHubIndex)
        {
            currentLevel++;
            return level1Index + currentLevel - 1;
        }
        else if (index >= level1Index && index < level1Index + totalLevels - 1)
        {
            return midHubIndex;
        }
        else if (index == level1Index + totalLevels - 1)
        {
            return creditsIndex;
        }
        else 
        {
            return 0;
        }
    }

    public void SpawnBoss()
    {
        spawn.SpawnBoss(currentLevel);
    }
}
