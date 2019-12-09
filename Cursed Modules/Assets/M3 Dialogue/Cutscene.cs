using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour {
	
	bool isCut = false;
	public float CutsceneLength;
	public Vector3 StartLocation;
	public Vector3 ReturnLocation;
	public AnimatorOverrideController AOC;
	
	float T = 0;
	
	void Update () {
		
		if (isCut) {
			
			T += Time.deltaTime;
					
			if (T > CutsceneLength) {
				T = 0;
				GlobVars.CurrentPlayer.transform.position = ReturnLocation;
				GlobVars.CurrentPlayer.GetComponent<Animator>().SetBool("isCut", false);
				Interactables.StopI();
				isCut = false;
			}
			
		}
	}
	
	void DoCut () {
		GlobVars.CurrentPlayer.transform.position = StartLocation;
		GlobVars.CurrentPlayer.GetComponent<Animator>().SetBool("isCut", true);
		GlobVars.CurrentPlayer.GetComponent<Animator>().runtimeAnimatorController = AOC;
		GetComponent<Animator>().Play("CutScene");
		GlobVars.CurrentPlayer.GetComponent<Animator>().Play("CutScene");
		isCut = true;
	}
}
