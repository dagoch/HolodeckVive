using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAndDisappear_Rotation : MonoBehaviour {
  //object will appear when the scene begins
  public bool appearOnStart;
  //if object is below the user - this is needed because of the way unity handles head rotation 
  //and the way I'm checking it
  public bool below;
  //seconds after scene is loaded at which objects will appear or disappear
  public float time;
  //angle distance up or down from straight forward (0 = straight forward)
  //  which will trigger appearance or disappearance of object.  Always positive degrees off axis.
  public float rotation;
  //user camera
  public Camera cr;

  bool seen = true;
  MeshRenderer[] meshRenderers;

  void Start()
  {
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
    //StartCoroutine(StartTimer(false));
  }

  // Call this to reset object to initial state
  public void reset()
  {
    switchState(appearOnStart);
  }

  // Immediately toggle visibility of object (e.g. for reset)
  //state = true will turn all associated meshrenderers on, and vice versa
  void switchState(bool state)
  {
    Debug.Log("aad_rotate switching to "+state);
    foreach (MeshRenderer mesh in meshRenderers)
    {
      mesh.enabled = state;
    }
  }

  public void StartTimerToggle()
  {
    StartCoroutine(StartTimer(!appearOnStart));
  }


  // Call this to start timer; when timer expires, start checking head tilt every frame
  //  and if head tilted past threshold in correct direction, change visibility based on 
  //  state flag
  //state = true will turn all associated meshrenderers on, and vice versa
  // to toggle from initial state, call this with !appearOnStart
  public IEnumerator StartTimer(bool state)
  {
    yield return new WaitForSeconds(time);
    seen = true;
    while (seen)
    {
      float currentRotation = cr.transform.rotation.eulerAngles.x;
      //if we're below the user, we check the opposite angle (e.g. 330 degrees becomes 30, and vice versa)
      if (below)
      {
        currentRotation = 360.0f - currentRotation;
      }
      //Debug.Log(currentRotation + " " + rotation);
      if (currentRotation > rotation && currentRotation < 180)
      {
        switchState(state);
        seen = false;
      }
      yield return null;
    }
  }
}
