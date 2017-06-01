using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;

public class Grabbing : Synchronizable, IGlobalTriggerPressDownHandler {

	int isGrabbing = 0;
	int isHolding = 0;
  int touchingGrabbable = 0;
  GameObject touchedObject;
  Transform heldObject;

  private ViveControllerReceiver vcreceiver;

  //Am I hosting this object? This will return true, if the index of the connected
  //ViveControllerReceiver matches the build index.
  //This way, each actor will control only their hands.
  public override bool Host 
  {
    get 
    {
      if (!vcreceiver) vcreceiver = GetComponent<ViveControllerReceiver>();
      return vcreceiver.index == Holojam.Tools.BuildManager.BUILD_INDEX;
    }
  }

  public override string Label 
    {
    get 
      {
      return this.name + "-grabber";
    }
  }

  public override bool AutoHost { get { return false; } }

  public override void ResetData()
  {
    data = new Holojam.Network.Flake(0, 0, 0, 3);
  }

  protected override void Sync()
  {
   if(Host)
    {
      data.ints[0] = isGrabbing;
      data.ints[1] = isHolding;
      data.ints[2] = touchingGrabbable;
    }
    else
    {
      isGrabbing = data.ints[0];
      isHolding = data.ints[1];
      touchingGrabbable = data.ints[2];
    }

    if (touchingGrabbable == 1 && isGrabbing == 1)
    {
      heldObject = touchedObject.gameObject.transform;
      heldObject.parent = GetComponent<Transform>();
      isHolding = 1;
    }
    else if (isHolding == 0 && heldObject.parent != null)
    {
      heldObject.parent = null;
    }

  }
  
  //when the controller collider touches another collider set as a trigger
  void OnTriggerStay(Collider other)
	{
		//and the object attatched to the other collider is tagged as grabbable
		if (other.tag == "Grabbable") 
		{
      touchingGrabbable = 1;
      touchedObject = other.gameObject;
		}
	}

  void OnTriggerExit(Collider other)
  {
    if(other.tag == "Grabbable")
    {
      touchingGrabbable = 0;
      touchedObject = null;
    }
  }

	//when trigger is pressed down
	void IGlobalTriggerPressDownHandler.OnGlobalTriggerPressDown(VREventData eventData){
		if (Host)
    {
			Debug.Log ("Trigger pressed to grab");
			if (isGrabbing == 1)
      {
        isGrabbing = 0;
      }
      else
      {
        isGrabbing = 1;
      }
			//if I'm already holding something, let it go
			if (isHolding == 1)
      {
				isHolding = 0;
			}
		}
	}
}
