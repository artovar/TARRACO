using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubUI : MonoBehaviour
{
    private bool showingUI = false;
    public GameObject hubUI;
    // Update is called once per frame
    void Update()
    {
        bool iG = GameController.Instance.inGame;
        if (showingUI && iG)
        {
            hubUI.SetActive(false);
            showingUI = false;
        }
        else if(!showingUI && !iG)
        {
            hubUI.SetActive(true);
            showingUI = true;
        }
    }
}
