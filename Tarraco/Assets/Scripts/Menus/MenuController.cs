using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
    }

    // Update is called once per frame

    public void LevelsMode()
    {
        SceneManager.LoadScene(1);
        mainMenu.SetActive(false);
    }

    public void ArenaMode()
    {
        SceneManager.LoadScene(3);
        mainMenu.SetActive(false);
    }

    public void Credits()
    {
        SceneManager.LoadScene(5);
        mainMenu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
