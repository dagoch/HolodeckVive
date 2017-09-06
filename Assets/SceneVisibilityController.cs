using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holojam.Network;
using UnityEngine.SceneManagement;

public class SceneVisibilityController : MonoBehaviour {

  //GameObject target needs to have a mesh renderer component attached for this to work
  public List<GameObject> actorBodyParts = new List<GameObject>();

  Transform environmentTransform;
  List<GameObject> sceneObjects = new List<GameObject>();
  AppearAndDisappear_View[] visibilityScriptsV;
  AppearAndDisappear_Rotation[] visibilityScriptsR;

  Vector3 _startPosition = new Vector3();
  Vector3 _startScale = new Vector3();

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
    environmentTransform = transform;
    _startPosition = environmentTransform.position;
    _startScale = environmentTransform.localScale;
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  // Add this code to your client

  void OnEnable()
  {
    Notifier.AddSubscriber(Trigger, "trigger");
    Notifier.AddSubscriber(Reset, "reset");
    Notifier.AddSubscriber(ActivateActors, "activate");
    Notifier.AddSubscriber(DeactivateActors, "deactivate");
  }

  void OnDisable()
  {
    Notifier.RemoveSubscriber(Trigger, "trigger");
    Notifier.RemoveSubscriber(Reset, "reset");
    Notifier.RemoveSubscriber(ActivateActors, "activate");
    Notifier.RemoveSubscriber(DeactivateActors, "deactivate");
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
    SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    //// React to reset
    //foreach (AppearAndDisappear_View scr in visibilityScriptsV)
    //{
    //  scr.reset();
    //}
    //foreach (AppearAndDisappear_Rotation scr in visibilityScriptsR)
    //{
    //  scr.reset();
    //}
    //ResetScaling();
  }

  void ResetScaling()
  {
    environmentTransform.position = _startPosition;

    environmentTransform.localScale = _startScale;
  }

  void ActivateActors(string source, string scope, Flake data)
  {
    Debug.Log("Activate actors");
    foreach (GameObject target in actorBodyParts)
    {
      target.SetActive(true);
    }
  }

  void DeactivateActors(string source, string scope, Flake data)
  {
    Debug.Log("Deactivate actors");
    foreach (GameObject target in actorBodyParts)
    {
      target.SetActive(false);
    }
  }
}
