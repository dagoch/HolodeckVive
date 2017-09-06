using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAndDisappear_View : MonoBehaviour {
  //object will appear when the scene begins
  public bool appearOnStart;
  //seconds after scene is loaded at which objects will appear or disappear
  public float time;
  //choose to use viewport checks or, if not, render checks to see if an object is "invisible"
  public bool checkViewport = true; // always this, except for making room pieces disappear
  //user camera
  public Camera userCamera;
  //if useViewport, this var will matter, else, it doesn't
  // how much additional distance outside viewport should object's center be before you consider it
  //  unseen.  (usually between 0 and 2.   viewport coordinates measure from 0 - 1 on each axis)
  public float viewportBuffer;
  MeshRenderer[] meshRenderers;

  bool seen = true;
  bool invisible;


  void Start()
  {
        appearOnStart = true;
    List<MeshRenderer> mrs = new List<MeshRenderer>();
    MeshRenderer mr = GetComponent<MeshRenderer>();
    if (mr)
    {
      mrs.Add(mr);
      mr.enabled = appearOnStart;
    }
    foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
    {
      mrs.Add(mesh);
      mesh.enabled = appearOnStart;
    }
    meshRenderers = mrs.ToArray();
    //debug
    //    StartCoroutine(StartTimer(true));
  }

  // Call this to reset object to initial state
  public void reset()
  {
    setState(appearOnStart);
  }

  // This immediately turns object on or off
  //state = true will turn all associated meshrenderers on, and vice versa
  void setState(bool state)
  {

    foreach (MeshRenderer mesh in meshRenderers)
    {
      mesh.enabled = state;
    }
  }

  // Note: these will never be called if the meshrenderer is off!
  void OnBecameInvisible()
  {
    Debug.Log("Now you don't.");
    invisible = true;
  }

  void OnBecameVisible()
  {
    Debug.Log("Now you see me.");
    invisible = false;
  }

  public void StartTimerToggle()
  {
    StartCoroutine(StartTimer(!appearOnStart));
  }

  // Call this to start timer; when timer expires, start checking if object is outside of 
  //  viewport (or is invisible to renderer), and if so, change visibility based on 
  //  state flag
  //state = true will turn all associated meshrenderers on, and vice versa
  public IEnumerator StartTimer(bool state)
  {
    yield return new WaitForSeconds(time);
    seen = true;
    while (seen)
    {
      if(checkViewport)
      {
        //checking a numerical graph of the camera viewport (x/y 0 to 1, + or - viewportBuffer) to see if the object's transform is there
        //this will often allow objects whose models go way beyond their transforms to pop into view
        Vector3 screenPoint = userCamera.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 - viewportBuffer && screenPoint.x > 0 - viewportBuffer && screenPoint.x < 1 + viewportBuffer && screenPoint.y > 0 - viewportBuffer && screenPoint.y < 1 + viewportBuffer;
        if (!onScreen)
        {
          seen = false;
          setState(state);
        }
      }
      else
      //this is only useful when making large objects disappear; won't ever work if it's already invisible
      {
        //invisible bool handled by separate OnBecameVisible/Invisible methods
        if (invisible)
        {
          seen = false;
          setState(state);
        }
      }
      yield return null;
    }
   
  }
}
