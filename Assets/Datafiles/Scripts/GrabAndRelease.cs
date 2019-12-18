using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tell what hands are touching and use right click to grab object.
public class GrabAndRelease : MonoBehaviour
{
    private GameObject m_CollidingObject;
    private GameObject m_HeldObject;

    private float m_closedTime = 0f; //Computer time until the fist closes.

    public Animator animator; //This is a reference to the Animator component attached to the VRHand object in the Hierarchy.

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
    void Update()
    {
        m_closedTime += Time.deltaTime; //Counting time that the right mouse button is down
        animator.SetFloat("Closed Timer", m_closedTime);

        //If other input type, replace Mouse1 with whatever....
        //This if statement is activated only when we press the right mouse button.
        if (Input.GetKeyDown(KeyCode.Mouse1)) //Right click.
        {
            if (m_CollidingObject != null)
            {
                Grab();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (m_HeldObject != null)
            {
                Release();
            }
        }
    }

    //When we pick up the cube it becomes a child of our hand.
    void Grab()  //Grab colliding object
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
