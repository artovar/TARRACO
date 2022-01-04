using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OvationSingleton : MonoBehaviour
{
    private static OvationSingleton instance;
    public static OvationSingleton Instance => instance;
    private OvationBar[] bars = new OvationBar[4];
    bool winned = false;
    private float time = 0;

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
    private void Update()
    {
        if(!winned) time += Time.deltaTime;
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
    }

    public void Win(Characters charact, TextMeshProUGUI texto)
    {
        if (winned) return;
        winned = true;
        switch(charact)
        {
            case Characters.Player1:
                texto.text = "You win!";
                break;
            case Characters.Player2:
                texto.text = "You win!";
                break;
            case Characters.Player3:
                texto.text = "You win!";
                break;
            case Characters.Player4:
                texto.text = "You win!";
                break;
        }
        GameController.Instance.Win();
    }

    public void ResetBars()
    {
        foreach(OvationBar o in bars)
        {
            if(o != null) o.ResetBar();
        }
    }
}
