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

    [SerializeField]
    private RuntimeAnimatorController test;

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

    public void HealHUD(int bonus)
    {
        life += bonus;
        if (life <= player.GetComponent<CharacterClass>().maxLife)
        {
            animator.SetBool("Heal", true);
            for(int i = 0; i < life; i++)
            {
                heartList[i].SetActive(true);
            }
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

    public void ResetLife()
    {
        if (!gameObject.activeInHierarchy) return;
        life = 3;
        animator.SetInteger("Life", life);
        animator.SetBool("Heal", true);
        animator.SetTrigger("Revive");
        for (int i = 0; i < life; i++)
        {
            heartList[i].SetActive(true);
        }
    }

    public void ChangeSkin(RuntimeAnimatorController cont)
    {
        animator.runtimeAnimatorController = cont;
    }
    
    public int GetLife()
    {
        return life;
    }
}
