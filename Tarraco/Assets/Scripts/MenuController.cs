using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject playMenu;
    // Start is called before the first frame update
    void Start()
    {
        playMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        playMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
}
