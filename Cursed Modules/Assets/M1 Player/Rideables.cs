using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rideables : MonoBehaviour {
	
	public Vector3 Offset;
	public bool Riding;
	
	public void GetOn () {
		if (GlobVars.InteractObject == this.gameObject) {
			Riding = true;
			GlobVars.Reading = true;
			GameObject.Find("Player").transform.position = transform.position + Offset;
		} else {
			Riding = false;
		}
	}
}
