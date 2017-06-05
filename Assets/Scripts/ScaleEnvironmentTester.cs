using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEnvironmentTester : MonoBehaviour {

    public Transform HeadTransform;
    public Transform EnvironmentTransform;
    public Transform RelativeTransform;
    public float PercentPerUnit;

    private Vector3 _DefaultScale;
    private Vector3 _DefaultPosition;
    private float[] _DefaultLightRanges;
    private bool _TriggerDown;
    private Vector3 _StartScale;
    private float[] _StartLightRanges;
    private float _StartY;
    private float _LastY;

	// Use this for initialization
	void Start () {
        _DefaultScale = EnvironmentTransform.localScale;
        _DefaultPosition = EnvironmentTransform.position;
        var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
        _DefaultLightRanges = new float[lights.Length];
        for (var i = 0; i < lights.Length; i++) {
            _DefaultLightRanges[i] = lights[i].range;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.B) && !_TriggerDown) {
            _TriggerDown = true;
            _LastY = HeadTransform.position.y;
            _StartY = HeadTransform.position.y;
            _StartScale = EnvironmentTransform.localScale;
            var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
            _StartLightRanges = new float[lights.Length];
            for (var i = 0; i < lights.Length; i++) {
                _StartLightRanges[i] = lights[i].range;
            }
        }
        else if (Input.GetKeyUp(KeyCode.B) && _TriggerDown) {
            _TriggerDown = false;
        }

        if (_TriggerDown) {
            //var diff = HeadTransform.position.y - _LastY;
            var diff = HeadTransform.position.y - _StartY;
            //var scale = EnvironmentTransform.localScale;
            var scale = _StartScale + (_DefaultScale * diff * PercentPerUnit);
            EnvironmentTransform.localScale = scale;

            var percentDiff = scale.x / _DefaultScale.x;
            var relativePosition = RelativeTransform.position;
            relativePosition.y = 0f;
            var position = Vector3.LerpUnclamped(relativePosition, _DefaultPosition, percentDiff);
            EnvironmentTransform.position = position;

            var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
            for (var i = 0; i < lights.Length; i++) {
                var range = _DefaultLightRanges[i];
                lights[i].range = _StartLightRanges[i] + range * diff * PercentPerUnit;
            }
            _LastY = HeadTransform.position.y;
        }		
	}
}
