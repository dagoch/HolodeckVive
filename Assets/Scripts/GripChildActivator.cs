using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;

public class GripChildActivator : MonoBehaviour, IGlobalGripPressDownHandler, IGlobalGripPressUpHandler {

    private bool _Activate = false;

    private void Update() {
        if (_Activate) {
            foreach (var activator in GetComponentsInChildren<IActivateReceiver>()) {
                activator.Held();
            }
        }
    }

    void IGlobalGripPressDownHandler.OnGlobalGripPressDown(VREventData eventData) {
        _Activate = true;
    }

    void IGlobalGripPressUpHandler.OnGlobalGripPressUp(VREventData eventData) {
        _Activate = false;
    }

}
