using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour {

    public float Speed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var position = transform.position;
        if (Input.GetKey(KeyCode.W)) {
            position.y += Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            position.y -= Speed * Time.deltaTime;
        }
        transform.position = position;
	}
}
