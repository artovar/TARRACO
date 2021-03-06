using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioValuesSingleton : MonoBehaviour
{
    private static AudioValuesSingleton instance;
    public static AudioValuesSingleton Instance => instance;


    public float musicVolume;
    public float effectsVolume;
    public float peopleVolume;
    public float masterVolume;

    public bool lockKeyboard;


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

    public void LockKeyboard(bool lck)
    {
        lockKeyboard = lck;
    }
}
