using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimHandShoot : MonoBehaviour
{
    public GameObject paintBall;

    [Range(500, 5000)]
    public float shootForce = 1500.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject tempPaintball =  Instantiate(paintBall, transform.position, transform.rotation);
            tempPaintball.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);

            Destroy(tempPaintball, 3.0f);
        }
    }
}
