using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    //We are moving th XRRig, so we need a reference to it
    public Transform vrRig; //Fill up the slot in the Inspector

    //We also need the positions of the "Fly" location (up and down)
    public Transform abovePMLocation;
    public Transform aboveRSLocation;
    public Transform groundPMLocation;

    public Transform hitRole;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            ManageRaycast(hitRole);
        }
    }

    private void ManageRaycast(Transform h)
    {
        switch (h.tag)
        {
            //Moving VRRig to one of the platforms above the scene
            // (meaning that an object flagged as "Fly" has been hit by the raycast (laser))
            case "FlyAbovePM":
                Debug.Log("FlyAbovePM");
                //Wait 2s before going up
                StartCoroutine(DelayRoutine(2.0f));

                //Move the vrRig to the position (and height) of the hit platform
                vrRig.transform.position = abovePMLocation.position;
                break;

            case "FlyAboveRS":
                Debug.Log("FlyAboveRS");
                //Wait 2s before going up
                StartCoroutine(DelayRoutine(2.0f));

                //Move the vrRig to the position (and height) of the hit platform
                vrRig.transform.position = aboveRSLocation.position;
                break;            

            case "FlyGroundPM":
                Debug.Log("FlyGroundPM");
                //Wait 2s before going up
                StartCoroutine(DelayRoutine(2.0f));

                //Move the vrRig to the position (and height) of the hit platform
                vrRig.transform.position = groundPMLocation.position;
                break;
        }
    }
    private IEnumerator DelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
