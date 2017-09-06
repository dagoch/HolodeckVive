using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Vive;
using Holojam.Tools;
using System;

public class ForceGrabTransform : Synchronizable, IGlobalTriggerPressDownHandler, IGlobalTriggerPressUpHandler {

    public Transform TargetTransform;

    private Transform _SavedParent;
    private bool _TriggerDown = false;
    private bool _HoldingTransform = false;
    private ViveControllerReceiver _VCReceiver;

    private float _DropPostDuration;
    private float _DropPostTime;

    public override bool Host {
        get {
            if (!_VCReceiver) _VCReceiver = GetComponent<ViveControllerReceiver>();
            return _VCReceiver.index == Holojam.Tools.BuildManager.BUILD_INDEX;
        }
    }

    public override bool AutoHost { get { return false; } }

    public override string Label {
        get {
            return this.name + "-forceGrab";
        }
    }

	// Use this for initialization
	void Start () {
		
	}

    public override void ResetData() {
        data = new Holojam.Network.Flake(2, 0, 0, 1);
    }

    protected override void Sync() {
        if (Host) {
            if (_TriggerDown) {
                //data.ints[0] = 1;
                //data.vector3s[0] = TargetTransform.localPosition;
                //data.vector3s[1] = TargetTransform.localRotation.eulerAngles;
                if (!_HoldingTransform) {
                    _SavedParent = TargetTransform.parent;
                    TargetTransform.parent = transform;
                    _HoldingTransform = true;
                }
                data.ints[0] = 1;
                data.vector3s[0] = TargetTransform.localPosition;
                data.vector3s[1] = TargetTransform.localRotation.eulerAngles;
            }
            else if (_HoldingTransform) {
                _DropPostTime = _DropPostDuration;
                TargetTransform.parent = _SavedParent;
                _HoldingTransform = false;
            }

            if (_DropPostTime > 0f) {
                _DropPostTime -= Time.deltaTime;
                data.ints[0] = 2;
            }
        }
        else {
            if (data.ints[0] == 1) {
                if (!_HoldingTransform) {
                    _SavedParent = TargetTransform.parent;
                    TargetTransform.parent = transform;
                    TargetTransform.localPosition = data.vector3s[0];
                    TargetTransform.localRotation = Quaternion.Euler(data.vector3s[1]);
                    _HoldingTransform = true;
                }
            }
            else if (data.ints[0] == 2) {
                TargetTransform.parent = _SavedParent;
                _HoldingTransform = false;
            }
        }
    }

    void IGlobalTriggerPressDownHandler.OnGlobalTriggerPressDown(VREventData eventData) {
        if (Host) {
            _TriggerDown = true;
        }
    }

    void IGlobalTriggerPressUpHandler.OnGlobalTriggerPressUp(VREventData eventData) {
        if (Host) {
            _TriggerDown = false;
        }
    }

}
