using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    //We are moving th XRRig, so we need a reference to it
    public Transform vrRig; //Fill up the slot in the Inspector

    //A list of all transforms for the 360 spheres. Used when user is pointing laser at the "Next" panel inside the sphere.
    public List<Transform> listOf360SpheresTransforms = new List<Transform>();

    public Transform hitRole;

    // Update is called once per frame
    void Update()
    {
        Transform go = hitRole.transform.parent; //Get the parent's transform (the transform of the sphere gameobject) of the hit sign

        Debug.Log("hitRole is: " + hitRole.name);
        Debug.Log("go is: " + go.name);
        Debug.Log("Inititial VRRig position is: " + vrRig.transform.position);

        if (go != null)
        {
            //Get the index of the current room's transform s(360 sphere) in the list for the "go" transform
            int indexOfCurrent360Sphere = listOf360SpheresTransforms.IndexOf(go);
            Debug.Log("Index is: " + indexOfCurrent360Sphere);

            //Move the vrRig to the new position (next room or next index in the list)
            vrRig.transform.position = listOf360SpheresTransforms[indexOfCurrent360Sphere + 1].position;
            Debug.Log("VRRig position is: " + vrRig.transform.position);

        }
    }
}
