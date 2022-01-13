using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    [SerializeField]
    ArenaGameController controller;

    public enum ButtonKind
    {
        mode,
        map
    };
    public ButtonKind kind;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameController.Instance.GetComponent<ArenaGameController>();
    }
    public void Interact()
    {
        switch(kind)
        {
            case ButtonKind.map:
                ChangeArena();
                break;
            case ButtonKind.mode:
                ChangeMode();
                break;
        }
    }

    private void ChangeMode()
    {
        controller.ChangeMode();
    }
    private void ChangeArena()
    {
        controller.ChangeArena();
    }
}
