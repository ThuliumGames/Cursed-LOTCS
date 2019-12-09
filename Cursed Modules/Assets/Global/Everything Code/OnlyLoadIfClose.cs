using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyLoadIfClose : MonoBehaviour {
	
	public GameObject Obj;
	public float Range;
	
	void Update () {
		Obj.SetActive(false);
		if (Vector3.Distance(transform.position, GameObject.FindObjectOfType<Movement>().transform.position) < Range) {
			Obj.SetActive(true);
		}
	}
	
	void OnDrawGizmosSelected () {
		Gizmos.DrawWireSphere(transform.position, Range);
	}
}
