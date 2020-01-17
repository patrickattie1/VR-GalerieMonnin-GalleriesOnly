using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;

//This script is attached to the Left Hand of the VRRig
public class XRLocomotion : MonoBehaviour
{
    public string trackpadAxisX, trackpadAxisY; // Need inputs defined for the trackpad/joystick inputs
    public string trackpadPress; //For speed increase

    public Transform vrRig; // Moving rig, just like in teleport!

    //Define what layer to sense as floor height. 
    //Do not forget to fill up this slot in the Inspector by selecting Terrain in the drop-down list
    //  Note: Nothing means that the RayCast will hit nothing.
    public LayerMask raycastMask;

    private float movingSpeed = 3.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(trackpadPress))
        {
            movingSpeed = 3.0f;
        }
        else
        {
            movingSpeed = 1.0f;
        }

        // Record the position on the x and y axis of where the thum is placed on trackpad (or where joystic pushed)
        float trackpadX = Input.GetAxis(trackpadAxisX); //  number representing the amount/speed to move left/right
        float trackpadY = Input.GetAxis(trackpadAxisY); //  number representing the amount/speed to move Forward/back

        // Convert the x and y trackpad values into a direction - essentially how much to move forward/back and how much to move left/right
        Vector3 forward = new Vector3(transform.forward.x, 0f, transform.forward.z) * trackpadX; // forward/back component of 3D motion (Vector3) - along the horizontal plane (do move vertically)
        Vector3 right   = new Vector3(transform.right.x, 0f, transform.right.z) * trackpadY;     // the direction pointing to the right of the controller 
        
        // Apply motion now to the rig!
        vrRig.transform.position = vrRig.transform.position + (forward + right) * Time.deltaTime * movingSpeed;

        float floorHeight = GetFloorHeight(); // Get the floor height
        Vector3 rigPosition = vrRig.position; // Take note of the current rig position, so we can edit the y-value 
        rigPosition.y = floorHeight; // Change the y-value
        vrRig.transform.position = rigPosition; // Now we have the xrRig position with the y-value updated, so apply it back to the rig's position

        // Apply the floorheight to the XR Rig's position - set height of the rig
    }

    private float GetFloorHeight()
    {
        float floorHeight = 0f; // Will hold the value of the height as sensed by raycast

        RaycastHit hitInfo; // Store info about my raycast hit (including position - has height info)

        //The Terrain layer mask number is 8
        if (Physics.Raycast(Camera.main.transform.position, Vector3.down, out hitInfo, Mathf.Infinity, raycastMask )) // Perform a raycast from my head down to the ground
        {
            floorHeight = hitInfo.point.y; // Take note of the floor height (height of surface that was hit)
        }
        return floorHeight; // Return this value to the caller 
    }
}
