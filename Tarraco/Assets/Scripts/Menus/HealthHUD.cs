using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthHUD : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    private Animator animator;
    public List<GameObject> heartList = new List<GameObject>();

    private int life;



    // Start is called before the first frame update
    void Start()
    {
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
        animator.SetInteger("Life", life);
        animator.SetBool("Damage", true);
        heartList[life].SetActive(false);
    }
    public void StopDamaging()
    {
        animator.SetBool("Damage", false);
        animator.SetBool("Heal", false);
    }
}
