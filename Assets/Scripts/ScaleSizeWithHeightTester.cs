using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSizeWithHeightTester : MonoBehaviour {

    public Transform HeadTransform;
    public Transform[] TransformsToScale;
    public float PercentPerUnit;

    private Vector3[] _DefaultScales;
    private bool _TriggerDown;
    private float _LastY;

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
        }	
        else if (Input.GetKeyUp(KeyCode.Space) && _TriggerDown) {
            _TriggerDown = false;
        }

        if (_TriggerDown) {
            var diff = HeadTransform.position.y - _LastY;
            for (var i = 0; i < TransformsToScale.Length; i++) {
                var scale = TransformsToScale[i].localScale;
                scale += (_DefaultScales[i] * diff * PercentPerUnit);
                TransformsToScale[i].localScale = scale;
            }
            _LastY = HeadTransform.position.y;
        }
	}
}
