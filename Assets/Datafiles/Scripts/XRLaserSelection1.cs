using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach this script to the Left Hand
//This script will use the LineRenderer as a visual for the player to select objects (here the Statcube object).
//Note: vrRig is the position of our feet.
public class XRLaserSelection1 : MonoBehaviour
{
    //We want the LineRenderer to point where we are pointing
    public LineRenderer line; //Fill up the slot in the Inspector

    //Last position hit by the RayCast
    Vector3 hitPosition; //A Vector3 declaration always initialises the vector to the origin

    //When linerenderer hits this cube, we show the canvas.
    private GameObject cubeGO;

    //We need to "Raycast" sense the trajectory of the laser beam (the LineRenderer object named line), i.e. where we are pointing.
    private RaycastHit hit; //will contain info about what we are pointing at and the position that is hit by the laser.

    ////bool hitIsValid = false; //To let us know if we hit a valid StatsCube collider.

    private void Start()
    {
        cubeGO = GameObject.FindGameObjectWithTag("StatCube");
        Debug.Log("cubeGO is: " + cubeGO);

        line.SetPosition(0, this.transform.position); //Remember the Positions Indexes (0 for start position and 1 for end position)
        line.SetPosition(1, this.transform.position * 50f); //The initial end of the line

        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //We need to define a start point at everyframe for the line object (start point = position of our right hand)
        line.SetPosition(0, this.transform.position); //Remember the Positions Indexes (0 for start position and 1 for end position)
        //line.SetPosition(1, this.transform.position * 50f); //The initial end of the line

        //This code should only run when we press the index trigger button (which is an axis providing a value from 0 to 1)
        if ( Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed");
            line.enabled = true; //Activate the Line renderer object so that it is visible

            //Gather information on the collider we are pointing at and perform a RayCast (a function in the Physics class)
            //Did we hit the collider of an object in the scene? If yes, run the code in the if statement
            if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, Mathf.Infinity))
            {
                Debug.Log("In Raycast");
                line.SetPosition(1, hit.point); //Reset the end of the line to new end point 

                if (hit.transform.tag == "StatCube")
                {
                    Debug.Log("StatCube touched");
                    //Display the StatisticsCanvas
                    cubeGO.GetComponent<Statistics>().DisplayStatisticsCanvas(true);
                }
            }
            else //Handling the case when no collider is hit (still want to draw laser pointer)
            {
                line.SetPosition(1, this.transform.forward * 10f); //Set the line 50 meters forward.
            }
        }
        //This code should only run when we release the laser button. If button is released, lets display 
        //   the StatisticsCanvas (and disable the laser)
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            line.enabled = false; //Disable the laser line. Check the corresponding slot in the Inspector.

            //Deactivate the StatisticsCanvas
            cubeGO.GetComponent<Statistics>().DisplayStatisticsCanvas(false);

        }

    }
}