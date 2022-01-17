using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OvationSingleton : MonoBehaviour
{
    private static OvationSingleton instance;
    public static OvationSingleton Instance => instance;

    protected OvationBar[] bars = new OvationBar[4];
    protected OvationBar middleBar;
    bool winned = false;
    private float time = 0;

    public int pointsToWin = 1;

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

    public virtual void Initialise()
    {

    }

    public OvationBar[] GetBars()
    {
        return bars;
    }

    private void Update()
    {
        if(!winned) time += Time.deltaTime;
    }
    public virtual void AddBar(OvationBar bar, Characters character)
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
    public virtual void IncreaseMeter(float incr, Characters character)
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

    public virtual void BarAccomplished(int score, Characters owner, TextMeshProUGUI text)
    {
        if(score >= pointsToWin)
        {
            Win(owner, text);
        }
    }

    public void Win(Characters charact, TextMeshProUGUI texto)
    {
        if (winned) return;
        winned = true;
        switch(charact)
        {
            case Characters.None:
                texto.text = "Level Finished";
                break;
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

    public virtual void ResetBars()
    {
        winned = false;
        foreach(OvationBar o in bars)
        {
            if(o != null) o.ResetBar();
        }
        if (middleBar != null) middleBar.ResetBar();
    }
}
