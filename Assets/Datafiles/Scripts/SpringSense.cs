using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script to the ButtonSenseCube gameobject
/// </summary>
public class SpringSense : MonoBehaviour
{
    public GameObject buttonCube; //Do not forget to fill this slot in the Inspector

    //The Audiosource to trigger when the button is sensed
    public AudioSource aSource; //Do not forget to fill this slot in the Inspector

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    //Make sure it is the ButtonCube object touching the ButtonSensorCube
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == buttonCube)
        {
            aSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == buttonCube)
        {
            aSource.Stop();
        }
    }
}