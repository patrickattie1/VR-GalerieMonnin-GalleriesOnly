using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class that describes a tutorial (one instance for each tutorial)
public class Tutorial : MonoBehaviour
{
    //Those 2 variables are always needed
    public int order; //Order in which this tutorial is called
    public string explanation; //Explanation for each tutorial
    public GameObject image; //Image in relation with the explanation

    //Called before Start
	void Awake ()
    {
        TutorialManager.Instance.tutorials.Add(this); //Tutorials list is dynamically built
	}

    //virtual in base class -> overriding in derived classes if needed
    public virtual void CheckIfHappening() { }

}
