using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartFollow : MonoBehaviour {
	
	public bool isFollowing;
	
	public Rigidbody RB;
	
	public Transform LPoint;
	public Transform RPoint;
	
	public LayerMask LM;
	
	void Update () {
		if (isFollowing) {
			
			transform.SetParent(GameObject.FindObjectOfType<Movement>().transform);
			transform.localPosition = Vector3.zero;
			transform.localEulerAngles = new Vector3 (0, 180, 0);
			RB.AddRelativeTorque(-10, 0, 0);
			GameObject.FindObjectOfType<Movement>().LPoint = LPoint;
			GameObject.FindObjectOfType<Movement>().RPoint = RPoint;
			RB.transform.localPosition = new Vector3 (0, (-Mathf.Sin(RB.transform.localEulerAngles.x))-0.28f, -7.769997f);
			
		} else {
			
			transform.SetParent(null);
			
		}
	}
	
	void DoFollow () {
		isFollowing = !isFollowing;
	}
}
