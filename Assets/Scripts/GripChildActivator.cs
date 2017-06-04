using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;

public class GripChildActivator : Synchronizable, IGlobalGripPressDownHandler, IGlobalGripPressUpHandler {

    private bool _Activate = false;

    private ViveControllerReceiver _VCReceiver;

    public override bool Host {
        get {
            if (!_VCReceiver) _VCReceiver = GetComponent<ViveControllerReceiver>();
            return _VCReceiver.index == Holojam.Tools.BuildManager.BUILD_INDEX;
        }
    }

    public override bool AutoHost { get { return false; } }

    public override string Label {
        get {
            return this.name + "-gripChildActivator";
        }
    }

    public override void ResetData() {
        data = new Holojam.Network.Flake(0, 0, 0, 1);
    }

    protected override void Sync() {
        if (Host) {
            data.ints[0] = (_Activate) ? 1 : 0;
        }
        else {
            _Activate = (data.ints[0] == 1) ? true : false;
        }
        foreach (var activator in GetComponentsInChildren<IActivateReceiver>()) {
            if (_Activate) {
                activator.Activate();
            }
            else {
                activator.Deactivate();
            }
        }
    }

    void IGlobalGripPressDownHandler.OnGlobalGripPressDown(VREventData eventData) {
        if (Host) {
            _Activate = true;
        }
    }

    void IGlobalGripPressUpHandler.OnGlobalGripPressUp(VREventData eventData) {
        if (Host) {
            _Activate = false;
        }
    }

}
