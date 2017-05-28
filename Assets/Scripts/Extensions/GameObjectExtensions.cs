using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions {
	public static bool HasComponent<T>(this GameObject gameObject) {
		return gameObject.GetComponent<T>() != null;
	}

	public static void SetLayers(this GameObject gameObject, int layer) {
		gameObject.layer = layer;
		foreach (Transform child in gameObject.transform) {
			child.gameObject.SetLayers(layer);
		}
	}
}