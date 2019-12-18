using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This script is attached to the Sprite object in the scene's hierarchy
/// </summary>
public class TouchToMultipleMoves : MonoBehaviour
{
    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            //Now we can get information about each touch
            Touch touch = Input.GetTouch(i);

            //Remember that touch.position is always in screen coordinates (as the screen is being touched)
            //  and the position of our sprite is in World coordinates. 
            //  Therefore ouch.position has to be converted to World coordinates
            //  -> conversion from screen to world space has to be done (result is obviously a Vector3)
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            //Draw a red line from the center of the screen to the touchPosition
            Debug.DrawLine(Vector3.zero, touchPosition, Color.red);
        } 
    }
}
