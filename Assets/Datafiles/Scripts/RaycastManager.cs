using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    private RaycastHit vision; //used for detecting Raycast collisions

    [Range(1,800)]
    public float bulletforce = 20;

    public float rayLength; // Used for assigning a length to the raycast
    public GameObject paintBallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rayLength = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //This will constantly draw the ray in our SCENE VIEW so we can see where the ray is going
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayLength, Color.red, 0.5f);

        //This statement is called when the Raycast is hitting a collider in the scene
        //Casts a ray, from point origin, in direction direction, of length maxDistance, against all colliders in the Scene.
        //Returns true if the ray intersects with a Collider, otherwise false.
        //Info about the collision is in the object named vision.
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out vision, rayLength))
        {
            //Did we hit an object with flag "Target"?
            if (vision.collider.tag == "Target")
            {
                Debug.Log("Target hit");
                if (Input.GetButton("Oculus_CrossPlatform_Button4")) //The X Button on the Oculus Quest Left Controller
                {
                    GameObject paintBallGO = Instantiate(paintBallPrefab, Camera.main.transform.position, Quaternion.identity);
                    //Camera.main.transform.forward means the force is applied in the same direction as the RayCast
                    paintBallGO.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * bulletforce); //No Time.deltatime needed

                    Destroy(paintBallGO, 5);
                }
            }
        }
    }
}
