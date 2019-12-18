using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardwareInputTest : MonoBehaviour
{
    private string trigger;

	// Use this for initialization
	void Start () //check what device we are using.
    {
        if (SystemInfo.deviceName == "Gear VR")
        {
            trigger = "GearVR_RTrigger"; //Check in Input manager
        }
        else
        {
            trigger = "OculusCrossPlatformSecondaryTrigger"; //Where it is placed on the controller(Joystick access 9th or 12th)
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis(trigger) > 0.5f)
        {

        }
	}
}
