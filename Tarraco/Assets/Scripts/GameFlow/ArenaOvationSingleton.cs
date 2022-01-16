using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaOvationSingleton : OvationSingleton
{
    ArenaGameController controller;
    private void Start()
    {
        controller = GameController.Instance.GetComponent<ArenaGameController>();
    }

    public override void IncreaseMeter(float incr, Characters character)
    {
        switch(controller.modeSelected)
        {
            case ModesEnum.KingOfTheHill:
                break;
            case ModesEnum.FreeForAll:
                break;
            case ModesEnum.AgainsAI:
                switch (character)
                {
                    case Characters.Player1:
                        bars[0].GetComponent<OvationBar>().IncreaseMeter(incr);
                        break;
                    case Characters.Player2:
                        bars[1].GetComponent<OvationBar>().IncreaseMeter(incr);
                        break;
                    case Characters.Player3:
                        bars[2].GetComponent<OvationBar>().IncreaseMeter(incr);
                        break;
                    case Characters.Player4:
                        bars[3].GetComponent<OvationBar>().IncreaseMeter(incr);
                        break;
                }
                break;
        }
    }

    public void AccumulatePoints(float incr, Characters character)
    {
        switch (character)
        {
            case Characters.Player1:
                bars[0].GetComponent<OvationBar>().IncreaseMeter(incr);
                break;
            case Characters.Player2:
                bars[1].GetComponent<OvationBar>().IncreaseMeter(incr);
                break;
            case Characters.Player3:
                bars[2].GetComponent<OvationBar>().IncreaseMeter(incr);
                break;
            case Characters.Player4:
                bars[3].GetComponent<OvationBar>().IncreaseMeter(incr);
                break;
        }
    }

    public void SetReductionRate(float red)
    {
        foreach(OvationBar b in bars)
        {
            if(b != null) b.SetReductionRate(red);
        }
        if (middleBar != null) middleBar.SetReductionRate(red);
    }
}
