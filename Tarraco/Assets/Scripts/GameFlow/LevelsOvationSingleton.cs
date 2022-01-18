using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelsOvationSingleton : OvationSingleton
{
    private int playersIn;
    private LevelGameController controller;

    public override void AddBar(OvationBar bar, Characters character)
    {
        switch (character)
        {
            case Characters.Player1:
            case Characters.Player2:
            case Characters.Player3:
            case Characters.Player4:
                if(bar.isActiveAndEnabled)
                {
                    playersIn++;
                }
                bar.DisableBar();
                break;
            case Characters.None:
                controller = GameController.Instance.GetComponent<LevelGameController>();
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
                middleBar.IncreaseMeter(incr * .2f + (incr/playersIn) * .5f + (incr/controller.currentLevel) * .3f);
                break;
        }
    }
    public override void BarAccomplished(int score, Characters owner, TextMeshProUGUI text)
    {
        GameController.Instance.GetComponent<LevelGameController>().SpawnBoss();
        middleBar.DisableBar();
    }
    public float GetMiddleBarValue()
    {
        if(middleBar.isActiveAndEnabled)
        {
            return middleBar.GetValue();
        }
        return -1;
    }
    public float GetMiddleBarMax()
    {
        return middleBar.GetMax();
    }
}