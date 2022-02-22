using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyboadLockScript : MonoBehaviour
{
    public Toggle toggle;
    public Sprite onCombo;
    public Sprite separated;
    public Image control;
    public Image controlBack;

    private void Start()
    {
        toggle.isOn = AudioValuesSingleton.Instance.lockKeyboard;
        if (toggle.isOn)
        {
            control.sprite = separated;
            controlBack.sprite = separated;
        }
        else
        {
            control.sprite = onCombo;
            controlBack.sprite = onCombo;
        }
    }
    public void OnTogleInteraction()
    {
        AudioValuesSingleton.Instance.LockKeyboard(toggle.isOn);
        if(toggle.isOn)
        {
            control.sprite = separated;
            controlBack.sprite = separated;
        }
        else
        {
            control.sprite = onCombo;
            controlBack.sprite = onCombo;
        }
    }
}
