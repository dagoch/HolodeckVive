using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FRL.SceneManagement {
	public class SceneManager : MonoBehaviour {
		public List<AbstractScene> scenes;
		private int _currentSceneIndex = -1;
		
		void Start() {
			LoadScene(_currentSceneIndex);
		}

		public void NextScene() {
			//Load the next scene in the list.
			int nextSceneIndex = (_currentSceneIndex + 1) % scenes.Count;
			LoadScene(nextSceneIndex);
		}

		public void LoadScene(int nextSceneIndex) {
			AbstractScene nextScene = scenes[nextSceneIndex];
			if (_currentSceneIndex == -1) {
				//No scene is loaded. No need to exit!
				nextScene.Enter();
			} else {
				AbstractScene currentScene = scenes[_currentSceneIndex];
				currentScene.Exit(OnSceneExitFinish: () => {
					nextScene.Enter();
				});
			}
			_currentSceneIndex = nextSceneIndex;
		}
	}
}
