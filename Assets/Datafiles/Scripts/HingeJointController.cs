using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is attached to the LeverCube game object
/// It will track how far the lever is pushed / pulled
/// </summary>
public class HingeJointController : MonoBehaviour
{
    //We need a referenc eto the joint to access the current angle
    private HingeJoint hJoint; //do not forget to fill this slot

    //Assuming the Min and the Max angles are the same by design 5in obsolute value of course!)
    private float angleLimit;


    // Start is called before the first frame update
    void Start()
    {
        hJoint = GetComponent<HingeJoint>();

        //Take note of angle limits for normalization
        angleLimit = hJoint.limits.min;
    }

    //Calculate percentage that Lever is pushed/pulled and return the result (value between -1 and +1)
    //Note that this function can be accessed by other scripts on other game objects to perform actions like moving
    public float NormalizeControlValue ()
    {
        //Normalization
        float normValue =  (hJoint.angle / angleLimit);

        return normValue;
    }
}
