using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonScript : MonoBehaviour
{
    MenuController gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MenuController>();
    }

    public void Play()
    {
        gameController.Play();
    }

    public void Exit()
    {
        gameController.Exit();
    }
}
