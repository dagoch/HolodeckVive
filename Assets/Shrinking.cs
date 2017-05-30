using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;


public class Shrinking : Synchronizable, IGlobalTouchpadPressDownHandler
{

  //GameObjects whose scale will be changed 
  public List<GameObject> targets = new List<GameObject>();
  //Public modifier which determines the final fraction of the user's original size that the shrinking will stop at
  public float finalFraction = 0.0f;
  float[] originalScales;
  float[] minScales;
  //this will be set to the standing height of the user before they begin "shrinking"
  float maxHeight;
  //the user will continue to shrink until this height is reached
  float minHeight;
  //the current fraction of the maximum height/scale the user is on
  float currentFraction;
  //whether or not the user is shrinking
  int shrinking = 0;

  public Transform userHeadTransform;

  int pressing = 0;

  //Most of the following pasted from HandToggler.
  private ViveControllerReceiver vcreceiver;

  void Start()
  {
    //goes through all the given GameObjects and records their original scales in an array
    //also determines their individual minimum scales by way of the final scale fraction given
    originalScales = new float[targets.Capacity];
    minScales = new float[targets.Capacity];
    foreach (GameObject target in targets)
    {
      originalScales[targets.LastIndexOf(target)] = target.transform.localScale.x;
      minScales[targets.LastIndexOf(target)] = target.transform.localScale.x * finalFraction;
    }
  }

  //Linecasts vertically from given head transform to 0. 
  //This will (hopefully) work so long as the floor collider remains above 0 on the Y-axis. 
  //float getCurrentHeight()
  //{
    //RaycastHit rc;
    //if (Physics.Linecast(userHeadTransform, new Vector3(userHeadTransform.position.x, 0, userHeadTransform.position.z), out rc))
    //{
      //return rc.distance;
    //s}
  //}

  //Am I hosting this object? This will return true, if the index of the connected
  //ViveControllerReceiver matches the build index.
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
      return this.name + "-shrinker";
    }
  }

  public override void ResetData()
  {
    data = new Holojam.Network.Flake(0, 0, 0, 2);
  }

  protected override void Sync()
  {
    if (Host)
    {
      //I am hosting this object. What do I do?
     // data.ints[0] = scale;
      data.ints[1] = shrinking;
    }
    else
    {
      //I am not hosting this object. What do I do?
      //scale = data.ints[0];
      shrinking = data.ints[1];
    }
    if (shrinking == 1)
    {
      foreach (GameObject target in targets)
      {
        float originalScale = originalScales[targets.LastIndexOf(target)];
        //Transform tr = target.GetComponent<Transform>();
      }
    }
  }



  //toggle "shrinking" - sets current height to max height when "shrinking" begins
  void IGlobalTouchpadPressDownHandler.OnGlobalTouchpadPressDown(VREventData eventData)
  {
    if (Host)
    {
      if (shrinking == 0)
      {
        //maxHeight = getCurrentHeight();
        shrinking = 1;
        minHeight = maxHeight * finalFraction;
        currentFraction = maxHeight / minHeight;
      }
      else
      {
        shrinking = 0;
      }
    }
  }
}
