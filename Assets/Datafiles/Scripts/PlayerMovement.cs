using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //To adjust the player's speed
    [Range(2, 50)]
    public float runSpeed = 10f;

    //We need a reference to the joystick
    public Joystick joystick; //Do not forget to fill up the slot in the Inspector

    private float horizontalMove = 0f;

    void Update()
    {
        if (joystick.HandleRange >= 0.2f) //Move right
        {
            //This code will give us a float value between -1 and +1
            horizontalMove = joystick.Horizontal * runSpeed * Time.deltaTime;
        }
        else if (joystick.HandleRange < 0.2f) //Move left
        {
            horizontalMove = -(joystick.Horizontal * runSpeed * Time.deltaTime);
        }
        else
        {
            horizontalMove = 0f;
        }

        Vector3 moveVector = new Vector3(horizontalMove, 0, 0);

        //Move the sprite left or right
        transform.position += moveVector;
    }
}
