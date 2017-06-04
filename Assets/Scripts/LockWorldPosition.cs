using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockWorldPosition : MonoBehaviour {

    private Vector3 _Position;

	// Use this for initialization
	void Start () {
        _Position = transform.position;	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = _Position;	
	}
}
