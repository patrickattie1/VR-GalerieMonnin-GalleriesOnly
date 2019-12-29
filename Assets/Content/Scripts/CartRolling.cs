using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CartRolling : MonoBehaviour
{
    AudioSource aSource;

    private void Start()
    {
        StartCoroutine("Delay");
        aSource = GetComponent<AudioSource>();
        transform.hasChanged = false;
    }

    private void LateUpdate()
    {
        if (transform.hasChanged)
        {
            aSource.Play();
            transform.hasChanged = false;
        }
        else
        {
            aSource.Stop();
        }      
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
    }
}
