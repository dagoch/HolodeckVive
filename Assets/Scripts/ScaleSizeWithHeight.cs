using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;

public class ScaleSizeWithHeight : Synchronizable, IGlobalTouchpadPressDownHandler, IGlobalTouchpadPressUpHandler {

    public Transform HeadTransform;
    public Transform[] TransformsToScale;
    public float PercentPerUnit;

    private Vector3[] _DefaultScales;
    private bool _Scaling;
    private Vector3[] _StartScales;
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
            return this.name + "-heightScaler";
        }
    }

    private void Start() {
        RefreshTransforms();
    }

    public void RefreshTransforms() {
        _DefaultScales = new Vector3[TransformsToScale.Length];
        for (var i = 0; i < TransformsToScale.Length; i++) {
            _DefaultScales[i] = TransformsToScale[i].localScale;
        }
    }

    public override void ResetData() {
        data = new Holojam.Network.Flake(TransformsToScale.Length, 0, 0, 0);
    }

    protected override void Sync() {
        if (Host) {
            if (_Scaling) {
                //var diff = HeadTransform.position.y - _LastY;
                var diff = HeadTransform.position.y - _StartY;
                for (var i = 0; i < TransformsToScale.Length; i++) {
                    //var scale = TransformsToScale[i].localScale;
                    var scale = _StartScales[i] + (_DefaultScales[i] * diff * PercentPerUnit);
                    TransformsToScale[i].localScale = scale;
                }
                //_LastY = HeadTransform.position.y;
            }
            for (var i = 0; i < TransformsToScale.Length; i++) {
                data.vector3s[i] = TransformsToScale[i].localScale;
            }
        }
        else {
            for (var i = 0; i < TransformsToScale.Length; i++) {
                TransformsToScale[i].localScale = data.vector3s[i];
            }
        }
    }

    void IGlobalTouchpadPressDownHandler.OnGlobalTouchpadPressDown(VREventData eventData) {
        if (Host) {
            _Scaling = true;
            //_LastY = HeadTransform.position.y;
            _StartY = HeadTransform.position.y;
            _StartScales = new Vector3[TransformsToScale.Length];
            for (var i = 0; i < TransformsToScale.Length; i++) {
                _StartScales[i] = TransformsToScale[i].localScale;
            }
        }
    }

    void IGlobalTouchpadPressUpHandler.OnGlobalTouchpadPressUp(VREventData eventData) {
        if (Host) {
            _Scaling = false;
        }
    }
}
