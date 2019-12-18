using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is attached to the PaintBrush game object in the Hierarchy.
/// It will accomplish the following:
/// - when the player grab the paintbrush with his hands and pulls the trigger, the desired interaction will occur, 
///   i.e. the paintbrush will paint
/// This interaction is described as follows:
/// - The paintbrush touches the red paint color for example (pickup the red color in the palette)
/// - The paintbrush paints in red (paints the trail)
/// </summary>
public class InteractablePaintbrush : MonoBehaviour
{
    //PaintTrail prefab will instantiate when we pull the trigger. Paint will follow the brush
    public GameObject paintTrailPrefab; //Do not forget to fill up this slot in the Inspector

    //The trail that we are currently painting (while the trigger is held)
    private GameObject currentPaintTrail;

    //This is the material that we will paint when the trigger is pulled
    private Material paintMaterial;

    //this function will be called be the XRGrabAndRelease.cs script when trigger is pulled
    private void Interact() //This is a generic name
    {
        //First, we need to check if we have paint before painting
        if (paintMaterial != null)
        {
            //The created paintTrail will have the PaintBrush a parent object so the trail follows the paint brush
            currentPaintTrail = Instantiate(paintTrailPrefab, transform.position, Quaternion.identity, transform);

            //Set the color of the paint trail (the material)
            //Also, note that "trail Renderer" is derived from the Renderer base class
            currentPaintTrail.GetComponent<Renderer>().material = paintMaterial;
        }
    }

    //Unparent the paint trail when trigger is released
    //This function is called by the XRGrabAndRelease.cs script when the trigger is released
    private void StopInteract()
    {
        currentPaintTrail.transform.SetParent(null); //Unparent
    }

    //To detect paint from our palette, when touched
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Paint") //Check that we are in fact touching a paint cube object
        {
            //Get the material from the paint cube object (get the color)
            paintMaterial = collision.gameObject.GetComponent<Renderer>().material;

            //Make the PaintBrush turn into the color that has been picked up by the player (change the color of the brush as well to match tha paint)
            gameObject.GetComponent<Renderer>().material = paintMaterial;
        }
    }
}
