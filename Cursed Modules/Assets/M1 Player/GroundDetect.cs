using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetect : MonoBehaviour {
	
	public Movement M;
	
	void OnTriggerEnter () {
		if (!M.Jumped) {
			M.CanOG = true;
		}
	}
	
	void OnTriggerStay () {
		if (Mathf.Abs (M.GetComponent<Rigidbody>().velocity.y) < 0.5f) {
			M.CanOG = true;
		}
	}
}
