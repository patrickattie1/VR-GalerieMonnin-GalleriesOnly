using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to be used in the context of the activity named "Teleporting in VR"
//Attach this script to the Right Hand
//We want to write code that only use the vrRig. We do not want to have to move the hands.
//Note: vrRig is the position of our feet.
public class XRTeleport : MonoBehaviour
{
    //We are moving th XRRig, so we need a reference to it
    public Transform vrRig; //Fill up the slot in the Inspector

    //We want the LineRenderer to point where we are pointing
    public LineRenderer line; //Fill up the slot in the Inspector

    //We need an axis name
    public string teleportButtonName; //Fill up the slot in the Inspector with the Button's name

    //Last position hit by the RayCast
    Vector3 hitPosition; //A Vector3 declaration always initialises the vector to the origin

    //We need to "Raycast" sense the trajectory of the laser beam (the LineRenderer object named line), i.e. where we are pointing.
    private RaycastHit hit; //will contain info about what we are pointing at and the position that is hit by the laser.

    bool hitIsValid = false; //To let us know if we hit a collider that we can teleport to.

    // Update is called once per frame
    void Update()
    {
        //We need to define a start point at everyframe for the line object (start point = position of our right hand)
        line.SetPosition(0, this.transform.position); //Remember the Positions Indexes (0 for start position and 1 for end position)

        //This code should only run when we press the teleport button
        if (Input.GetButton(teleportButtonName) || Input.GetKeyDown(KeyCode.Space)) //To be able to test it with VRTK also (keyboard)
        {
            line.enabled = true; //Activate the Line renderer object so that it is visible

            //Gather information on the collider we are pointing at and perform a RayCast (a function in the Physics class)
            //Did we hit the collider of an object in the scene? If yes, run the code in the if statement
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, Mathf.Infinity))
            {
                hitIsValid = true;
                hitPosition = hit.point; //Save a the position of what is hit
                line.SetPosition(1, hitPosition); //Reset the end of the line to new end point
            }
            else //Handling the case when no collider is hit (still want to draw laser pointer)
            {
                line.SetPosition(1, this.transform.forward * 100f); //Set the line 100 meters forward.
                hitIsValid = false;
            }
        }

        //This code should only run when we release the teleport button
        //If button is released, let's try teleporting (and disable the laser)
        //GetKeyUp to be able to tes tthe code also with VRTK (Keyboard)
        if (Input.GetButtonUp(teleportButtonName) || Input.GetKeyUp(KeyCode.Space)) 
        {
            line.enabled = false; //Disable the laser line. Check the corresponding slot in the Inspector

            //First check that our raycast did not hit a collider (before teleport)
            if (hitIsValid == true)
            {
                //If your hands a NOT centered relative to the VRRig, apply the following correction:
                //   - Before moving the vrRig, apply a correction for offset between room center and headset
                //   - Calculate the distance between the VRRig position and the camera position.
                //   - Readjust the VRRig's position by adding the calculated offset.
                ///Vector3 offsetFix = vrRig.position - Camera.main.transform.position;
                ///offsetFix.y = 0f; //Prevent this offset fix from changing the height of the vrRig.position. Set the vertical component of the offsetFix to 0.
                ///vrRig.position = hitPosition + offsetFix; // Move the rig to the last valid position that we hit with our raycast while we were holding the button

                //If your hands ARE centered relative to the VRRig, just do the following:
                vrRig.position = hitPosition; // Move the rig to the last valid position that we hit with our raycast while we were holding the button

                hitIsValid = false; //Just in case, reset this boolean.
            }
        }

    }
}