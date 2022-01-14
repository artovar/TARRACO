using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutGenerator : MonoBehaviour
{
    public GameObject[] level1Layouts;
    public GameObject[] level2Layouts;
    public GameObject[] arenaLayouts;
    public GameObject[] kothLayouts;
    public enum Generation
    {
        Arena,
        KingOfTheHill,
        Level1,
        Level2
    };
    public Generation generation;
    private void Awake()
    {
        ArenaGameController controller = GameController.Instance.GetComponent<ArenaGameController>();
        if(controller != null)
        {
            switch (controller.modeSelected)
            {
                case ModesEnum.KingOfTheHill:
                    generation = Generation.KingOfTheHill;
                    break;
            }
        }
    }

    private void Start()
    {
        switch(generation)
        {
            case Generation.Arena:
                if (arenaLayouts.Length > 0)
                {
                    Instantiate(arenaLayouts[Random.Range(0, arenaLayouts.Length - 1)]);
                }
                break;
            case Generation.KingOfTheHill:
                if (kothLayouts.Length > 0)
                {
                    Instantiate(kothLayouts[Random.Range(0, kothLayouts.Length - 1)]);
                }
                break;
            case Generation.Level1:
                if (level1Layouts.Length > 0)
                {
                    Instantiate(level1Layouts[Random.Range(0, level1Layouts.Length - 1)]);
                }
                break;
            case Generation.Level2:
                if (level2Layouts.Length > 0)
                {
                    Instantiate(level2Layouts[Random.Range(0, level2Layouts.Length - 1)]);
                }
                break;
        }
    }
}
