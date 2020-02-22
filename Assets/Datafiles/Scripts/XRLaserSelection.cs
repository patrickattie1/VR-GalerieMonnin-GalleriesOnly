using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GMGalleriesOnly
{

    //Attach this script to the Left Hand and to the Right Hand
    //This script will use the LineRenderer as a visual for the player to select objects (here the StatCube flagged object).
    //Note: vrRig is the position of our feet.
    public class XRLaserSelection : MonoBehaviour
    {
        //We are moving the VRRig, so we need a reference to it
        public Transform vrRig; //Fill up the slot in the Inspector

        //We need to deactivate/reactivate the XRLocomotion script component when we go up/down
        public GameObject leftHand, rightHand;

        //We want the LineRenderer to point where we are pointing
        public LineRenderer line; //Fill up the slot in the Inspector

        //We need an axis name
        public string laserButtonName; //Fill up the slot in the Inspector with the Button's name

        //A list of all transforms for the 360 spheres. Used when user is pointing laser at the "Next" panel inside the sphere.
        public List<Transform> listOf360SpheresTransforms = new List<Transform>();

        //We need the positions of interest at the entry of each gallery
        public Transform inFrontOfPM;
        public Transform inFrontOfRS;

        //We also need the positions of the "Fly" location (up and down)
        public Transform abovePMLocation;
        public Transform aboveRSLocation;
        public Transform aboveMiddleLocation;
        //public Transform groundPMPortalLocation, groundRSPortalLocation, groundPMEntryLocation, groundRSEntryLocation, groundPMInsideVideoLocation, groundInsideVideoLocation;
        public Transform groundPMPortalLocation;

        //We also need access to the billboards
        public GameObject inTheAirBillboardPM; //The billboard above the PM fly platform
        public GameObject inTheAirBillboardRS; //The billboard above the RS fly platform
        public GameObject inTheAirBillboardMiddle; //The billboard above the Middle fly platform

        //Last position hit by the RayCast
        private Vector3 hitPosition; //A Vector3 declaration always initialises the vector to the origin

        //When linerenderer hits this stats cube, we show the canvas.
        private GameObject cubeGO, cubePerVisitorGO;

        //We need to "Raycast" sense the trajectory of the laser beam (the LineRenderer object named line), i.e. where we are pointing.
        private RaycastHit hit; //will contain info about what we are pointing at and the position that is hit by the laser.

        private void Start()
        {
            cubeGO = GameObject.FindGameObjectWithTag("StatCube"); //For aggregated stats
            cubePerVisitorGO = GameObject.FindGameObjectWithTag("PerUserStatCube"); //For stats per visitor

            line.enabled = false;

            line.SetPosition(0, this.transform.position);
            line.SetPosition(1, this.transform.forward * 40f);

            //Make sure all "in the air" billboards are initially not visible
            DisableAllInTheAirBillboards();

        }

        //When we go up, we need to disable the XRLocomotion script and reenable it after we go back down
        //This script will be used for thi spurpose
        private void EnableXRLocomotion(bool enableLoco)
        {
            rightHand.GetComponent<XRLocomotion>().enabled = enableLoco;
            leftHand.GetComponent<XRLocomotion>().enabled  = enableLoco;
        }

        private void DisableAllInTheAirBillboards()
        {
            inTheAirBillboardPM.SetActive(false);
            inTheAirBillboardRS.SetActive(false);
            inTheAirBillboardMiddle.SetActive(false);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //We need to define a start point at everyframe for the line object (start point = position of our right hand)
            line.SetPosition(0, this.transform.position); //Remember the Positions Indexes (0 for start position and 1 for end position)

            if (Input.GetAxis(laserButtonName) > 0.5f)
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
            else if (Input.GetAxis(laserButtonName) < 0.5f)
            {
                //Disable the laser line. Check the corresponding slot in the Inspector
                line.enabled = false;

                //Deactivate the StatisticsCanvas
                cubeGO.GetComponent<Statistics>().DisplayStatisticsCanvas(false);
                cubePerVisitorGO.GetComponent<StatisticsCanvasPerUser>().DisplayStatisticsCanvas(false);
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
                    //Allways start by disabling in the air billboards
                    DisableAllInTheAirBillboards();
                    //Make sure XRLocomotion script is enabled
                    EnableXRLocomotion(true);
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
                        //Move the vrRig to the next position (next room or next index in the list)
                        ////vrRig.transform.position = listOf360SpheresTransforms[indexOfCurrent360Sphere + 1].position;

                        //Make current sphere invisible
                        listOf360SpheresTransforms[indexOfCurrent360Sphere].gameObject.SetActive(false);
                        //Make next sphere visible
                        listOf360SpheresTransforms[indexOfCurrent360Sphere + 1].gameObject.SetActive(true);
                    }
                    break;

                //Moving VRRig to the entrance of the P. Monnin gallery (meaning that the P. Monnin sign inside the sphere has been hit by the raycast (laser))
                case "GoToPM":
                    //Allways start by disabling in the air billboards
                    DisableAllInTheAirBillboards();
                    //Move the vrRig to the entrance of the P. Monnin gallery
                    vrRig.transform.position = inFrontOfPM.position;
                    //Make sure XRLocomotion script is enabled
                    EnableXRLocomotion(true);
                    break;

                //Moving VRRig to the entrance of the R. Stephenson gallery (meaning that the R. Stephenson sign inside the sphere has been hit by the raycast (laser))
                case "GoToRS":
                    //Allways start by disabling in the air billboards
                    DisableAllInTheAirBillboards();
                    //Move the vrRig to the entrance of the R. Stephenson gallery
                    vrRig.transform.position = inFrontOfRS.position;
                    //Make sure XRLocomotion script is enabled
                    EnableXRLocomotion(true);
                    break;

                //Moving VRRig to one of the platforms above the scene
                // (meaning that an object flagged as "Fly" has been hit by the raycast (laser))
                case "FlyAbovePM":
                    //Make sure XRLocomotion script is enabled
                    EnableXRLocomotion(false);
                    //Move the vrRig to the position (and height) of the hit platform
                    //Note on Lerp: you do not "detect when Lerp is done. Instead, you call Lerp repeatedly, every frame, 
                    //   and each time you tell it how done it is
                    vrRig.transform.position = Vector3.Lerp(vrRig.transform.position, abovePMLocation.transform.position, Time.deltaTime);
                    //Make sure corresponding Billboard is made visible, only when Lerp is completed i.e. when we have arrived to the destination.
                    if (Vector3.Distance(vrRig.transform.position, abovePMLocation.transform.position) <= 7.0f)
                    {
                        DisableAllInTheAirBillboards();
                        inTheAirBillboardPM.SetActive(true);
                    }
                    break;

                case "FlyAboveRS":
                    //Make sure XRLocomotion script is enabled
                    EnableXRLocomotion(false);
                    //Move the vrRig to the position s(and height) of the hit platform
                    vrRig.transform.position = Vector3.Lerp(vrRig.transform.position, aboveRSLocation.transform.position, Time.deltaTime);
                    //Make sure corresponding Billboard is made visible, only when Lerp is completed i.e. when we have arrived to the destination.
                    if (Vector3.Distance(vrRig.transform.position, aboveRSLocation.transform.position) <= 7.0f)
                    {
                        DisableAllInTheAirBillboards();
                        inTheAirBillboardRS.SetActive(true);
                    }
                    break;

                case "FlyAboveMiddle":
                    //Make sure XRLocomotion script is enabled
                    EnableXRLocomotion(false);
                    //Move the vrRig to the position (and height) of the hit platform
                    vrRig.transform.position = Vector3.Lerp(vrRig.transform.position, aboveMiddleLocation.transform.position, Time.deltaTime);
                    //Make sure corresponding Billboard is made visible, only when Lerp is completed i.e. when we have arrived to the destination.
                    if (Vector3.Distance(vrRig.transform.position, aboveMiddleLocation.transform.position) <= 7.0f)
                    {
                        DisableAllInTheAirBillboards();
                        inTheAirBillboardMiddle.SetActive(true);
                    }
                    break;

                case "LerpOnTheGround":
                    //Progressively (Lerp) move the vrRig to the position (and height) of the hit platform
                    vrRig.transform.position = Vector3.Lerp(vrRig.transform.position, h.transform.position, Time.deltaTime);
                    //Make sure XRLocomotion script is (re)-enabled, only when Lerp is completed i.e. when we have arrived to the destination.
                    if (Vector3.Distance(vrRig.transform.position, h.transform.position) <= 7.0f)
                    {
                        EnableXRLocomotion(true);
                        DisableAllInTheAirBillboards();
                    }
                    break;

                //Exiting the application
                // (meaning that the exit sphere has been hit by the raycast (laser))
                case "Exit":
                    Application.Quit();
                    break;
            }
        }
    }
}