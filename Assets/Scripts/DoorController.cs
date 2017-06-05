using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public TriggerSubscriber DoorSubscriber;
    public float InteractionLockOut;

    private Animator _Animator;
    private float _RemainingLockout;

    private bool _Closed = true;

	void Start () {
        _Animator = GetComponent<Animator>();
        DoorSubscriber.SubscribeToEnter(OnDoorColliderEnter);
	}
	
	void Update () {
        _RemainingLockout -= Time.deltaTime;	
	}

    void OnDoorColliderEnter(Collider other) {
        if (_RemainingLockout > 0f) {
            return;
        }
        if (!_Closed) {
            _Animator.SetTrigger("Close");
        }
        else {
            _Animator.SetTrigger("Open");
        }
        _Closed = !_Closed;
        _RemainingLockout = InteractionLockOut;
    }
}
