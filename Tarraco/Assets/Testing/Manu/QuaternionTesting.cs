using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QuaternionTesting : MonoBehaviour
{
    public Transform upperRArm, lowerRArm, upperLArm, lowerLArm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //print("Upper L Arm is: " + upperLArm.localRotation); // 0.9, 0.2, -0.3, -0.2 || 0.8, 0.3, -0.3, -0.3
        //print("Lower L Arm is: " + lowerLArm.localRotation); // 0.5, -0.2, -0.4, -0.7 || 0.5, -0.2, -0.4, -0.7
        /*if (upperRArm.GetComponent<ConfigurableJoint>().targetRotation.Equals(upperRArm.rotation))
        {*/
            print("Upper R Arm is reaching its target: " + upperRArm.GetComponent<ConfigurableJoint>().targetRotation + " || " + upperRArm.rotation);
        /*}
        if (lowerRArm.GetComponent<ConfigurableJoint>().targetRotation.Equals(lowerRArm.rotation))
        {*/
            print("Lower R Arm is reaching its target: " + upperLArm.GetComponent<ConfigurableJoint>().targetRotation + " || " + upperLArm.rotation);
        //}
    }
}
