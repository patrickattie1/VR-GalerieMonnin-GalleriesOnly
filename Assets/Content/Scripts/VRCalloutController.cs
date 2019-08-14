using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;


//this script will make the CalloutCanvas visible when the reticle 
//   is hovering over the photo of the looket at property.
[RequireComponent (typeof(VRInteractiveItem))]
public class VRCalloutController : MonoBehaviour 
{
	//Canvas that will be shown or hidden
	public Canvas canvas;

	//VR Interactive object component
	VRInteractiveItem vrInteractive;

	//Is it visible by default
	public bool isInitiallyVisible = false;

	//Activate when hovering the reticle over?
	public bool isHoverActivated = false;
	//Activate when clicking?
	public bool isClickActivated = false;

	void Awake()
	{
		vrInteractive = GetComponent<VRInteractiveItem> ();

		if (isInitiallyVisible) 
		{
			canvas.enabled = true;
		} 
		else 
		{
			canvas.enabled = false;
		}
	}

    //This function is called when the object becomes enabled and active
    //Note the order of execution: Awake() -> OnEnable() -> Start() -> FixedUpdate() -> Update() -> LateUpdate()
    void OnEnable()
	{
		//Hook on to hovering events
		if (isHoverActivated) 
		{
			vrInteractive.OnOver += ShowCanvas; //Subscribe to OnOver event
			vrInteractive.OnOut  += HideCanvas; //Subscribe to  OnOut event
		}

		//Hook on to clicking events
		if (isClickActivated) 
		{
			vrInteractive.OnClick += ToggleCanvas;
		}
	}

    // This function is called when the object is destroyed and can be used for any cleanup code
    void OnDisable()
	{
		//Hook on to hovering events
		if (isHoverActivated) 
		{
			vrInteractive.OnOver -= ShowCanvas;
			vrInteractive.OnOut  -= HideCanvas;
		}

		//Hook on to clicking events
		if (isClickActivated) 
		{
			vrInteractive.OnClick -= ToggleCanvas;
		}
	}

	void ShowCanvas()
	{
		canvas.enabled = true;
	}

	void HideCanvas()
	{
		canvas.enabled = false;
	}

	void ToggleCanvas()
	{
		canvas.enabled = !canvas.enabled;
	}
}
