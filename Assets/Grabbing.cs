using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;

public class Grabbing : MonoBehaviour, IGlobalTriggerPressDownHandler {

	bool isGrabbing = false;
	bool isHolding = false;

	Transform heldObject;

	//when the controller collider touches another collider set as a trigger
	void OnTriggerEnter(Collider other)
	{
		//and the object attatched to the other collider is tagged as grabbable
		if (other.tag == "Grabbable") 
		{
			//and the trigger is pressed/user is "grabbing"
			if (isGrabbing) 
			{
				heldObject = other.gameObject.transform;
				heldObject.parent = GetComponent<Transform>();
				isHolding = true;
			}
		}
	}

	//when trigger is pressed down
	void IGlobalTriggerPressDownHandler.OnGlobalTriggerPressDown(VREventData eventData){
		//query HandToggler script to see if I own this controller
		//no point writing it all twice
		if (GetComponent<HandToggler>().Host) {
			Debug.Log ("Trigger pressed to grab");
			isGrabbing = true;
			//if I'm already holding something, let it go
			if (isHolding) {
				heldObject.parent = null;
				isHolding = false;
			}
		}
	}
}
