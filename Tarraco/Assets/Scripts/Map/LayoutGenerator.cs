using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutGenerator : MonoBehaviour
{
    public GameObject[] layouts;
    private void Start()
    {
        if(layouts.Length > 0)
        {
            Instantiate(layouts[Random.Range(0, layouts.Length - 1)]);
        }
    }
}
