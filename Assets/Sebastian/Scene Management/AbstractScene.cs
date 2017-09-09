using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRL.SceneManagement {
	public abstract class AbstractScene : MonoBehaviour {

		public float enterTime = 1f;
		public float exitTime = 1f;

		public List<ISceneObject> sceneObjects;

		private Coroutine _enterRoutine;
		private Coroutine _exitRoutine;

		public Action<float> OnEnterStart;
		public Action OnEnterFinish;
		public Action<float> OnExitStart;
		public Action OnExitFinish;

		protected virtual void OnEnable() {
			foreach (ISceneObject obj in sceneObjects) {
				if (obj != null)
					Register(obj);
			}
		}

		protected virtual void OnDisable() {
			foreach (ISceneObject obj in sceneObjects) {
				if (obj != null)
					Deregister(obj);
			}
		}

		protected void Register(ISceneObject obj) {
			OnEnterStart += obj.OnEnterStart;
			OnEnterFinish += obj.OnEnterFinish;
			OnExitStart += obj.OnExitStart;
			OnExitFinish += obj.OnEnterFinish;
		}

		protected void Deregister(ISceneObject obj) {
			OnEnterStart -= obj.OnEnterStart;
			OnEnterFinish -= obj.OnEnterFinish;
			OnExitStart -= obj.OnExitStart;
			OnExitFinish -= obj.OnExitFinish;
		}

		//We choose to restart the enter/exit routines if they're called mid routine.
		//We can choose instead to ignore further calls if the routine is already running.

		public virtual void Enter(float seconds = -1f,
			Action<float> OnSceneEnterStart = null, Action OnSceneEnterFinish = null) {
			//Enter the scene. Execute EnterRoutineAsync in the timespan seconds.
			if (seconds == -1f) {
				seconds = enterTime;
			}
			if (_enterRoutine != null) {
				StopCoroutine(_enterRoutine);
				_enterRoutine = null;
			}
			_enterRoutine = StartCoroutine(EnterRoutineAsync(seconds, OnSceneEnterStart, OnSceneEnterFinish));
		}
		protected abstract IEnumerator EnterRoutineAsync(float seconds, Action<float> OnSceneEnterStart, Action OnSceneEnterFinish);

		public virtual void Exit(float seconds = -1f,
			Action<float> OnSceneExitStart = null, Action OnSceneExitFinish = null) {
			//Exit the scene. Execute ExitRoutineAsync in the timespan seconds.
			if (seconds == -1f) {
				seconds = exitTime;
			}
			
			if (_exitRoutine != null) {
				StopCoroutine(_exitRoutine);
				_exitRoutine = null;
			}
			_exitRoutine = StartCoroutine(ExitRoutineAsync(seconds, OnSceneExitStart, OnSceneExitFinish));
		}
		protected abstract IEnumerator ExitRoutineAsync(float seconds, Action<float> OnSceneExitStart, Action OnSceneExitFinish);
	}
}
