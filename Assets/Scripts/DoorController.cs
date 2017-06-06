using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Holojam.Tools.Synchronizable {

    public TriggerSubscriber DoorSubscriber;
    public float InteractionLockOut;

    private Animator _Animator;
    private float _RemainingLockout;

    private bool _Open = false;
    private bool _Toggled = false;

    public int Index;

    private bool _Active;

    public override bool AutoHost { get { return false; } }

    public override bool Host {
        get {
            return Index == Holojam.Tools.BuildManager.BUILD_INDEX;
        }
    }

    public override string Label {
        get {
            return this.name + "-door";
        }
    }

    public override void ResetData() {
        data = new Holojam.Network.Flake(0, 0, 0, 2);
    }

    private void Start() {
        _Animator = GetComponent<Animator>();
    }

    protected override void Update() {
        base.Update();

        _RemainingLockout -= Time.deltaTime;
    }

    protected override void Sync() {
        if (Host) {
            data.ints[0] = 1;
            if (_Open) {
                data.ints[1] = 1;
            }
            else {
                data.ints[1] = 0;
            }
        }
        else {
            if (data.ints[0] == 1) {
                if (data.ints[1] == 1 && !_Open) {
                    _Open = true;
                    _Animator.SetTrigger("Open");
                }
                else if (data.ints[1] == 0 && _Open) {
                    _Open = false;
                    _Animator.SetTrigger("Close");
                }
            }
        }
    }

    public void Toggle() {
        if (_RemainingLockout > 0f) {
            return;
        }
        _Open = !_Open;
        if (_Open) {
            _Animator.SetTrigger("Open");
        }
        else {
            _Animator.SetTrigger("Close");
        }
        _RemainingLockout = InteractionLockOut;
    }

    void OnDoorColliderEnter(Collider other) {
        //if (_RemainingLockout > 0f) {
        //    return;
        //}
        //if (!_Closed) {
        //    _Animator.SetTrigger("Close");
        //}
        //else {
        //    _Animator.SetTrigger("Open");
        //}
        //_Closed = !_Closed;
        //_RemainingLockout = InteractionLockOut;
    }
}
