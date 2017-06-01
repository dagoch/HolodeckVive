using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedAppearance : MonoBehaviour {

  public float appearTime;
  //increase this to make the area around the user's viewport larger - increase if objects are popping into view
  public float viewportBuffer;
  private float timer = 0.0f;
  public Camera cr;
  // Use this for initialization
	void Start ()
  {
    foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
    { 
      mr.enabled = false;
    }
    if (GetComponent<MeshRenderer>())
    {
      GetComponent<MeshRenderer>().enabled = false;
    }
  }
	
	// Update is called once per frame
	void Update () {
    timer += Time.deltaTime;
    if (appearTime < timer)
    {
      Vector3 screenPoint = cr.WorldToViewportPoint(transform.position);
      bool onScreen = screenPoint.z > 0-viewportBuffer && screenPoint.x > 0-viewportBuffer && screenPoint.x < 1+viewportBuffer && screenPoint.y > 0-viewportBuffer && screenPoint.y < 1+viewportBuffer;
      if (!onScreen)
      {
        if (GetComponent<MeshRenderer>())
        {
          GetComponent<MeshRenderer>().enabled = true;
        }
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
          mr.enabled = true;
        }
        this.enabled = false;
      }
    }
	}
}
