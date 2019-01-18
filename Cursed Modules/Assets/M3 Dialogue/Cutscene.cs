using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

[System.Serializable]
public class Objects {
	public Animator ObjectToMove;
	public RuntimeAnimatorController NewAnimCon;
	public bool RMToggle;
	[HideInInspector]
	public RuntimeAnimatorController OrigAnimCon;
	[HideInInspector]
	public bool RMWasOn;
	[HideInInspector]
	public Vector3 PrevPos;
}
public class Cutscene : MonoBehaviour {
	
	bool isCut;
	
	public Objects[] ObjectsToMove;
	public Camera CutCam;
	public float CutsceneLength;
	
	float T = 0;
	
	void Update () {
		if (isCut) {
			T += Time.deltaTime;
			GlobVars.Reading = true;
			CutCam.depth = 10;
		}
		
		if (T >= CutsceneLength) {
			isCut = false;
			T = 0;
			foreach (Objects O in ObjectsToMove) {
				O.PrevPos = O.ObjectToMove.transform.position;
				O.ObjectToMove.runtimeAnimatorController = O.OrigAnimCon;
				if (O.RMWasOn) {
					O.ObjectToMove.applyRootMotion = true;
				} else {
					O.ObjectToMove.applyRootMotion = false;
				}
				O.ObjectToMove.transform.position = O.PrevPos;
			}
			GlobVars.Reading = false;
			CutCam.depth = -100;
			GlobVars.Interacting = false;
			GlobVars.InteractObject = null;
			Destroy(this.gameObject);
		}
	}
	
	void DoCut () {
		isCut = true;
		foreach (Objects O in ObjectsToMove) {
		
			if (O.ObjectToMove.applyRootMotion) {
				O.RMWasOn = true;
			} else {
				O.RMWasOn = false;
			}
			
			if (O.RMToggle) {
				O.ObjectToMove.applyRootMotion = !O.ObjectToMove.applyRootMotion;
			}
			
			O.OrigAnimCon = O.ObjectToMove.runtimeAnimatorController;
			O.ObjectToMove.runtimeAnimatorController = O.NewAnimCon;
		}
	}
	
}
