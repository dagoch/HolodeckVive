using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSizeWithHeight : MonoBehaviour {

    [Range(0, 1)]
    public float LowerLimit;

    private float _StartY;
    private bool _TriggerDown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && !_TriggerDown) {
            _TriggerDown = true;
            _StartY = transform.position.y;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && _TriggerDown) {
            _TriggerDown = false;
        }

        if (_TriggerDown) {
            var percent = Mathf.Clamp(transform.position.y / _StartY, LowerLimit, 1f);
            transform.localScale = Vector3.one * percent;
        }
	}
}
