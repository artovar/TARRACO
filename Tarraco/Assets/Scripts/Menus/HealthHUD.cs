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
            print(heartList.Count);
        }

        life = player.GetComponent<CharacterClass>().life;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void healHUD(int bonus)
    {
        if (life == player.GetComponent<CharacterClass>().maxLife)
        {
            life += bonus;
            heartList[life].SetActive(true);
        }
    }

    public void hurtHUD(int damage)
    {
        life -= damage;

        print("te queda esta vida:" + life);
        heartList[life].SetActive(false);
        if (life == 0)
        {
            gameOver();
        }
    }

    void gameOver()
    {
        Time.timeScale = 0.2f;
    }


}
