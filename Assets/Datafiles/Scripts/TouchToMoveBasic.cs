using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This script is attached to the Sprite object in the scene's hierarchy
/// </summary>
public class TouchToMoveBasic : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0) //Has the screen been touched? (remember that we can have multiples inputs at once)
        {
            //Now we can get information about each touch
            Touch touch = Input.GetTouch(0);

            //Remember that touch.position is always in screen coordinates (as the screen is being touched)
            //  and the position of our sprite is in World coordinates. 
            //  Therefore ouch.position has to be converted to World coordinates
            //  -> conversion from screen to world space has to be done (result is obviously a Vector3)
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            //Make sure the Z-Position of the sprite is not the same as the Camera
            //  We want the sprite to look like it is on the screen
            touchPosition.z = 0;

            //Now move the sprite (this script is attached to it.
            transform.position = touchPosition;
        }
    }
}
