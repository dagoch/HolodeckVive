using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holojam.Network;

public class SceneVisibilityController : MonoBehaviour {

  List<GameObject> sceneObjects = new List<GameObject>();
  AppearAndDisappear_View[] visibilityScriptsV;
  AppearAndDisappear_Rotation[] visibilityScriptsR;

  // Use this for initialization
  void Start () {
    visibilityScriptsV = GameObject.FindObjectsOfType<AppearAndDisappear_View>();
    for (int i = 0; i < visibilityScriptsV.Length; i++ )
    {
      sceneObjects.Add(visibilityScriptsV[i].gameObject);

    }
    visibilityScriptsR = GameObject.FindObjectsOfType<AppearAndDisappear_Rotation>();
    for (int i = 0; i < visibilityScriptsR.Length; i++)
    {
      sceneObjects.Add(visibilityScriptsR[i].gameObject);

    }
    Debug.Log("Found " + sceneObjects.Count + "appearanddisappear scripts");
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  // Add this code to your client

  void OnEnable()
  {
    Notifier.AddSubscriber(Trigger, "trigger");
    Notifier.AddSubscriber(Reset, "reset");
  }

  void OnDisable()
  {
    Notifier.RemoveSubscriber(Trigger, "trigger");
    Notifier.RemoveSubscriber(Reset, "reset");
  }

  void Trigger(string source, string scope, Flake data)
  {
    Debug.Log("Trigger");
    // React to trigger
    foreach (AppearAndDisappear_View scr in visibilityScriptsV)
    {
      scr.StartTimerToggle();
    }
    foreach (AppearAndDisappear_Rotation scr in visibilityScriptsR)
    {
      scr.StartTimerToggle();
    }
  }

  void Reset(string source, string scope, Flake data)
  {
    Debug.Log("Reset");
    // React to reset
    foreach (AppearAndDisappear_View scr in visibilityScriptsV)
    {
      scr.reset();
    }
    foreach (AppearAndDisappear_Rotation scr in visibilityScriptsR)
    {
      scr.reset();
    }
  }

}
