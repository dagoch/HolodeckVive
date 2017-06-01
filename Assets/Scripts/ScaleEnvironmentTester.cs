using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEnvironmentTester : MonoBehaviour {

    public Transform HeadTransform;
    public Transform EnvironmentTransform;
    public float PercentPerUnit;

    private Vector3 _DefaultScale;
    private bool _TriggerDown;
    private float[] _SavedLightRanges;
    private float _LastY;

	// Use this for initialization
	void Start () {
        _DefaultScale = EnvironmentTransform.localScale;	
        var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
        _SavedLightRanges = new float[lights.Length];
        for (var i = 0; i < lights.Length; i++) {
            _SavedLightRanges[i] = lights[i].range;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.B) && !_TriggerDown) {
            _TriggerDown = true;
            _LastY = HeadTransform.position.y;
        }
        else if (Input.GetKeyUp(KeyCode.B) && _TriggerDown) {
            _TriggerDown = false;
        }

        if (_TriggerDown) {
            var diff = HeadTransform.position.y - _LastY;
            var scale = EnvironmentTransform.localScale;
            scale += (_DefaultScale * diff * PercentPerUnit);
            EnvironmentTransform.localScale = scale;
            var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
            for (var i = 0; i < lights.Length; i++) {
                var range = _SavedLightRanges[i];
                lights[i].range += range * diff * PercentPerUnit;
            }
            _LastY = HeadTransform.position.y;
        }		
	}
}
