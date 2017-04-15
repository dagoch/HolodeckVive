using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;

public class HandToggler : Synchronizable, IGlobalGripPressDownHandler {

  public List<GameObject> hands = new List<GameObject>();
  private int activeIndex = 0;

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
      return this.name + "-hand";
    }
  }

  public override void ResetData() {
    data = new Holojam.Network.Flake(0, 0, 0, 1);
  }

  protected override void Sync() {
    if (Host) {
      //I am hosting this object. What do I do?
      data.ints[0] = activeIndex;
    } else {
      //I am not hosting this object. What do I do?
      activeIndex = data.ints[0];
    }

    for (int i = 0; i < hands.Count; i++) {
      hands[i].SetActive(i == activeIndex);
    }
  }


  public void Toggle() {
    activeIndex = (activeIndex + 1) % hands.Count;
  }

  #region IGlobalGripPressDownHandler implementation

  void IGlobalGripPressDownHandler.OnGlobalGripPressDown(VREventData eventData) {
    if (Host) {
      Toggle();
    }
  }


  #endregion
}
