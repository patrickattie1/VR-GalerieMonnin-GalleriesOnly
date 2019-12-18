using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 Description of your code
 .......................
 .......................
 .......................
 End of the description
 */
  
public class SimHandMove : MonoBehaviour
{
    //convention: public variables always declared first
    [Range(1f, 100f)]
    public float moveSpeed = 15.0f;
    [Range(1f, 25f)]
    public float turnSpeed =35.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    //This function is called when the Tab key is pressed
    void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame (60 times per second)
    void Update()
    {
        //TOOGLE MOUSE BEING LOCKED OR UNLOCKED
        //GetKeyDown used to make sure it is not Toggled many times in one button press.
        if (Input.GetKeyDown(KeyCode.Tab)) //
        {
            ToggleCursor();
        }

        //TRANSLATION ON X AND Z USING THE KEYBOARD(3 DOF)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        //TRANSLATION ON Y USING THE KEYBOARD
        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            this.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }

        // ROTATION (ANOTHER 3 DOF) USING KEYBOARD TO ROTATE AROUND THE Z AXIS
        if (Input.GetKey(KeyCode.Q)) //Q = Tilt to the left
        {
            this.transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E)) //E = Tilt to the right
        {
            this.transform.Rotate(Vector3.back * turnSpeed * Time.deltaTime);
        }

        // ROTATION (ANOTHER 3 DOF) USING THE MOUSE POSITION TO ROTATE IN X,Y PLANE. 
        //this.transform.Rotate(Vector3.up    * Input.GetAxis("Mouse X") * turnSpeed, Space.World); 
        //this.transform.Rotate(Vector3.left  * Input.GetAxis("Mouse Y") * turnSpeed);
        
        // Get the mouse delta. This is not in the range -1...1
        float h = turnSpeed * Input.GetAxis("Mouse X");
        float v = turnSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(v, h, 0);
    }
}
