using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : Rideables {
	
	GameObject Player;
	Vector3 PosToGo;
	bool CanChangeSpeed = true;
	int CurentSpeed;
	
	void LateUpdate () {
		Player = GameObject.Find("Player");
		if (!GetComponent<Rideables>().Riding) {
			CurentSpeed = 1;
			GetOn();
		} else {
			GetComponent<Rigidbody>().angularVelocity = new Vector3 (0, SSInput.LHor[0] * TurnSpeed, 0);
			GetComponent<Rigidbody>().velocity = (transform.forward * GetComponent<Rideables>().Speeds[CurentSpeed]) + new Vector3 (0, GetComponent<Rigidbody>().velocity.y, 0);
			
			if (CanChangeSpeed) {
				if (SSInput.LVert[0] >= 0.75) {
					CanChangeSpeed = false; 
					if (CurentSpeed < GetComponent<Rideables>().Speeds.Length - 1) {
						CurentSpeed++;
					}
				}
				if (SSInput.LVert[0] <= -0.75) {
					CanChangeSpeed = false; 
					if (CurentSpeed > 0) {
						CurentSpeed--;
					}
				}
			} else {
				if (SSInput.LVert[0] == 0) {
					CanChangeSpeed = true;
				}
			}
			
			if (SSInput.B[0] == "Pressed" && SSInput.LHor[0] == 0 && SSInput.LVert[0] == 0) {
				GlobVars.Reading = false;
				Player.transform.parent = GameObject.Find("All Level Objects").transform;
				Player.GetComponent<Animator>().enabled = true;
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
