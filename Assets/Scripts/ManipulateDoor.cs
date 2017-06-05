using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using Holojam.Tools;
using Holojam.Vive;
using System;

public class ManipulateDoor : Synchronizable, IGlobalGripPressDownHandler {

    public DoorController DoorControl;

    private bool _OverlappingDoor;
    private bool _GripDown;

    private ViveControllerReceiver _VCReceiver;

    public override bool Host {
        get {
            if (!_VCReceiver) _VCReceiver = GetComponent<ViveControllerReceiver>();
            return _VCReceiver.index == Holojam.Tools.BuildManager.BUILD_INDEX;
        }
    }

    public override bool AutoHost { get { return false; } }

    public override string Label {
        get {
            return this.name + "-manipulateDoor";
        }
    }

	// Use this for initialization
	void Start () {
		
	}

    public override void ResetData() {
        data = new Holojam.Network.Flake(0, 0, 0, 1);
    }

    protected override void Sync() {
        if (Host) {
            if (_GripDown) {
                data.ints[0] = 1;
            }
            else {
                data.ints[0] = 0;
            }
        }
        else {
            _GripDown = (data.ints[0] == 1) ? true : false;
        }
        if (_GripDown && _OverlappingDoor) {
            //_DoorObject.GetComponent<DoorController>().Toggle();
            DoorControl.Toggle();
        }
        _GripDown = false;
    }

    void IGlobalGripPressDownHandler.OnGlobalGripPressDown(VREventData eventData) {
        if (Host) {
            _GripDown = true; 
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Door") {
            _OverlappingDoor = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Door") {
            _OverlappingDoor = false;
        }
    }
}
