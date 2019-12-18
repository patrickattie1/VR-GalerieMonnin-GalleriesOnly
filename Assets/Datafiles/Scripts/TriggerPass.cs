using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Using tags. Understand how to add a tag
public class TriggerPass : MonoBehaviour
{
    public GameObject prefabBall;

    //!!!!!!!Only one of the objects need a RigidBody
    //Uses the IsTrigger property.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Instantiate(prefabBall, transform.position + transform.up, Quaternion.identity);
        }
    }

    //Basically same as OnTriggerEnter but more parameters available
    //!!!!!!!Both objects need a RigiBody
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Instantiate(prefabBall, transform.position + transform.up, Quaternion.identity);
        }
    }
}
