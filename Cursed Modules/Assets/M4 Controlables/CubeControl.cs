using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : Rideables {
	
	void LateUpdate () {
		GetOn();
		if (GetComponent<Rideables>().Riding) {
			GetComponent<Rigidbody>().velocity = Camera.main.transform.right*SSInput.LHor[0] + Camera.main.transform.forward*SSInput.LVert[0];
			if (SSInput.B[0] == "Pressed" && SSInput.LHor[0] == 0 && SSInput.LVert[0] == 0) {
				GlobVars.Reading = false;
				Interactables.StopI();
			}
		}
	}
}
