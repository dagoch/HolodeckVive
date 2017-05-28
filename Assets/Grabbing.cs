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

  void update()
  {
    //if (ViveControllerReciever.GetClick(3)) {
    //  //query HandToggler script to see if I own this controller
    //  //no point writing it all twice
    //  Debug.Log("Trigger pressed");
    //  if (GetComponent<HandToggler>().Host)
    //  {
    //    Debug.Log("Trigger pressed to grab");
    //    isGrabbing = true;
    //    //if I'm already holding something, let it go
    //    if (isHolding)
    //    {
    //      Debug.Log("letting go");
    //      heldObject.parent = null;
    //      isHolding = false;
    //    }
    //  }
    //}
  }

  //Boolean stayed = false;
  //when the controller collider touches another collider set as a trigger
  void OnTriggerStay(Collider other)
	{
		//and the object attatched to the other collider is tagged as grabbable
		if (other.tag == "Grabbable") 
		{
      //Debug.Log("touching grabbable");
			//and the trigger is pressed/user is "grabbing"
			if (isGrabbing) 
			{
        //Debug.Log("Grabbing attempted");
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
    //Debug.Log("Trigger pressed");
		if (GetComponent<HandToggler>().Host) {
			Debug.Log ("Trigger pressed to grab");
			isGrabbing = !isGrabbing;
			//if I'm already holding something, let it go
			if (isHolding) {
        Debug.Log("letting go");
				heldObject.parent = null;
				isHolding = false;
			}
		}
	}
}
