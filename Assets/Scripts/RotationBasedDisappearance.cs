using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBasedDisappearance : MonoBehaviour {
  //seconds after scene is loaded at which objects will appear
  public float disappearTime;
  //x-rotation of the user's head which will trigger disappearance of object
  public float rotation;
  public Camera cr;

  float timer;

  void Update()
  {
    timer += Time.deltaTime;
    if (disappearTime < timer)
    {
      float currentRotation = cr.transform.rotation.eulerAngles.x;
      if (currentRotation > rotation && currentRotation < 180)
      {
        Debug.Log("DING2");
        if (GetComponent<MeshRenderer>())
        {
          GetComponent<MeshRenderer>().enabled = false;
        }
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
          mr.enabled = false;
        }
        this.enabled = false;
      }
    }
  }
}
