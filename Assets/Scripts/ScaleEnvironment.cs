using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;

public class ScaleEnvironment : Synchronizable, IGlobalTriggerPressDownHandler, IGlobalTriggerPressUpHandler {

    public Transform HeadTransform;
    public Transform EnvironmentTransform;
    public float PercentPerUnit;

    private Vector3 _DefaultScale;
    private float[] _DefaultLightRanges;
    private bool _Scaling;
    private Vector3 _StartScale;
    private float[] _StartLightRanges = new float[0];
    private float _StartY;
    private float _LastY;

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

    void Start() {
        ResetScales();
    }

    public override void ResetData() {
        data = new Holojam.Network.Flake(1, 0, _StartLightRanges.Length, 1);
    }

    public void ResetScales() {
        _DefaultScale = EnvironmentTransform.localScale;	
        var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
        _StartLightRanges = new float[lights.Length];
        for (var i = 0; i < lights.Length; i++) {
            _StartLightRanges[i] = lights[i].range;
        }
    }

    protected override void Sync() {
        if (Host) {
            var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
            if (_Scaling) {
                // set the first int value to 1 to indicate you are updating scales
                data.ints[0] = 1;
                var diff = HeadTransform.position.y - _StartY;
                var scale = _StartScale + (_DefaultScale * diff * PercentPerUnit);
                EnvironmentTransform.localScale = scale;
                for (var i = 0; i < lights.Length; i++) {
                    var range = _DefaultLightRanges[i];
                    lights[i].range = _StartLightRanges[i] + range * diff * PercentPerUnit;
                }
                data.vector3s[0] = EnvironmentTransform.localScale;
                for (var i = 0; i < lights.Length; i++) {
                    data.floats[i] = lights[i].range;
                }
            }
            else {
                data.ints[0] = 0;
            }
        }
        else {
            // only change scales if the host script is changing them. This is to prevent
            // multiple different scaling scripts overriding each other
            if (data.ints[0] == 1) {
                EnvironmentTransform.localScale = data.vector3s[0];
                var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
                for (var i = 0; i < lights.Length; i++) {
                    lights[i].range = data.floats[i];
                }
            }
        }
    }

    void IGlobalTriggerPressDownHandler.OnGlobalTriggerPressDown(VREventData eventData) {
        if (Host) {
            _Scaling = true;
            _LastY = HeadTransform.position.y;
            _StartY = HeadTransform.position.y;
            _StartScale = EnvironmentTransform.localScale;
            var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
            _StartLightRanges = new float[lights.Length];
            for (var i = 0; i < lights.Length; i++) {
                _StartLightRanges[i] = lights[i].range;
            }
        }
    }

    void IGlobalTriggerPressUpHandler.OnGlobalTriggerPressUp(VREventData eventData) {
        if (Host) {
            _Scaling = false;
        }
    }
}
