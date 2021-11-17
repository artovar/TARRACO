using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseButtonScript : MonoBehaviour
{
    public PauseMenuController eventSystem;

    void Start()
    {

    }


    public void Resume()
    {
        eventSystem.Pause();
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
