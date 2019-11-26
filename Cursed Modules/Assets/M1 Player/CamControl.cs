using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {
	
	public Transform ObjToFollow;
	float UpDown = 0;
	public float Max;
	public float Min;
	public float Back;
	public float Up;
	
	public float FollowSpeed;
	
	public LayerMask LM;
	
	float Ac;
	
	Vector3 Pre;
	float Ler;
	
	void LateUpdate () {
		
		if (!GlobVars.Paused) {
			GameObject G = new GameObject();
			G.transform.position = Vector3.Lerp (Pre, ObjToFollow.position, FollowSpeed*Time.deltaTime);
			transform.position = G.transform.position;
			transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			transform.Rotate (0, ((SSInput.RHor[0]*100))*Time.deltaTime, 0);
			transform.Translate (0, Up, -Back);
			transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			UpDown += -SSInput.RVert[0]*100*Time.deltaTime;
			UpDown = Mathf.Clamp (UpDown, Min, Max);
			transform.RotateAround (ObjToFollow.position+new Vector3 (0,1,0), transform.right, UpDown);
			RaycastHit Hit1;
			if (Physics.BoxCast (ObjToFollow.position+(Vector3.up*Up), Vector3.one*0.75f, -transform.forward, out Hit1, Quaternion.Euler (Vector3.zero), Back+0.5f, LM)) {
				RaycastHit Hit;
				if (Physics.Raycast (ObjToFollow.position+(Vector3.up*Up), -transform.forward, out Hit, Back+0.5f, LM)) {
					if (Ler < Back-Hit.distance+0.5f) {
						Ler = Back-Hit.distance+0.5f;
					} else {
						Ler = Mathf.Lerp (Ler, Back-Hit1.distance+0.5f, 3*Time.deltaTime);
					}
				} else {
					Ler = Mathf.Lerp (Ler, Back-Hit1.distance+0.5f, 3*Time.deltaTime);
				}
				transform.Translate (0, 0, Ler);
			} else {
				RaycastHit Hit;
				if (Physics.Raycast (ObjToFollow.position+(Vector3.up*Up), -transform.forward, out Hit, Back+0.5f, LM)) {
					Ler = Back-Hit.distance+0.5f;
				} else {
					Ler = Mathf.Lerp (Ler, 0, 3*Time.deltaTime);
				}
				transform.Translate (0, 0, Ler);
			}
			transform.Rotate (10, 0, 0);
			Pre = G.transform.position;
			Destroy(G);
		}
		
	}
}
