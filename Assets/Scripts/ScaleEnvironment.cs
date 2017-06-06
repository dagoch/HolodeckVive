using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;

public class ScaleEnvironment : Synchronizable, IGlobalTouchpadPressDownHandler, IGlobalTouchpadPressUpHandler {

    public Transform HeadTransform;
    public Transform EnvironmentTransform;
    public Transform RelativeTransform;
    public float PercentPerUnit;
    public float UpperLimit;

    private Vector3 _DefaultScale;
    private float _DefaultMagnitude;
    private Vector3 _DefaultPosition;
    private float[] _DefaultLightRanges = new float[0];
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
        CalibrateScale();
    }

    public override void ResetData() {
        data = new Holojam.Network.Flake(2, 0, _DefaultLightRanges.Length, 1);
    }

    public void CalibrateScale() {
        _DefaultScale = EnvironmentTransform.localScale;
        _DefaultMagnitude = _DefaultScale.magnitude;
        _DefaultPosition = EnvironmentTransform.position;
        var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
        _DefaultLightRanges = new float[lights.Length];
        for (var i = 0; i < lights.Length; i++) {
            _DefaultLightRanges[i] = lights[i].range;
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
                var magnitude = scale.magnitude;
                scale.Normalize();
                scale *= Mathf.Clamp(magnitude, _DefaultMagnitude, _DefaultMagnitude * UpperLimit);
                //scale = Vector3.ClampMagnitude(scale, _DefaultMagnitude * UpperLimit);
                EnvironmentTransform.localScale = scale;
                data.vector3s[0] = EnvironmentTransform.localScale;

                var percentDiff = scale.x / _DefaultScale.x;
                //percentDiff = Mathf.Clamp(percentDiff, 1f, UpperLimit);

                //scale = Vector3.Lerp(Vector3.zero, _DefaultScale, percentDiff);
                EnvironmentTransform.localScale = scale;
                data.vector3s[0] = EnvironmentTransform.localScale;

                var relativePosition = RelativeTransform.position;
                relativePosition.y = 0;
                var position = Vector3.LerpUnclamped(relativePosition, _DefaultPosition, percentDiff);
                EnvironmentTransform.position = position;
                data.vector3s[1] = EnvironmentTransform.position;

                for (var i = 0; i < lights.Length; i++) {
                    var defaultRange = _DefaultLightRanges[i];
                    var range = _StartLightRanges[i] + defaultRange * diff * PercentPerUnit;
                    range = Mathf.Clamp(range, defaultRange, defaultRange * UpperLimit);
                    lights[i].range = _StartLightRanges[i] + range * diff * PercentPerUnit;
                }
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
                EnvironmentTransform.position = data.vector3s[1];
                var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
                for (var i = 0; i < lights.Length; i++) {
                    lights[i].range = data.floats[i];
                }
            }
        }
    }

    void IGlobalTouchpadPressDownHandler.OnGlobalTouchpadPressDown(VREventData eventData) {
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
            ResetData();
        }
    }

    void IGlobalTouchpadPressUpHandler.OnGlobalTouchpadPressUp(VREventData eventData) {
        if (Host) {
            _Scaling = false;
        }
    }
}
