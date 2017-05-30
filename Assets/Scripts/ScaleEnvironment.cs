using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEnvironment : MonoBehaviour {

    public Transform HolojamTransform;
    public Transform EnvironmentTransform;
    public float ScaleFactor;
    public Transform TestTransform;

    private float _StartY;
    private bool _TriggerDown;
    private Vector3 _StartScale;
    private float[] _SavedLightRanges;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.B) && !_TriggerDown) {
            _TriggerDown = true;
            _StartY = transform.position.y;
            _StartScale = EnvironmentTransform.localScale;
            var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
            _SavedLightRanges = new float[lights.Length];
            for (var i = 0; i < lights.Length; i++) {
                _SavedLightRanges[i] = lights[i].range;
            }
        }	
        if (Input.GetKeyUp(KeyCode.B) && _TriggerDown) {
            _TriggerDown = false;
        }

        if (_TriggerDown) {
            var posDiff = EnvironmentTransform.position - HolojamTransform.position;
            Debug.Log(posDiff);
            var diff = transform.position.y - _StartY;
            var scale = _StartScale * (1 + diff * ScaleFactor);
            EnvironmentTransform.localScale = scale;
            var lights = EnvironmentTransform.GetComponentsInChildren<Light>();
            for (var i = 0; i < lights.Length; i++) {
                lights[i].range = _SavedLightRanges[i] * (1 + diff * ScaleFactor);
            }
        }
	}
}
