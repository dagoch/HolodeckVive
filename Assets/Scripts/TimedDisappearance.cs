using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDisappearance : MonoBehaviour {
  //seconds after scene is loaded at which objects will appear
  public float disappearTime;
  //increase this to make the area around the user's viewport larger - increase if objects are disappearing in view
  public float viewportBuffer;
  private float timer = 0.0f;
  public Camera cr;
  bool invisible;

  // Update is called once per frame
  void Update()
  {
    timer += Time.deltaTime;
    if (disappearTime < timer)
    {
      //Vector3 screenPoint = cr.WorldToViewportPoint(transform.position);
      //bool onScreen = screenPoint.z > 0 - viewportBuffer && screenPoint.x > 0 - viewportBuffer && screenPoint.x < 1 + viewportBuffer && screenPoint.y > 0 - viewportBuffer && screenPoint.y < 1 + viewportBuffer;
      if (invisible)
      {
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

  void OnBecameInvisible()
  {
    invisible = true;
  }
  void OnBecameVisible()
  {
    invisible = false;
  }
}
