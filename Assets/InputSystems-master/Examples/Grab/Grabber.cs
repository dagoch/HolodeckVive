using UnityEngine;
using System.Collections;
using FRL.IO;

public class Grabber : MonoBehaviour, IGlobalTriggerPressDownHandler, IGlobalTriggerPressUpHandler {

    private GameObject _OverlappedObject;
    private bool _Holding = false;
    private Transform _SavedTransform;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Grabbable") {
            _OverlappedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
       if (other.gameObject == _OverlappedObject && !_Holding) {
            _OverlappedObject = null;
        } 
    }

    void IGlobalTriggerPressDownHandler.OnGlobalTriggerPressDown(VREventData eventData) {
        if (_OverlappedObject != null) {
            _SavedTransform = _OverlappedObject.transform.parent;
            _OverlappedObject.transform.parent = transform;
            Debug.Log("set to new parent: " + transform);
            _Holding = true;
        }
    }

    void IGlobalTriggerPressUpHandler.OnGlobalTriggerPressUp(VREventData eventData) {
        if (_OverlappedObject != null) {
            _Holding = false;
            Debug.Log("set to old parent: " + _SavedTransform);
            _OverlappedObject.transform.parent = _SavedTransform;
        }
    }
}
