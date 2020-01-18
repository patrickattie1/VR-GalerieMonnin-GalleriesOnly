using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Attach this script to the Left Hand
//This script will use the LineRenderer as a visual for the player to select objects (here the Statcube object).
//Note: vrRig is the position of our feet.
public class XRLaserSelection : MonoBehaviour
{
    //We are moving the VRRig, so we need a reference to it
    public Transform vrRig; //Fill up the slot in the Inspector

    //We want the LineRenderer to point where we are pointing
    public LineRenderer line; //Fill up the slot in the Inspector

    //We need an axis name
    public string laserButtonName; //Fill up the slot in the Inspector with the Button's name

    //A list of all transforms for the 360 spheres. Used when user is pointing laser at the "Next" panel inside the sphere.
    public List<Transform> listOf360SpheresTransforms = new List<Transform>();

    //We need the initial positions at the entries of each gallery
    public Transform inFrontOfPM;
    public Transform inFrontOfRS;

    //Last position hit by the RayCast
    private Vector3 hitPosition; //A Vector3 declaration always initialises the vector to the origin

    //When linerenderer hits this stats cube, we show the canvas.
    private GameObject cubeGO, cubePerVisitorGO;

    private bool triggerPulled = false;

    //We need to "Raycast" sense the trajectory of the laser beam (the LineRenderer object named line), i.e. where we are pointing.
    private RaycastHit hit; //will contain info about what we are pointing at and the position that is hit by the laser.

    ////bool hitIsValid = false; //To let us know if we hit a valid StatsCube collider.

    private void Start()
    {
        cubeGO = GameObject.FindGameObjectWithTag("StatCube"); //For aggregated stats
        cubePerVisitorGO = GameObject.FindGameObjectWithTag("PerUserStatCube"); //For stats per visitor

        line.enabled = false;
        triggerPulled = false;

        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, this.transform.forward * 40f);
    }

    // Update is called once per frame
    void Update()
    {
        //We need to define a start point at everyframe for the line object (start point = position of our right hand)
        line.SetPosition(0, this.transform.position); //Remember the Positions Indexes (0 for start position and 1 for end position)

        //This code should only run when we press the index trigger button (which is an axis providing a value from 0 to 1)
        //if ( (Input.GetAxis(laserButtonName)>0.5f  && triggerPulled == false) )

        if (Input.GetAxis(laserButtonName)> 0.5f)
        {
            //triggerPulled = true;
            line.enabled = true; //Activate the Line renderer object so that it is visible

            //Gather information on the collider we are pointing at and perform a RayCast (a function in the Physics class)
            //Did we hit the collider of an object in the scene? If yes, run the code in the if statement
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 300f))
            {
                line.SetPosition(1, hit.point); //Reset the end of the line to new end point 

                //Action depending on which object has been hit
                ManageRaycast(hit);
            }
            else //Handling the case when no collider is hit (still want to draw laser pointer)
            {
                //Writing line.SetPosition(1, this.transform.forward * 100f) is wrong!
                line.SetPosition(1, this.transform.position + transform.forward * 300f); //Set the line's end to infinity
            }
        }
        //This code should only run when we release the laser button. If button is released, lets display 
        //   the StatisticsCanvas (and disable the laser)
        else if (Input.GetAxis(laserButtonName)<0.5f)
        {
            //Disable the laser line. Check the corresponding slot in the Inspector
            line.enabled = false;

            //Deactivate the StatisticsCanvas
            cubeGO.GetComponent<Statistics>().DisplayStatisticsCanvas(false);
        }
    }

    private void ManageRaycast(RaycastHit h)
    {
        switch (h.transform.tag)
        {
            case "StatCube":
                //Display the StatisticsCanvas
                cubeGO.GetComponent<Statistics>().DisplayStatisticsCanvas(true);
                break;

            case "PerUserStatCube":
                //Display the StatisticsCanvasPerUser
                cubePerVisitorGO.GetComponent<StatisticsCanvasPerUser>().DisplayStatisticsCanvas(true);
                break;

            //Moving VRRig to next room
            case "GoToNext":
                //The Next sign inside the current sphere has been hit
                //Remember that the list is a list of Transform, not a list of GameObject
                //NOTE: Do not use the GetComponentInParent() method as it will start searching on the given object and not on the first
                // parent. So using this method to find a Transform will return the Transform of the object and not of the parent.
                Transform go = h.transform.parent; //Get the parent's transform (the transform of the sphere gameobject) of the hit sign

                //Get the index of the current room's transform s(360 sphere) in the list for the "go" transform
                int indexOfCurrent360Sphere = listOf360SpheresTransforms.IndexOf(go);

                //If all rooms have been visited, reset the index to start from the beginning again
                if (indexOfCurrent360Sphere == 16)
                {
                    indexOfCurrent360Sphere = -1;
                }

                if (go != null)
                {
                    //Wait 2s before going to next room
                    StartCoroutine(DelayRoutine(2.0f));

                    //Move the vrRig to the next position (next room or next index in the list)
                    vrRig.transform.position = listOf360SpheresTransforms[indexOfCurrent360Sphere + 1].position;
                }
                break;

                //Moving VRRig to the entrance of the P. Monnin gallery 
                // (meaning that the P. Monnin sign inside the sphere has been hit by the raycast (laser))
            case "GoToPM":
                //Wait 2s before going in front of PM's gallery
                StartCoroutine(DelayRoutine(2.0f));
                //Move the vrRig to the next position (next room or next index in the list)
                vrRig.transform.position = inFrontOfPM.position;
                break;

            //Moving VRRig to the entrance of the R. Stephenson gallery
            // (meaning that the R. Stephenson sign inside the sphere has been hit by the raycast (laser))
            case "GoToRS":
                //Wait 2s before going in front of PM's gallery
                StartCoroutine(DelayRoutine(2.0f));
                //Move the vrRig to the next position (next room or next index in the list)
                vrRig.transform.position = inFrontOfRS.position;
                break;
        }
    }

    private IEnumerator DelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}