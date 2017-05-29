using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;


public class SizeToggler : Synchronizable, IGlobalTouchpadPressDownHandler, IGlobalTouchpadPressUpHandler
{

  //GameObjects whose scale will be changed 
  public List<GameObject> targets = new List<GameObject>();
  //Public modifier which determines how much the scale will change each synchronization
  public float scaleChangeModifier = 0.1f;
  public float minScale = 0.0f;
  public float maxScale;

  int scale = 1;
  bool pressing = false;

  //Most of the following pasted from HandToggler.
  private ViveControllerReceiver vcreceiver;

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
      return this.name + "-resizer";
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
      data.ints[0] = scale;
    }
    else
    {
      //I am not hosting this object. What do I do?
      scale = data.ints[0];
    }
    if (pressing)
    {
      foreach (GameObject target in targets)
      {
        Transform tr = target.GetComponent<Transform>();
        if (tr.localScale.x >= minScale && tr.localScale.x <= maxScale)
        {
          tr.localScale += new Vector3(scaleChangeModifier * scale, scaleChangeModifier * scale, scaleChangeModifier * scale);
        }
        else
        {
          if (tr.localScale.x < minScale)
          {
            tr.localScale = new Vector3(minScale, minScale, minScale);
          }
          else
          {
            tr.localScale = new Vector3(maxScale, maxScale, maxScale);
          }
        }
      }
    }
  }


  

  void IGlobalTouchpadPressDownHandler.OnGlobalTouchpadPressDown(VREventData eventData)
  {
    if (Host)
    {
      pressing = true;
      if (eventData.touchpadAxis.y > 0)
      {
        scale = 1;
      }
      else
      {
        scale = -1;
      }
    }
  }

  void IGlobalTouchpadPressUpHandler.OnGlobalTouchpadPressUp(VREventData eventData)
  {
    if (Host)
    {
      pressing = false;
    }
  }
}
