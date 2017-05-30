using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;

public class ScaleSizeWithHeight : Synchronizable, IGlobalTriggerPressDownHandler, IGlobalTriggerPressUpHandler {

    public Transform HeadTransform;
    public Transform LeftHandTransform;
    public Transform RightHandTransform;

    [Range(0, 1)]
    public float LowerLimit;

    private float _StartY;
    private bool _TriggerDown;

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
            return this.name + "-heightScaler";
        }
    }

    public override void ResetData() {
        data = new Holojam.Network.Flake(1, 0, 0, 0);
    }

    protected override void Sync() {
        if (Host) {
            if (_TriggerDown) {
                var diff = HeadTransform.position.y - _StartY;
                var percent = Mathf.Clamp(HeadTransform.position.y / _StartY, LowerLimit, 1f);
                HeadTransform.localScale = Vector3.one * percent;
                Debug.Log("sending data and stuff!!!");
            }
            data.vector3s[0] = HeadTransform.localScale;
            //data.ints[0] = (_TriggerDown) ? 1 : 0;
        }
        else {
            HeadTransform.localScale = data.vector3s[0];
            //_TriggerDown = (data.ints[0] == 1) ? true : false;
        }
    }

    void IGlobalTriggerPressDownHandler.OnGlobalTriggerPressDown(VREventData eventData) {
        Debug.Log("trigger is down");
        if (Host) {
            _TriggerDown = true;
            _StartY = HeadTransform.position.y;
        }
    }

    void IGlobalTriggerPressUpHandler.OnGlobalTriggerPressUp(VREventData eventData) {
        if (Host) {
            _TriggerDown = false;
        }
    }
}
