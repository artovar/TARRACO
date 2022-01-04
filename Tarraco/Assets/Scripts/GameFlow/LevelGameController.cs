using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGameController : GameController
{
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


    private int currentLevel = 0;

    public override int NextLevel()
    {
        SaveWeapons();
        int index = SceneManager.GetActiveScene().buildIndex;
        print(index);
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
}
