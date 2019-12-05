using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowboard : Rideables {

	public Transform Vis;
	
	void LateUpdate () {
		if (!GetComponent<Rideables>().Riding) {
			GetOn();
		} else {
			
			Player.transform.position = transform.position+Offset;
			Player.transform.eulerAngles = Vis.eulerAngles;
			
			GetComponent<Rigidbody>().AddTorque(Vis.forward*SSInput.LVert[0]*1000 + Vis.right*SSInput.LHor[0]*1000);
			RaycastHit H;
			Physics.Raycast(transform.position, Vector3.down, out H);
			Vis.LookAt(H.normal);
			Vis.Rotate (0, SSInput.LHor[0]*20, 0);
			
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
