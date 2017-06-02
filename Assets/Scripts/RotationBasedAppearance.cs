using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBasedAppearance : MonoBehaviour
{
  //seconds after scene is loaded at which objects will appear
  public float appearTime;
  //x-rotation of the user's head which will trigger appearance of object
  public float rotation;
  public Camera cr;
  //the terrain to be disabled. this is a hacky solution because I wrote this for the terrain and it doesn't 
  //have a meshrenderer
  Terrain t;

  float timer;

  void Start()
  {
    t = GetComponent<Terrain>();
    t.enabled = false;
  }

  void Update()
  {
    timer += Time.deltaTime;
    if (appearTime < timer)
    {
      float currentRotation = cr.transform.rotation.eulerAngles.x;
      Debug.Log(currentRotation);
      if (currentRotation > rotation && currentRotation > 180)
      {
       t.enabled = true;
       this.enabled = false;
      }
    }
  }
}