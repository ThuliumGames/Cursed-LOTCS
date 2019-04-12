using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rideables : MonoBehaviour {
	
	public float[] Speeds;
	public float TurnSpeed;
	public Vector3 Offset;
	public bool Riding;
	public float PlayerMass;
	GameObject Player;
	
	public void GetOn () {
		Player = GameObject.Find("Player");
		if (GlobVars.InteractObject == this.gameObject) {
			Riding = true;
			GlobVars.Reading = true;
			Player.GetComponent<Movement>().enabled = false;
			Player.GetComponent<Animator>().enabled = false;
			Player.transform.position = transform.position + Offset;
			Player.transform.rotation = transform.rotation;
			Player.transform.parent = this.gameObject.transform;
			PlayerMass = Player.GetComponent<Rigidbody>().mass;
			Destroy(Player.GetComponent<Rigidbody>());

		} else {
			Riding = false;
		}
	}
}
