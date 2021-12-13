using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvationSingleton : MonoBehaviour
{
    private static OvationSingleton instance;
    public static OvationSingleton Instance => instance;
    private OvationBar[] bars = new OvationBar[4];

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
    public void AddBar(OvationBar bar, Characters character)
    {
        switch (character)
        {
            case Characters.Player1:
                bars[0] = bar;
                break;
            case Characters.Player2:
                bars[1] = bar;
                break;
            case Characters.Player3:
                bars[2] = bar;
                break;
            case Characters.Player4:
                bars[3] = bar;
                break;
        }
    }
    public void IncreaseMeter(float incr, Characters character)
    {
        switch(character)
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

    public void BarAccomplished()
    {
        print("Has llegado al maximo");
    }
}
