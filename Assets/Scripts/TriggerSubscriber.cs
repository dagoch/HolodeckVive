using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerSubscriber : MonoBehaviour {

    private List<Action<Collider>> _EnterActions;

	// Use this for initialization
	void Start () {
        _EnterActions = new List<Action<Collider>>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SubscribeToEnter(Action<Collider> action) {
        _EnterActions.Add(action);
    }

    public void UnsubscribeFromEnter(Action<Collider> action) {
        _EnterActions.Remove(action);
    }

    private void OnTriggerEnter(Collider other) {
        foreach(var action in _EnterActions) {
            action(other);
        }
    }
}
