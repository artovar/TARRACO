using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubScreen : MonoBehaviour
{
    [SerializeField]
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUP()
    {

    }

    public void DisplayArena(int chosenOne)
    {
        switch(chosenOne)
        {
            case 1:
                image.color = Color.white;
                break;
            case 2:
                image.color = Color.black;
                break;
        }
    }
}
