using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBaddies : MonoBehaviour
{
    public GameObject prefabPaintBall;

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(prefabPaintBall, transform.position + transform.up, Quaternion.identity);
    }
}
