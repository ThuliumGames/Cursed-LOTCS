using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : Rideables {
	
	void Update () {
		GetOn();
		if (GetComponent<Rideables>().Riding) {
			GetComponent<Rigidbody>().velocity = Camera.main.transform.right*SSInput.LHor[0] + Camera.main.transform.forward*SSInput.LVert[0];
		}
	}
}
