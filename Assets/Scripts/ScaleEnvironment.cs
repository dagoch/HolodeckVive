using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;

public class ScaleEnvironment : Synchronizable, IGlobalTriggerPressDownHandler, IGlobalTriggerPressUpHandler {

    public Transform HeadTransform;
    public Transform HolojamTransform;
    public Transform EnvironmentTransform;
    public float ScaleFactor;

    private float _StartY;
    private bool _TriggerDown;
    private Vector3 _StartScale;
    private float[] _SavedLightRanges;

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
            return this.name + "-environmentScaler";
        }
    }

    public override void ResetData() {
        data = new Holojam.Network.Flake(1, 0, 0, 0);
    }

    protected override void Sync() {
        if (Host) {
            if (_TriggerDown) {
                var diff = HeadTransform.position.y - _StartY;
                Debug.Log("diff is: " + diff);
                var scale = _StartScale * (1 + diff * ScaleFactor);
                EnvironmentTransform.localScale = scale;
                Debug.Log("start: " + _StartScale + " local: " + scale);
                //EnvironmentTransform.localScale = _StartScale;
            }
            data.vector3s[0] = EnvironmentTransform.localScale;
        }
        else {
            EnvironmentTransform.localScale = data.vector3s[0];
        }
    }

    void IGlobalTriggerPressDownHandler.OnGlobalTriggerPressDown(VREventData eventData) {
        if (Host) {
            _TriggerDown = true;
            _StartY = HeadTransform.position.y;
            _StartScale = EnvironmentTransform.localScale;
            var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
            _SavedLightRanges = new float[lights.Length];
            for (var i = 0; i < lights.Length; i++) {
                _SavedLightRanges[i] = lights[i].range;
            }
        }
    }

    void IGlobalTriggerPressUpHandler.OnGlobalTriggerPressUp(VREventData eventData) {
        if (Host) {
            _TriggerDown = false;
        }
    }
}
