using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {
	
	public Transform ObjToFollow;
	float UpDown;
	public float Max;
	public float Min;
	public float Back;
	public float Up;
	
	public LayerMask LM;
	
	void Update () {
		if (!GlobVars.Paused) {
			transform.position = ObjToFollow.position;
			transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			transform.Rotate (0, SSInput.RHor[0]*100*Time.deltaTime, 0);
			transform.Translate (0, Up, -Back);
			transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			UpDown += -SSInput.RVert[0]*100*Time.deltaTime;
			UpDown = Mathf.Clamp (UpDown, Min, Max);
			transform.RotateAround (ObjToFollow.position+new Vector3 (0,1,0), transform.right, UpDown);
			RaycastHit Hit;
			if (Physics.Raycast (ObjToFollow.position+(Vector3.up*Up), -transform.forward, out Hit, Back+0.5f, LM)) {
				transform.Translate (0, 0, Back-Hit.distance+0.5f);
			}
			transform.Rotate (10, 0, 0);
		}
	}
}
