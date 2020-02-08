using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Haptics;

//This script is attached to each of the collider planes
public class PortalTeleporter : MonoBehaviour
{
    //We need a reference to the Player (here the VRRig)
    public Transform player; //Make sure vrRig is tagged as player

    //We also need to know where we should teleport our player
    public Transform receiverPortal;

    private bool playerIsOverlapping = false;

    //We need to use the Vibrate method in the OculusHapticsController.cs script (attached to Left/Right Hand)
    //Uncomment for left controller haptics
    ////[SerializeField]
    ////OculusHapticsController leftControllerHaptics;
    [SerializeField]
    OculusHapticsController rightControllerHaptics;

    void FixedUpdate()
    {
        if (playerIsOverlapping)
        {
            //We need to make sure the Player is entering the portal from the right side.
            //We will use the dot product to achieve this.
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            ////Debug.Log("DP is: " + dotProduct);

            if (dotProduct < 0f) // The Player has just crossed the portal
            {
                //Vibrate the left/right hand controller
                ////leftControllerHaptics.Vibrate(VibrationForce.Hard);
                rightControllerHaptics.Vibrate(VibrationForce.Hard);
                OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);

                ////Debug.Log("DP is negative");
                //****Fix the rotation
                //Align the player with the orientation of the destination portal
                float rotationDifference = -Quaternion.Angle(transform.rotation, receiverPortal.rotation);
                rotationDifference += 180;
                //Entering the initial portal from the front and arriving behind the destination portal makes a better effect (180 degrees rotation)
                player.Rotate(Vector3.up, rotationDifference);

                //****Do the actual translation
                //Current offset between the player and the start portal. Make sure it is also rotated when applied.
                //  (this vector has already been calculated before with the portalToPlayer vector).
                Vector3 positionOffset = Quaternion.Euler(0f, rotationDifference, 0f) * portalToPlayer;
                //Make sure the Player is teleported a few meters further than the ReceiveRPortal so that it does not loop back and forth between portals.
                player.position = receiverPortal.position + positionOffset - receiverPortal.forward * 3.0f;

                playerIsOverlapping = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bool is: " + playerIsOverlapping);

        if (other.tag == "Player")
        {
            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }
}
