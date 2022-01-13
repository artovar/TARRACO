using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaOvationSingleton : OvationSingleton
{
    ArenaGameController controller;
    private void Start()
    {
        controller = GameController.Instance.GetComponent<ArenaGameController>();
    }

}
