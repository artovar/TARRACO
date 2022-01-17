using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicoOvaciones : MonoBehaviour
{
    LevelsOvationSingleton ovationSingleton;
    AudioSource source;
    float max;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        if(OvationSingleton.Instance != null)ovationSingleton = OvationSingleton.Instance.GetComponent<LevelsOvationSingleton>();
        if (ovationSingleton != null)
        {
            max = ovationSingleton.GetMiddleBarMax();
        }
        else source.volume = .3f * .4f;
    }

    // Update is called once per frame
    void Update()
    {
        if(ovationSingleton != null)
        {
            float v = ovationSingleton.GetMiddleBarValue();
            if (v >= 0)
            {
                source.volume = (v / max) * .4f;
            }
        }
    }
}
