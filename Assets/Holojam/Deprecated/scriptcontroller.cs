using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using FRL.IO;

namespace Holojam.Tools
{
	[ExecuteInEditMode]
	public class scriptcontroller :  MonoBehaviour, IGlobalTouchpadTouchSetHandler
	{
		public TextAsset Textfile;
		public string[] lines;
		public float dis;
		public int dir;
		public Transform center;
		public VRConsole[] Vcon;
		public int top, bottom, previous, next;
		public float t;
		public int mode = 0;
		public float masterDis;
		public Transform telePos;
		public float masterAlpha;
		public GameObject targetPos;
        [SerializeField]
        public float touch_y;
		private int curmode = 0;
		private float origin = -2f;
		private float timethreshold = 0.5f;
		private float timer = 0;
		private static Vector3 _Reset = new Vector3 (0, 2, 0);


		// Use this for initialization
		void Start ()
		{
			top = 0;
			bottom = 5;
			previous = 0;
			next = 5;
			lines = Textfile.text.Split ('\n');
			for (int i = 0; i < 6; i++) {
				Vcon [i].gameObject.GetComponent<MeshRenderer> ().enabled = true;
				Vcon [i].setText (lines [i]);
				SetColor (i);
			}
			SetAlpha (0);
		}

		void Update ()
		{
			//telePos.rotation = GameObject.Find ("OriginTele").transform.rotation;
		}
		// Update is called once per frame
		void LateUpdate ()
		{
			//masterAlpha = GameObject.Find ("MasterManager").transform.position.z;
			//if (mode == 3) {
			//	for (int i = 0; i < 6; i++) {
			//		Vcon [i].gameObject.GetComponent<MeshRenderer> ().enabled = true;
			//	}
			//	masterDis = GameObject.Find ("MasterManager").GetComponent<MasterManager> ().masterDis;
			//	SetPos (masterDis);
			//}
			//if (mode == 5) {
			//	telePos.position = GameObject.Find ("OriginTele").transform.position;
			//	telePos.parent = GameObject.Find ("VRCamera").transform;
			//}
			//if (mode == 4) {
			//	if (telePos.parent.gameObject == targetPos)
			//		telePos.parent = null;
			//	else
			//		telePos.parent = targetPos.transform;
			//}

			//if (mode < 3 && mode > -1)
			//	curmode = mode;
			
		}

		void InitText ()
		{
			for (int i = 0; i < 6; i++) {
				Vcon [i].gameObject.GetComponent<MeshRenderer> ().enabled = true;
				Vcon [i].setText (lines [i]);
			}
		}

		void SetPrompterPos (float _dis)
		{
			telePos.localPosition += new Vector3 (0, _dis * 0.1f, 0);
		}

		void SetPos ()
		{
			SetPos (dis);
		}

		void SetPos (float _dis)
		{
            Debug.Log("the new dis is" + _dis);
			for (int i = 0; i < 6; i++) {
				Vcon [i].transform.localPosition += new Vector3 (0, _dis*3, 0);
			}				
			if (Vcon [top].transform.localPosition.y > 5) {
				Vcon [top].transform.localPosition = Vcon [bottom].transform.localPosition - _Reset;
                //Debug.Log("Change Top");
               // Debug.Log(Vcon[top].transform.localPosition.y);
				SetText (top, true);
				bottom = top;
                //Debug.Log(Vcon[top].transform.localPosition.y);
                top = (top + 1) % 6;
			}
			if (Vcon [bottom].transform.localPosition.y < -4) {
				Vcon [bottom].transform.localPosition = Vcon [top].transform.localPosition + _Reset;
				SetText (bottom, false);
				top = bottom;
				bottom = (top + 5) % 6;
			}
			SetAlpha (masterAlpha);
				
		}

		void SetText (int index, bool forward)
		{
			if (forward) {
				if (next + 1 < lines.Length) {
					Vcon [index].setText (lines [++next]);
					SetColor (index);
					if (next > 6)
						previous++;
				} else {
					//if (!(previous == 0||next == lines.Length - 1))
					Vcon [index].setText ("");
					if (previous + 1 <= next)
						previous++;
				}
			} else {
				if (previous - 1 >= 0) {
					Vcon [index].setText (lines [previous--]);
					SetColor (index);
					if (previous < lines.Length - 6)
						next--;
				} else {
					Vcon [index].setText ("");
					if (next - 1 >= previous)
						next--;
				}
			}

		}

		void InitColor ()
		{
		}

		void SetAlpha ()
		{
			SetAlpha (0);
		}

		void SetAlpha (float alpha)
		{
			for (int i = 0; i < 6; i++) {
                Vcon[i].setAlpha(t);
            }
		}

		void SetColor (int ind)
		{
			string tmp = Vcon [ind].getText ();
			int i = (int)Char.GetNumericValue (tmp [0]);
			tmp = tmp.Remove (0, 1);
			Vcon [ind].setText (tmp);
			switch (i) {
			case 1:
				Vcon [ind].setColor (new Vector4 (0.5f, 0.7f, 1f, 1f));
				break;
			case 2:
				Vcon [ind].setColor (new Vector4 (1f, 0.6f, 0.6f, 1f));
				break;
			case 3:
				Vcon [ind].setColor (Color.red);
				break;
			default:
				Vcon [ind].setColor (Color.white);
				break;	
			}
		}


        public void OnGlobalTouchpadTouchDown(ViveControllerModule.EventData eventData)
        {
            for (int i = 0; i < 6; i++)
            {
                Vcon[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            touch_y = eventData.touchpadAxis.y;
            Debug.Log("touch!");
        }
    

        public void OnGlobalTouchpadTouch(ViveControllerModule.EventData eventData)
        {
            dis = eventData.touchpadAxis.y - touch_y;
            if (!((previous == 0 && dis < 0) || (next == lines.Length - 1 && dis >= 0)))
            {
                SetPos(dis);
            }
            //Debug.Log(dis);
            touch_y = eventData.touchpadAxis.y;
        }

        public void OnGlobalTouchpadTouchUp(ViveControllerModule.EventData eventData)
        {
            //throw new NotImplementedException();
        }
    }
}

