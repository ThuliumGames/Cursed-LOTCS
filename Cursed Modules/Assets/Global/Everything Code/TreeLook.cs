using UnityEngine;

public class TreeLook : MonoBehaviour {
	
	void Update () {
		Vector3 VTmp = Camera.main.transform.position;
		transform.LookAt (VTmp);
	}
}
