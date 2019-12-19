using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tell what hands are touching and use right click to grab object.
public class XRGrabAndRelease : MonoBehaviour
{
    private GameObject m_CollidingObject;
    private GameObject m_HeldObject;

    //This is a reference to the Animator component attached to the VRHand object in the Hierarchy
    public Animator animator;

    //XR Specific variables
    //Name of the grip object that we setup in the Project Settings Inputs list (setup in the corresponding slot in the Inspector)
    public string gripInputName;
    //Used to determine if we are already holding the object. Prevent the Update Loop to run the code 90x/sec.
    private bool gripHeld;

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

        //If other input type, replace Mouse1 with whatever....
        //This if statement is activated only when we press the grip button.
        if (Input.GetAxis(gripInputName) > 0.5f && gripHeld == false) //Grab
        {
            animator.SetBool("Closed Hand", true); //Changes the parameter in the Animator

            if (m_CollidingObject != null)
            {
                Grab();

                //Check if m_CollidingObject is colliding with the cart
                //if it is, the make the colliding object a child of the cart.
            }
            //Prevents from running this code(e.g. m_animator.setBool("Close Hand", true) and Grab()) every frame(e.g. 90x per second)
            gripHeld = true;
        }
        else if (Input.GetAxis(gripInputName) < 0.5f && gripHeld == true) //Release
        {
            animator.SetBool("Closed Hand", false); //Changes the parameter in the Animator

            if (m_HeldObject != null)
            {
                Release();
            }
            gripHeld = false;
        }
    }

    //When we pick up the cube it becomes a child of our hand.
    void Grab()  //Grab colliding object (= Make it a child of the parent hand)
    {
        m_HeldObject = m_CollidingObject; //Referencing the touched object
        m_CollidingObject.GetComponent<Rigidbody>().isKinematic = true; //Making the touched object kinematic so it does not fall
        m_CollidingObject.transform.SetParent(transform);//Make it a child of our hand so that it moves with our hand
    }

    void Release() //Release colliding object
    {
        m_HeldObject.transform.SetParent(null);
        m_HeldObject.GetComponent<Rigidbody>().isKinematic = false;
        m_HeldObject = null;
    }
}
