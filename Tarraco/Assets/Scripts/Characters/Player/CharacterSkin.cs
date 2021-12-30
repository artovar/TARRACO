using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkin : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    
    public void SetSkin(Mesh mesh, Material material)
    {
        meshRenderer.material = material;
        meshRenderer.sharedMesh = mesh;
    }
}
