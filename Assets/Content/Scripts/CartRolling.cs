using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartRolling : MonoBehaviour
{
    AudioSource aSource;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
        transform.hasChanged = false;
        StartCoroutine("Delay");
    }

    private void LateUpdate()
    {
        if (transform.hasChanged)
        {
            Debug.Log("Play");
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
