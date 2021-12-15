using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Player To Follow")]
    //Player root
    public Transform APRRoot;
    public Transform PacaRoot;

    [Header("Follow Properties")]
    //Follow values
    public float distance = 12.5f; //The distance is only used when "rotateCamera" is enabled, when disabled the camera offset is used
    private float originalDistance;
    public float smoothness = 0.15f;

    [Header("Rotation Properties")]
    //Rotate with input
    public bool rotateCamera = true;
    public float rotateSpeed = 5.0f;

    //Min & max camera angle
    public float minAngle = -45.0f;
    public float maxAngle = -10.0f;


    //Private variables
    private Camera cam;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
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

    public void AddPlayer2(Transform p2)
    {
        PacaRoot = p2;
    }


    //Camera mouse input and (clamping for rotation)
    void Update()
    {
        currentX = currentX + Input.GetAxis("Mouse X") * rotateSpeed;
        currentY = currentY + Input.GetAxis("Mouse Y") * rotateSpeed;

        currentY = Mathf.Clamp(currentY, minAngle, maxAngle);
    }


    //Camera follow and rotation
    void FixedUpdate()
    {
        if (Physics.Raycast(APRRoot.position, Vector3.back, 5, 1 << LayerMask.NameToLayer("Wall")))
        {
            offset = originalOffset + new Vector3(0, 0/*Mathf.Abs(originalOffset.z)*/, -(2 * originalOffset.z / 3));
            //distance = offset.magnitude;
        }
        else if (!Physics.Raycast(APRRoot.position, Vector3.back, 6, 1 << LayerMask.NameToLayer("Wall")))
        {
            offset = originalOffset;
            distance = originalDistance;
        }

        if(!Object.ReferenceEquals(PacaRoot, null))
        {
            var targetRotation = Quaternion.LookRotation(APRRoot.position - cam.transform.position);
            cam.transform.position = Vector3.Lerp(cam.transform.position, APRRoot.position + offset.normalized * distance, smoothness);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, smoothness);
        }
        else
        {
            distance = originalDistance + (PacaRoot.position - APRRoot.position).magnitude;
            Vector3 point = (APRRoot.position + (PacaRoot.position - APRRoot.position) / 2);
            var targetRotation = Quaternion.LookRotation(point - cam.transform.position);
            cam.transform.position = Vector3.Lerp(cam.transform.position, point + offset*(((distance)/originalDistance)/1.2f), smoothness);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, smoothness);
        }
    }
}