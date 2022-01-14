using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSingleton : MonoBehaviour
{
    private static SkinSingleton instance;
    public static SkinSingleton Instance => instance;

    [SerializeField]
    private List<Material> materials;
    [SerializeField]
    private List<Mesh> meshes;
    private List<Boolean> bools = new List<Boolean>();

    public RuntimeAnimatorController[] controllers;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        foreach (Material mat in materials)
        {
            bools.Add(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetNextSkin(Material oldMat, out Mesh mesh, out Material mat, out RuntimeAnimatorController controller)
    {
        int f = materials.IndexOf(oldMat);
        int selected = f;
        for(int i = 0; i < bools.Count; i++)
        {
            if (bools[selected])
            {
                bools[selected] = false;
                break;
            }
            selected++;
            if (selected == bools.Count) selected = 0;
        }
        bools[f] = true;
        mesh = meshes[selected];
        mat = materials[selected];
        controller = controllers[selected];
        print(f);
    }
    public void GetNewSkin(out Mesh mesh, out Material mat, out RuntimeAnimatorController controller)
    {
        int i = 0;
        for(i = 0; i < bools.Count; i++)
        {
            if(bools[i])
            {
                bools[i] = false;
                break;
            }
        }
        mesh = meshes[i];
        mat = materials[i];
        controller = controllers[i];
    }
}
