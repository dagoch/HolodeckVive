using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaPartySceneController : MonoBehaviour, ISceneController {

    public float Duration;
    public AnimationCurve FallCurve;

    public Transform Pivot;

    public void Appear() {
        StartCoroutine(DropWall());
    }

    public void Exit() {

    }

    IEnumerator DropWall() {
        var time = 0f;

        while (time < Duration) {
            time += Time.deltaTime;
            var percent = time / Duration;
            var eulerRotation = Pivot.eulerAngles;
            eulerRotation.x = Mathf.Lerp(-90f, 0, FallCurve.Evaluate(percent));
            Pivot.rotation = Quaternion.Euler(eulerRotation);
            yield return 0;
        }
    }
}
