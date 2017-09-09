using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRL.SceneManagement {
	public interface ISceneObject {
		void OnEnterStart(float seconds);
		void OnEnterFinish();
		void OnExitStart(float seconds);
		void OnExitFinish();
	}
}
