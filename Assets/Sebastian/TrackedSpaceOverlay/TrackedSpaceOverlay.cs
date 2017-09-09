using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRL.TrackedSpace {
	[ExecuteInEditMode]
	public class TrackedSpaceOverlay : MonoBehaviour {

		public Color color;
		public Vector3 grain;
		public Vector3 width;
		private Renderer _renderer;

		// Use this for initialization
		void Start () {
			_renderer = this.GetComponent<Renderer>();
		}
		
		// Update is called once per frame
		void Update () {
			Vector3 scale = transform.localScale;
			Vector4 scaledWidth = new Vector4(width.x / scale.x, width.y / scale.y, width.z / scale.z, 1);
			//Vector4 scaledGrain = new Vector4(grain.x / scale.x, grain.y / scale.y, grain.z / scale.z, 1);
			_renderer.sharedMaterial.SetVector("_Color", color);
			_renderer.sharedMaterial.SetVector("_Grain", new Vector4(grain.x, grain.y, grain.z, 1));
			_renderer.sharedMaterial.SetVector("_Width", scaledWidth);
		}
	}
}
