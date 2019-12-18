using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tell what hands are touching and use right click to grab object.
//We will interact (paint), not right away when the Grabbing is done but also when the trigger button is pressed.
//This script is attached to the Left Hand and to the Right Hand objects.
//Make sure the Input is configured in Input Settings for the Trigger

public class XRFixedJointInteractableGrabAndRelease : MonoBehaviour
{
    private GameObject m_CollidingObject;
    private GameObject m_HeldObject;

    //This is a reference to the Animator component attached to the VRHand object in the Hierarchy
    public Animator animator;

    //XR Specific variables
    //Name of the GRIP object that we setup in the Project Settings Inputs list (setup in the corresponding slot in the Inspector)
    public string gripInputName;

    //Name of the TRIGGER object that we setup in the Project Settings Inputs list (setup in the corresponding slot in the Inspector)
    public string triggerInputName;

    //Used to determine if we are already holding the grip on the hand controller. 
    //Prevents the Update Loop to run the code 90x/sec.
    private bool gripHeld;

    //Used to determine if we are already holding the trigger on the hand controller. 
    //Prevents the Update Loop to run the code 90x/sec.
    private bool triggerPulled = false;

    //Joint between the hand and the held object.
    private FixedJoint grabJoint;

    private void OnTriggerStay(Collider other) //Other object has to have a RigidBody
    {
        if (other.GetComponent<Rigidbody>())
        {
            m_CollidingObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_CollidingObject = null;
    }

    // Update is called once per frame
    void Update() //Occurs 90 times/second in VR. But needed only when we pull the trigger
    {

        //This if statement is activated only when we press the grip button (we close the hand)
        if (Input.GetAxis(gripInputName) > 0.5f && gripHeld == false) //Grab
        {
            animator.SetBool("Closed Hand", true); //Changes the parameter in the Animator

            if (m_CollidingObject != null)
            {
                Grab(); //m_heldObject is now non null (probably a paint color if object collided is flagged a "Paint")).
            }

            //Prevents from running this code(e.g. m_animator.setBool("Close Hand", true) 
            //  and Grab()) every frame(e.g. 90x per second)
            gripHeld = true;
        }
        //Release when we open the hand
        else if (Input.GetAxis(gripInputName) < 0.5f && gripHeld == true)
        {
            animator.SetBool("Closed Hand", false); //Changes the parameter in the Animator

            if (m_HeldObject != null)
            {
                Release();
            }
            gripHeld = false;
        }


        //Monitor the trigger pull - Interact when pulled > 50%
        //Call the Interact() function of the held object if the trigger is activated. 
        //  Has to be located after Grab()
        if (Input.GetAxis(triggerInputName) > 0.5f && triggerPulled == false)
        {
            //Code to Interact (paint / shoot paintball gun / start an electric drill / etc.) 
            //Send message to m_heldObject to ask to run the "Interact" function 
            //Before anything, still make sure you are holding an object
            if (m_HeldObject != null)
            {
                //Attempt Interact if held object has a function with name Interact
                m_HeldObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            }
            triggerPulled = true; //Prevent repeated interacts while holding trigger
        }
        //Trigger is being held down then released -> StopInteract
        else if (Input.GetAxis(triggerInputName) < 0.5f && triggerPulled == true)
        {
            if (m_HeldObject != null)
            {
                //Attempt Interact if held object has a function with name StopInteract
                //(call the StopInteract function of the held object)
                m_HeldObject.SendMessage("StopInteract", SendMessageOptions.DontRequireReceiver);
            }
            triggerPulled = false; 
        }
    }

    //When we pick up the cube it becomes a child of our hand.
    void Grab()  //Grab colliding object (= Make it a child of the parent hand)
    {
        //Referencing the touched object
        m_HeldObject = m_CollidingObject;

        //Create the joint component on the hand
        grabJoint = gameObject.AddComponent<FixedJoint>();

        //Define the connected body (the held object's RigidBody)
        grabJoint.connectedBody = m_HeldObject.GetComponent<Rigidbody>();
        //Amount of force required to break the joint (the held object will fall to the ground on break)
        grabJoint.breakForce = 1000f;
        grabJoint.breakTorque = 1000f;

        //THE OLD PARENTING APPROACH
        //m_CollidingObject.GetComponent<Rigidbody>().isKinematic = true;
        //m_CollidingObject.transform.SetParent(transform);
    }

    void Release() //Release colliding object
    {
        Destroy(grabJoint); //Remove the joint object

        //THE OLD PARENTING APPROACH
        //m_HeldObject.transform.SetParent(null);
        //m_HeldObject.GetComponent<Rigidbody>().isKinematic = false;

        m_HeldObject = null;
    }
}
