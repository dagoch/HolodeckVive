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
    private bool _Scaling;
    private float[] _SavedLightRanges = new float[0];
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
        data = new Holojam.Network.Flake(1, 0, _SavedLightRanges.Length, 0);
    }

    public void ResetScales() {
        _DefaultScale = EnvironmentTransform.localScale;	
        var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
        _SavedLightRanges = new float[lights.Length];
        for (var i = 0; i < lights.Length; i++) {
            _SavedLightRanges[i] = lights[i].range;
        }
    }

    protected override void Sync() {
        if (Host) {
            if (_Scaling) {
                var diff = HeadTransform.position.y - _LastY;
                var scale = EnvironmentTransform.localScale;
                scale += (_DefaultScale * diff * PercentPerUnit);
                EnvironmentTransform.localScale = scale;
                var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
                for (var i = 0; i < lights.Length; i++) {
                    var range = _SavedLightRanges[i];
                    lights[i].range += range * diff * PercentPerUnit;
                    data.floats[i] = lights[i].range;
                }
                _LastY = HeadTransform.position.y;
            }
            data.vector3s[0] = EnvironmentTransform.localScale;
        }
        else {
            EnvironmentTransform.localScale = data.vector3s[0];
            var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
            for (var i = 0; i < lights.Length; i++) {
                lights[i].range = data.floats[i];
            }
        }
    }

    void IGlobalTriggerPressDownHandler.OnGlobalTriggerPressDown(VREventData eventData) {
        if (Host) {
            _Scaling = true;
            _LastY = HeadTransform.position.y;
        }
    }

    void IGlobalTriggerPressUpHandler.OnGlobalTriggerPressUp(VREventData eventData) {
        if (Host) {
            _Scaling = false;
        }
    }
}
