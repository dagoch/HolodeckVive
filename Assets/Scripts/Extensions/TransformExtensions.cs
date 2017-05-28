using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions {

	public static Transform[] GetAllChildren(this Transform t) {
		return RecursiveGetAllWhere(t, new List<Transform>(), (Transform tran) => { return tran != t; })();
	}

	public static Transform[] GetAllChildrenWhere(this Transform t, Predicate<Transform> p ) {
		return RecursiveGetAllWhere(t, new List<Transform>(), p)();	
	}

	private static Func<Transform[]> RecursiveGetAllWhere(
		Transform t,
		List<Transform> l,
		Predicate<Transform> p
	) {
		if (p(t)) {
			l.Add(t);
		}
		for (int i = 0; i < t.childCount; i++) {
			RecursiveGetAllWhere(t.GetChild(i), l, p);
		}
		return () => { return l.ToArray(); };
	}

    public static void ActOnChildren(this Transform t, Action<Transform> a) {
        var children = t.GetAllChildren();
        for (var i = 0; i < children.Length; i++) {
            a(children[i]);
        }
    }
}
