using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartFollow : MonoBehaviour {
	
	public bool isFollowing;
	
	public Transform RotObj;
	
	public Transform LPoint;
	public Transform RPoint;
	
	public LayerMask LM;
	
	void Update () {
		if (isFollowing) {
			transform.SetParent(GameObject.FindObjectOfType<Movement>().transform);
			
			RaycastHit H;
			RaycastHit H2;
			Physics.Raycast(RotObj.position+RotObj.up, -Vector3.up, out H, Mathf.Infinity, LM);
			Physics.Raycast(RotObj.position+RotObj.up-(RotObj.forward).normalized, -Vector3.up, out H2, Mathf.Infinity, LM);
			
			RotObj.localEulerAngles = new Vector3 ((H.distance - H2.distance)*20f, 0, 0);
			RotObj.localPosition = new Vector3 (0, 0, -H.distance+1);
			transform.position = Vector3.zero;
			transform.localEulerAngles = new Vector3 (0, 180, 0);
			
			GameObject.FindObjectOfType<Movement>().LPoint = LPoint;
			GameObject.FindObjectOfType<Movement>().RPoint = RPoint;
			
			transform.SetParent(null);
			
		}
	}
	
	void DoFollow () {
		isFollowing = !isFollowing;
	}
}
