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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Space) && !_TriggerDown) {
        //    _TriggerDown = true;
        //    _StartY = transform.position.y;
        //}
        //else if (Input.GetKeyUp(KeyCode.Space) && _TriggerDown) {
        //    _TriggerDown = false;
        //}

        //if (_TriggerDown) {
        //    var percent = Mathf.Clamp(HeadTransform.position.y / _StartY, LowerLimit, 1f);
        //    HeadTransform.localScale = Vector3.one * percent;
        //}
	}

    public override void ResetData() {
        data = new Holojam.Network.Flake(1, 0, 0, 0);
    }

    protected override void Sync() {
        if (Host) {
            if (_TriggerDown) {
                var percent = Mathf.Clamp(HeadTransform.position.y / _StartY, LowerLimit, 1f);
                HeadTransform.localScale = Vector3.one * percent;
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
