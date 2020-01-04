using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera cameraRS;
    public Camera cameraPM;

    public Material cameraMat1;
    public Material cameraMat2;

    // Start is called before the first frame update
    void Start()
    {
        if ((cameraRS.targetTexture != null) && (cameraPM.targetTexture != null))
        {
            //Automatically adjust the screen (portal) size to the screen's size for both cameras' textures
            cameraRS.targetTexture.width = Screen.width;
            cameraRS.targetTexture.height = Screen.height;
            cameraRS.targetTexture.depth = 24;

            cameraPM.targetTexture.width  = Screen.width;
            cameraPM.targetTexture.height = Screen.height;
            cameraPM.targetTexture.depth  = 24;
        }

        
    }

}
