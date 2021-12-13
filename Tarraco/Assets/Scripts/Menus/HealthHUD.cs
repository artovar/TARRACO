using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHUD : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    private int life;

    private List<GameObject> heartList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
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
        Time.timeScale = 0.5f;
    }
}
