using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;

public class HandToggler : MonoBehaviour, IGlobalGripPressDownHandler {

  public List<GameObject> hands = new List<GameObject>();
  private int activeIndex = -1;

  void Awake() {
    Toggle();
  }

  public void Toggle() {
    activeIndex = (activeIndex + 1) % hands.Count;

    for (int i = 0; i < hands.Count; i++) {
      hands[i].SetActive(i == activeIndex);
    }
  }

  #region IGlobalGripPressDownHandler implementation

  void IGlobalGripPressDownHandler.OnGlobalGripPressDown(VREventData eventData) {
    Toggle();
  }

  #endregion
}
