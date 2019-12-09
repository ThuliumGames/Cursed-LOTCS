using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Expression {
	public string name;
	public Vector3[] LocPos;
	public Vector3[] LocAng;
}

[ExecuteInEditMode]
public class Face : MonoBehaviour {
	
	public Transform[] Objs;
	public Expression[] Faces;
	
	public int FaceToMake;
	
	public bool Set;
	public bool Return;
	
	void Update () {
		if (Application.isPlaying) {
			int i = 0;
			foreach (Transform T in Objs) {
				T.localPosition = Vector3.Lerp (T.localPosition, Faces[FaceToMake].LocPos[i], 5*Time.deltaTime);
				T.localRotation = Quaternion.Lerp (T.localRotation, Quaternion.Euler (Faces[FaceToMake].LocAng[i]), 5*Time.deltaTime);
				i++;
			}
		}
		if (Set) {
			int i = 0;
			foreach (Transform T in Objs) {
				Faces[FaceToMake].LocPos[i] = T.localPosition;
				Faces[FaceToMake].LocAng[i] = T.localEulerAngles;
				i++;
			}
			Set = false;
		}
		if (Return) {
			int i = 0;
			foreach (Transform T in Objs) {
				T.localPosition = Faces[FaceToMake].LocPos[i];
				T.localRotation = Quaternion.Euler (Faces[FaceToMake].LocAng[i]);
				i++;
			}
			Return = false;
		}
	}
}
