using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChronoScript : MonoBehaviour
{
    static private float chronoTime;
    static private int chronoMinutes;
    static private int chronoSeconds;
    static private int chronoMillis;

    static private bool startClock = false;

    [SerializeField]
    private TextMeshProUGUI crMin;
    [SerializeField]
    private TextMeshProUGUI crSec;
    [SerializeField]
    private TextMeshProUGUI crMil;


    // Start is called before the first frame update
    void Start()
    {
        DisplayClock();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startClock) return;
        chronoTime -= Time.deltaTime * 100;
        int aux = (int)(chronoTime);
        chronoMillis = aux % 100;
        aux /= 100;
        chronoSeconds = aux % 60;
        aux /= 60;
        chronoMinutes = aux % 60;
        DisplayClock();
    }

    static public void SetClock(int minutes, int seconds, int millis)
    {
        chronoTime = millis + seconds * 60 + minutes * 3600;
        chronoMinutes = minutes;
        chronoSeconds = seconds;
        chronoMillis = millis;
    }

    void DisplayClock()
    {
        crMin.text = (chronoMinutes < 10 ? "0" : "") + chronoMinutes;
        crSec.text = (chronoSeconds < 10 ? "0" : "") + chronoSeconds;
        crMil.text = (chronoMillis < 10 ? "0" : "") + chronoMillis;
    }
    public void StartClock()
    {
        startClock = true;
    }

    static public void ResetClock()
    {
        startClock = false;
        chronoTime = 0;
        chronoMinutes = 0;
        chronoSeconds = 0;
        chronoMillis = 0;
    }
}
