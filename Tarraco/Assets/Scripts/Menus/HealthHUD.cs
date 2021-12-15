using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthHUD : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    public GameObject gameOver;
    private Animator animator;
    public List<GameObject> heartList = new List<GameObject>();

    private int life;



    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
        animator = GetComponent<Animator>();
        /*foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            heartList.Add(child.gameObject);
        }*/

        life = player.GetComponent<CharacterClass>().life;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HealHUD(int bonus)
    {
        print(player.name);
        life += bonus;
        if (life <= player.GetComponent<CharacterClass>().maxLife)
        {
            animator.SetBool("Heal", true);
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
        print(animator.GetBool("Damage"));
        animator.SetInteger("Life", life);
        animator.SetBool("Damage", true);
        print(life);
        heartList[life].SetActive(false);
        if (life == 0)
        {
            GameOver();
        }
    }
    public void StopDamaging()
    {
        print("Stoped damaging");
        animator.SetBool("Damage", false);
        animator.SetBool("Heal", false);
    }

    void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0.1f;
    }

    public void Restart()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.tag = "Untagged";
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        DestroyImmediate(controller);
    }

    public void BackToMenu()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        controller.tag = "Untagged";
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        DestroyImmediate(controller);
    }
}
