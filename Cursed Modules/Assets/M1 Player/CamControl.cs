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
	
	float TimePressed;
	
	void LateUpdate () {
		
		ObjToFollow = GlobVars.CurrentPlayer.transform;
		
		if (!GlobVars.Paused) {
			
			if (ObjToFollow.GetComponent<Animator>().GetBool("Crouching")) {
				Up = 1;
			} else {
				Up = 1.75f;
			}
			
			GameObject G = new GameObject();
			if (FollowSpeed > 0) {
				G.transform.position = Vector3.Lerp (Pre, ObjToFollow.position, FollowSpeed*Time.deltaTime);
				transform.position = G.transform.position;
			} else {
				transform.position = ObjToFollow.position;
			}
			transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			if (ObjToFollow.GetComponent<Animator>().GetBool("isTargeting")) {
				transform.rotation = Quaternion.Lerp(transform.rotation, ObjToFollow.rotation, 10*Time.deltaTime);
			} else {
				
				if (SSInput.B[0] == "Down") {
					transform.Rotate (0, (SSInput.LHor[0]*50)*Time.deltaTime, 0);
				}
				
				transform.Rotate (0, ((SSInput.RHor[0]*(100*(TimePressed+0.5f))))*Time.deltaTime, 0);
				transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			}
			transform.Translate (0, Up, -Back);
		
			if (SSInput.B[0] == "Down") {
				if (ObjToFollow.InverseTransformPoint(transform.position).z <= 0) {
					UpDown -= (UpDown-(-ObjToFollow.GetComponent<Animator>().GetFloat("Slope")*180))*Time.deltaTime;
				} else {
					UpDown -= (UpDown-(ObjToFollow.GetComponent<Animator>().GetFloat("Slope")*180))*Time.deltaTime;
				}
			}
			
			UpDown += -SSInput.RVert[0]*(100*(TimePressed+0.5f))*Time.deltaTime;
			UpDown = Mathf.Clamp (UpDown, Min, Max);
			transform.RotateAround (ObjToFollow.position+new Vector3 (0,1,0), transform.right, UpDown);
			RaycastHit Hit1;
			if (Physics.BoxCast (ObjToFollow.position+(Vector3.up*Up), Vector3.one*0.5f, -transform.forward, out Hit1, Quaternion.Euler (Vector3.zero), Back+0.5f, LM)) {
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
			if (new Vector2(SSInput.RHor[0], SSInput.RVert[0]).magnitude > 0.1f) {
				TimePressed += Time.deltaTime*4;
			} else {
				TimePressed = 0;
			}
			TimePressed = Mathf.Clamp01 (TimePressed);
			Destroy(G);
		}
	}
}
