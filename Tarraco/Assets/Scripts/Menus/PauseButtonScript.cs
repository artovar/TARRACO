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
<<<<<<< Updated upstream
        Time.timeScale = 1f;
=======
        Time.timeScale = 1;
>>>>>>> Stashed changes
        SceneManager.LoadScene(0);
    }
}
