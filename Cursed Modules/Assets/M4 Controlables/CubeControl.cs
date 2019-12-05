using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : Rideables {
	
	Vector3 PosToGo;
	float Angle;
	public float Speed;
	public float TurnSpeed;
	
	void LateUpdate () {
		if (!GetComponent<Rideables>().Riding) {
			if (GetComponent<Animator>()) {
				GetComponent<Animator>().SetBool ("isMoving", false);
			}
			GetOn();
		} else {
			
			Player.transform.localPosition = Offset;
			Player.transform.localEulerAngles = Vector3.zero;
			
			if ((Mathf.Abs (SSInput.LHor[0]) > 0.05f || Mathf.Abs (SSInput.LVert[0]) > 0.05f)) {
				Angle = (Mathf.Atan2(SSInput.LHor[0], SSInput.LVert[0])*Mathf.Rad2Deg)+Camera.main.transform.eulerAngles.y;
				if (Speed > 0) {
					GetComponent<Rigidbody>().velocity = (transform.forward * (Mathf.Clamp01(new Vector2(SSInput.LHor[0], SSInput.LVert[0]).magnitude)*Speed)) + new Vector3 (0, GetComponent<Rigidbody>().velocity.y, 0);;
				}
				if (GetComponent<Animator>()) {
					GetComponent<Animator>().SetBool ("isMoving", true);
				}
				GameObject G = new GameObject();
				G.transform.eulerAngles = new Vector3 (0, Angle, 0);
				transform.rotation = Quaternion.Lerp(transform.rotation, G.transform.rotation, TurnSpeed*Time.deltaTime);
				Destroy(G);
			} else {
				if (Speed > 0) {
					GetComponent<Rigidbody>().velocity = new Vector3 (0, GetComponent<Rigidbody>().velocity.y, 0);
				}
				if (GetComponent<Animator>()) {
					GetComponent<Animator>().SetBool ("isMoving", false);
				}
			}
			
			if (SSInput.B[0] == "Pressed" && SSInput.LHor[0] == 0 && SSInput.LVert[0] == 0) {
				GlobVars.Reading = false;
				Player.transform.parent = GameObject.Find("All Level Objects").transform;
				Player.GetComponent<Animator>().SetBool("Riding", false);
				Player.GetComponent<Movement>().enabled = true;
				if (Player.GetComponent<Rigidbody>() == null) {
					Player.AddComponent<Rigidbody>();
					Rigidbody RB;
					RB = Player.GetComponent<Rigidbody>();
					RB.mass = GetComponent<Rideables>().PlayerMass;
					RB.collisionDetectionMode = CollisionDetectionMode.Continuous;
					RB.interpolation = RigidbodyInterpolation.None;
					RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
				}
				Interactables.StopI();
				GetOn();
			}
		}
	}
}
