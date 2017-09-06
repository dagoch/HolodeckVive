using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSizeWithHeightTester : MonoBehaviour {

    public Transform HeadTransform;
    public Transform[] TransformsToScale;
    public float PercentPerUnit;

    private Vector3[] _DefaultScales;
    private bool _TriggerDown;
    private Vector3[] _StartScales;
    private float _LastY;
    private float _StartY;

	// Use this for initialization
	void Start () {
        RefreshTransforms();
	}

    public void RefreshTransforms() {
        _DefaultScales = new Vector3[TransformsToScale.Length];
        for (var i = 0; i < TransformsToScale.Length; i++) {
            _DefaultScales[i] = TransformsToScale[i].localScale;
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space) && !_TriggerDown) {
            _TriggerDown = true;
            _LastY = HeadTransform.position.y;
            _StartY = HeadTransform.position.y;
            _StartScales = new Vector3[TransformsToScale.Length];
            for (var i = 0; i < TransformsToScale.Length; i++) {
                _StartScales[i] = TransformsToScale[i].localScale;
            }
        }	
        else if (Input.GetKeyUp(KeyCode.Space) && _TriggerDown) {
            _TriggerDown = false;
        }

        if (_TriggerDown) {
            //var diff = HeadTransform.position.y - _LastY;
            var diff = HeadTransform.position.y - _StartY;
            for (var i = 0; i < TransformsToScale.Length; i++) {
                //var scale = TransformsToScale[i].localScale;
                var scale = _StartScales[i] + (_DefaultScales[i] * diff * PercentPerUnit);
                TransformsToScale[i].localScale = scale;
            }
            _LastY = HeadTransform.position.y;
        }
	}
}
