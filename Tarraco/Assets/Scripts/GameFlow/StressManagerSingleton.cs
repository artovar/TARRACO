using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressManagerSingleton : MonoBehaviour
{
    private static StressManagerSingleton instance;
    public static StressManagerSingleton Instance => instance;

    public int tBarraDef;
    private int[] stressBars = new int[4];
    private Transform[] players = new Transform[4];

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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBar(int player, Transform root)
    {
        stressBars[player] = tBarraDef;
        players[player] = root;
    }
}
