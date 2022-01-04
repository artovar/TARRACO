using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Player To Follow")]
    //Player root
    public Transform[] Roots = new Transform[4];

    [Header("Follow Properties")]
    //Follow values
    public float distance = 12.5f;
    private float originalDistance = 12.5f;
    public float smoothness = 0.09f;
    public float menosAltura = 10f;

    [Header("Rotation Properties")]
    //Rotate with input
    public float rotateSpeed = 5.0f;

    //Private variables
    private Camera cam;
    private Quaternion rotation;
    private Vector3 dir;
    private Vector3 offset;
    private Vector3 originalOffset;

    private enum CAMMODE{
        Hub,
        InGame
    };
    CAMMODE mode = CAMMODE.Hub;



    //Lock and hide cursor
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        cam = Camera.main;
        originalDistance = distance;
        offset = cam.transform.position;
        originalOffset = offset;
    }

    public void ChangeToHub()
    {
        cam = Camera.main;
        Start();
        mode = CAMMODE.Hub;
    }

    public void ChangeToGame()
    {
        cam = Camera.main;
        Start();
        mode = CAMMODE.InGame;
    }

    public void AddPlayer(Transform p, int pNumber)
    {
        Roots[pNumber-1] = p;
    }
    public void RemovePlayer(int pNumber)
    {
        Roots[pNumber - 1] = null;
    }


    //Camera follow and rotation
    void FixedUpdate()
    {
        /*
        if (Physics.Raycast(Roots[0].position, Vector3.back, 5, 1 << LayerMask.NameToLayer("Wall")))
        {
            offset = originalOffset + new Vector3(0, 0, -(2 * originalOffset.z / 3));
        }
        else if (!Physics.Raycast(Roots[0].position, Vector3.back, 6, 1 << LayerMask.NameToLayer("Wall")))
        {
            offset = originalOffset;
            distance = originalDistance;
        }*/
        switch(mode)
        {
            case CAMMODE.Hub:
                MoveInHub();
                break;
            case CAMMODE.InGame:
                MoveInGame();
                break;
        }
    }
    private void MoveInHub()
    {
        Vector3 point = Vector3.zero;
        float minX = 0, maxX = 0, minZ = 0, maxZ = 0;
        foreach (Transform t in Roots)
        {
            if (t != null)
            {
                minX = t.position.x;
                maxX = t.position.x;
                minZ = t.position.z;
                maxZ = t.position.z;
                break;
            }
        }
        float i = 0;
        foreach (Transform t in Roots)
        {
            if (t != null)
            {
                if (t.position.x < minX) minX = t.position.x;
                if (t.position.x > maxX) maxX = t.position.x;
                if (t.position.z < minZ) minZ = t.position.z;
                if (t.position.z > maxZ) maxZ = t.position.z;
                point += t.position;
                i++;
            }
        }
        //var targetRotation = Quaternion.LookRotation(APRRoot.position - cam.transform.position);
        //cam.transform.position = Vector3.Lerp(cam.transform.position, APRRoot.position + offset.normalized * distance, smoothness);
        //cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, smoothness);
        point /= i;
        point = Vector3.up * 2 + point * .35f;
        float mag = 10;//(new Vector3(maxX, 0f, maxZ) - new Vector3(minX, 0f, minZ)).magnitude;
        //if (mag < 10f) mag = 10f;
        distance = originalDistance + mag;
        var targetRotation = Quaternion.LookRotation(point - cam.transform.position);
        cam.transform.position = Vector3.Lerp(cam.transform.position, point + offset * (((distance) / originalDistance) / 1.2f) - Vector3.up*menosAltura, smoothness);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, smoothness * rotateSpeed);
    }

    private void MoveInGame()
    {
        Vector3 point = Vector3.zero;
        float minX = 0, maxX = 0, minZ = 0, maxZ = 0;
        foreach (Transform t in Roots)
        {
            if (t != null)
            {
                minX = t.position.x;
                maxX = t.position.x;
                minZ = t.position.z;
                maxZ = t.position.z;
                break;
            }
        }
        float i = 0;
        foreach (Transform t in Roots)
        {
            if (t != null)
            {
                if (t.position.x < minX) minX = t.position.x;
                if (t.position.x > maxX) maxX = t.position.x;
                if (t.position.z < minZ) minZ = t.position.z;
                if (t.position.z > maxZ) maxZ = t.position.z;
                point += t.position;
                i++;
            }
        }
        //var targetRotation = Quaternion.LookRotation(APRRoot.position - cam.transform.position);
        //cam.transform.position = Vector3.Lerp(cam.transform.position, APRRoot.position + offset.normalized * distance, smoothness);
        //cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, smoothness);
        if (i == 0) i = 1;
        point /= i;
        float mag = (new Vector3(maxX, 0f, maxZ) - new Vector3(minX, 0f, minZ)).magnitude;
        if (mag < 10f) mag = 10f;
        distance = originalDistance + mag;
        var targetRotation = Quaternion.LookRotation(point - cam.transform.position);
        cam.transform.position = Vector3.Lerp(cam.transform.position, point + offset * (((distance) / originalDistance) / 1.2f), smoothness);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, smoothness * 2f);
    }
}