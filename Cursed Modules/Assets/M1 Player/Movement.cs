using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public Animator Anim;
	
	public float Acceleration;
	public float TurnSpeed;
	public float JumpPower;
	public float MaxStepHeight;
	
	Vector3 Pos;
	Vector3 PrevPos;
	public LayerMask LM;
	
	bool WasPaused;
	Vector3 PrevVel;
	
	Quaternion Q;
	
	void Update () {
		
		if (!GlobVars.Paused) {
			Pos = transform.position;
			GetComponent<Rigidbody>().isKinematic = false;
			if (WasPaused) {
				GetComponent<Rigidbody>().velocity = PrevVel;
				WasPaused = false;
			}
			
			if (!GlobVars.PlayerPaused && !GlobVars.Reading) {
				
				if (Anim.GetBool("OnGround")) {
					
					if (SSInput.Y[0] == "Pressed") {
						transform.position += new Vector3 (0, 1f, 0);
						transform.rotation = Q;
						GetComponent<Rigidbody>().velocity = new Vector3 (GetComponent<Rigidbody>().velocity.x, JumpPower, GetComponent<Rigidbody>().velocity.z);
					}
					
					if (SSInput.B[0] == "Down") {
						Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), new Vector2 (SSInput.LHor[0], SSInput.LVert[0]).magnitude, Acceleration * Time.deltaTime));
					} else {
						Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), new Vector2 (SSInput.LHor[0], SSInput.LVert[0]).magnitude*0.5f, Acceleration * Time.deltaTime));
					}
					if (new Vector2 (SSInput.LHor[0], SSInput.LVert[0]).magnitude > 0.01f) {
						GameObject G = new GameObject();
						G.transform.position = transform.position;
						G.transform.eulerAngles = new Vector3 (0, Camera.main.transform.eulerAngles.y + Mathf.Atan2(SSInput.LHor[0], SSInput.LVert[0])*Mathf.Rad2Deg, 0);
						Q = G.transform.rotation;
						Destroy (G);
					}
					transform.rotation = Quaternion.Slerp (transform.rotation, Q, (TurnSpeed/(Mathf.Pow(Anim.GetFloat("VSpeed")+1, 2)))*Time.deltaTime);
				}
			} else {
				Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), 0, Acceleration * Time.deltaTime));
			}
			
			RaycastHit Hit1;
			if (Physics.Raycast ((transform.position)+(transform.up*MaxStepHeight), Vector3.down, out Hit1, MaxStepHeight+1, LM)) {
				transform.position = new Vector3 (transform.position.x, Hit1.point.y+0.125f, transform.position.z);
				Anim.SetBool("OnGround", true);
			} else {
				Anim.SetBool("OnGround", false);
				Anim.SetFloat("YVel", GetComponent<Rigidbody>().velocity.y/10);
			}
		} else {
			if (!WasPaused) {
				PrevVel = GetComponent<Rigidbody>().velocity;
				WasPaused = true;
			}
			transform.position = Pos;
			GetComponent<Rigidbody>().isKinematic = true;
		}
	}
}
