using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonManuMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
    }
    public void Press()
    {
        SceneManager.LoadScene(1);
    }
}
