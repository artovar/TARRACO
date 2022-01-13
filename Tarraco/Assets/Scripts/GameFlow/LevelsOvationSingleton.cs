using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelsOvationSingleton : OvationSingleton
{

    public override void AddBar(OvationBar bar, Characters character)
    {
        switch (character)
        {
            case Characters.Player1:
            case Characters.Player2:
            case Characters.Player3:
            case Characters.Player4:
                bar.DisableBar();
                break;
            case Characters.None:
                middleBar = bar;
                break;
        }
    }
    public override void IncreaseMeter(float incr, Characters character)
    {
        switch (character)
        {
            case Characters.Player1:
            case Characters.Player2:
            case Characters.Player3:
            case Characters.Player4:
                middleBar.IncreaseMeter(incr);
                break;
        }
    }
    public override void BarAccomplished(int score, Characters owner, TextMeshProUGUI text)
    {
        GameController.Instance.GetComponent<LevelGameController>().SpawnBoss();
        middleBar.DisableBar();
    }
}