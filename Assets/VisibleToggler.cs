using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;


public class VisibleToggler : Synchronizable, IGlobalGripPressDownHandler {

  //GameObject target needs to have a mesh renderer component attached for this to work
  public List<GameObject> targets = new List<GameObject>();
  
  // 1 for visible, 0 for invisible
  int visible = 1;

  //Most of the following pasted from HandToggler.
  private ViveControllerReceiver vcreceiver;

  //Am I hosting this object? This will return true, if the index of the connected
  //ViveControllerReceiver matches the build index.
  //This way, each actor will control only their hands.
  public override bool Host {
    get {
      if (!vcreceiver) vcreceiver = GetComponent<ViveControllerReceiver>();
      return vcreceiver.index == Holojam.Tools.BuildManager.BUILD_INDEX;
    }
  }

  //Is the "master client" hosting this object? Nope.
  public override bool AutoHost { get { return false; } }

  public override string Label {
    get {
      //create a unique label for this object, based off of it's gameobject name + "-hand",
      return this.name + "-thing";
    }
  }

  public override void ResetData()
  {
    data = new Holojam.Network.Flake(0, 0, 0, 1);
  }

  protected override void Sync()
  {
    if (Host)
    {
      //I am hosting this object. What do I do?
      data.ints[0] = visible;
    }
    else
    {
      //I am not hosting this object. What do I do?
      visible = data.ints[0];
    }

    if (visible == 1)
    {
      foreach (GameObject target in targets)
      {
        target.GetComponent<MeshRenderer>().enabled = true;
      }
    }
    else
    {
      foreach (GameObject target in targets)
      {
        target.GetComponent<MeshRenderer>().enabled = false;
      }
    }
  }
  

  #region IGlobalGripPressDownHandler implementation

  void IGlobalGripPressDownHandler.OnGlobalGripPressDown(VREventData eventData)
  {
    if (Host)
    {
      Debug.Log("toggling");
      if (visible == 1)
      {
        Debug.Log("off");
        visible = 0;
      }
      else
      {
        Debug.Log("on");
        visible = 1;
      }
    }
  }


  #endregion

}
