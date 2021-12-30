using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseButtonScript : MonoBehaviour
{
    public PauseMenuController eventSystem;
    private GameObject controller;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
    }


    public void Resume()
    {
        eventSystem.Pause();
    }

    public void Exit()
    {
        controller.tag = "Untagged";
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        DestroyImmediate(controller);
    }
}
