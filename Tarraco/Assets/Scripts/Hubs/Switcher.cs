using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    [SerializeField]
    ArenaGameController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameController.Instance.GetComponent<ArenaGameController>();
    }

    public void ChangeMode()
    {
        controller.ChangeMode();
    }
    public void ChangeArena()
    {
        controller.ChangeArena();
    }
}
