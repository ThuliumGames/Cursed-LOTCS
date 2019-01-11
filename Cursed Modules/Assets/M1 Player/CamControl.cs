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
	
	void Update () {
		if (!GlobVars.PlayerPaused || GlobVars.Reading) {
			transform.position = ObjToFollow.position;
			transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			ObjToFollow.eulerAngles = transform.eulerAngles;
			transform.Rotate (0, SSInput.RHor[0]*100*Time.deltaTime, 0);
			transform.Translate (0, Up, -Back);
			transform.eulerAngles = new Vector3 (10, transform.eulerAngles.y, 0);
			UpDown += -SSInput.RVert[0]*100*Time.deltaTime;
			UpDown = Mathf.Clamp (UpDown, Min, Max);
			transform.RotateAround (ObjToFollow.position+new Vector3 (0,1,0), transform.right, UpDown);
		}
	}
}
