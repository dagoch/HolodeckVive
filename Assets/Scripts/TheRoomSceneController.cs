using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRoomSceneController : MonoBehaviour, ISceneController {

    public float ExitDuration;
    public AnimationCurve ExitCurve;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void Appear() {
    }

    public void Exit() {
        StartCoroutine(Squash());
    }

    IEnumerator Squash() {
        var time = 0f;
        var startY = transform.localScale.y;

        while (time < ExitDuration) {
            time += Time.deltaTime;
            var percent = time / ExitDuration;
            var scale = transform.localScale;
            scale.y = Mathf.Lerp(startY, 0f, ExitCurve.Evaluate(percent));
            transform.localScale = scale;
            yield return 0;
        }
        transform.ActOnChildren((Transform t) => { t.gameObject.SetActive(false); });

        yield return null;
    }
}
