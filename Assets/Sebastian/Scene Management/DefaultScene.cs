using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRL.SceneManagement {
	public class DefaultScene : AbstractScene {
		
		protected override IEnumerator EnterRoutineAsync(float seconds, Action<float> OnSceneEnterStart, Action OnSceneEnterFinish) {
			if (OnSceneEnterStart != null)
				OnSceneEnterStart(seconds);
			if (OnEnterStart != null)
				OnEnterStart(seconds);

			//DO STUFF HERE.
			yield return new WaitForSeconds(seconds);

			if (OnExitStart != null)
				OnEnterFinish();
			if (OnSceneEnterFinish != null)
				OnSceneEnterFinish();
		}

		protected override IEnumerator ExitRoutineAsync(float seconds, Action<float> OnSceneExitStart, Action OnSceneExitFinish) {
			if (OnSceneExitStart != null)
				OnSceneExitStart(seconds);
			if (OnExitStart != null)
				OnExitStart(seconds);

			//DO STUFF HERE.
			yield return new WaitForSeconds(seconds);

			if (OnExitFinish != null)
				OnExitFinish();
			if (OnSceneExitFinish != null)
				OnSceneExitFinish();
		}
	}
}
