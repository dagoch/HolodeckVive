using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public TriggerSubscriber OpenSubscriber;
    public TriggerSubscriber CloseSubscriber;
    public float InteractionLockOut;

    private Animator _Animator;
    private float _RemainingLockout;

	void Start () {
        _Animator = GetComponent<Animator>();

        OpenSubscriber.SubscribeToEnter(OnOpenEnter);
        CloseSubscriber.SubscribeToEnter(OnCloseEnter);
	}
	
	void Update () {
        _RemainingLockout -= Time.deltaTime;	
	}

    void OnOpenEnter(Collider other) {
        if (_RemainingLockout > 0f) {
            return;
        }
        _RemainingLockout = InteractionLockOut;
        _Animator.SetTrigger("Open");
    }

    void OnCloseEnter(Collider other) {
        if (_RemainingLockout > 0f) {
            return;
        }
        _RemainingLockout = InteractionLockOut;
        _Animator.SetTrigger("Close");
    }
}
