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
        foreach(Transform t in  gameObject.GetComponentsInChildren<Transform>()) {
            t.gameObject.SetActive(true);
            heartList.Add(t.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void healHUD(int life) {
        heartList[life-1].SetActive(true);
    }

    void hurtHUD(int life) {
        heartList[life-1].SetActive(false);
    }
    
    void gameOver() {
        Time.timeScale = 0;
    }

    
}
