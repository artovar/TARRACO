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
    public float smoothness = 0.07f;

    [Header("Rotation Properties")]
    //Rotate with input
    public bool rotateCamera = true;
    public float rotateSpeed = 5.0f;

    //Min & max camera angle
    public float minAngle = -45.0f;
    public float maxAngle = -10.0f;


    //Private variables
    private Camera cam;
    private Quaternion rotation;
    private Vector3 dir;
    private Vector3 offset;
    private Vector3 originalOffset;


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

    public void AddPlayer(Transform p, int pNumber)
    {
        Roots[pNumber-1] = p;
    }


    //Camera follow and rotation
    void FixedUpdate()
    {
        if (Physics.Raycast(Roots[0].position, Vector3.back, 5, 1 << LayerMask.NameToLayer("Wall")))
        {
            offset = originalOffset + new Vector3(0, 0, -(2 * originalOffset.z / 3));
        }
        else if (!Physics.Raycast(Roots[0].position, Vector3.back, 6, 1 << LayerMask.NameToLayer("Wall")))
        {
            offset = originalOffset;
            distance = originalDistance;
        }
        Vector3 point = Vector3.zero;
        float minX = Roots[0].position.x;
        float maxX = Roots[0].position.x;
        float minZ = Roots[0].position.z;
        float maxZ = Roots[0].position.z;
        float i = 0;
        foreach(Transform t in Roots)
        {
            if(t != null)
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
        float mag = (new Vector3(maxX, 0f, maxZ) - new Vector3(minX, 0f, minZ)).magnitude;
        if (mag < 5f) mag = 5f;
        distance = originalDistance + mag;
        var targetRotation = Quaternion.LookRotation(point - cam.transform.position);
        cam.transform.position = Vector3.Lerp(cam.transform.position, point + offset*(((distance)/originalDistance)/1.2f), smoothness);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, smoothness*2f);
    }
}