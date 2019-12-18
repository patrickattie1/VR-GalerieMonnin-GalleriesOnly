using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Write this script
//Change the OnClick settings
//Add the scene to the Build Settings.

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        //Single = The current scene is replaced by the new one
        //Additive = 
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
