using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthHUD : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    public GameObject gameOver;

    private int life;



    private List<GameObject> heartList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            heartList.Add(child.gameObject);
        }

        life = player.GetComponent<CharacterClass>().life;


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HealHUD(int bonus)
    {
        life += bonus;
        if (life <= player.GetComponent<CharacterClass>().maxLife)
        {
            heartList[life - 1].SetActive(true);
        }
        else
        {
            life = player.GetComponent<CharacterClass>().maxLife;
        }
    }

    public void HurtHUD(int damage)
    {
        if (life <= 0) return;
        life -= damage;

        heartList[life].SetActive(false);
        if (life == 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0.1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
