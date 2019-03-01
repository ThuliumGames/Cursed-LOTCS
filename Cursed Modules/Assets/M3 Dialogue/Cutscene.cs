using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour {
	
	bool isCut = false;
	
	public string CutSceneName;
	string OrigSceneName;
	public float CutsceneLength;
	public Vector3 ReturnLocation;
	
	float T = 0;
	
	void Update () {
		
		if (isCut) {
			
			T += Time.deltaTime;
					
			if (T > CutsceneLength) {
				isCut = false;
				if (CutsceneLength > 0) {
					SceneManager.LoadScene(OrigSceneName);
				}
			}
			
		} else {
			if ((T > CutsceneLength && SceneManager.GetActiveScene().name == OrigSceneName) || (CutsceneLength == 0 && SceneManager.GetActiveScene().name == CutSceneName)) {
				GameObject.Find("Player").transform.position = ReturnLocation;
				Destroy(this.gameObject);
			}
		}
	}
	
	void LateUpdate () {
		OrigSceneName = SceneManager.GetActiveScene().name;
	}
	
	void DoCut () {
		if (SceneManager.GetActiveScene().name == OrigSceneName) {
			if ((GameObject.Find (name + "CutKeep") == null && !name.Contains("CutKeep"))) {
				name += "CutKeep";
				isCut = true;
				T = 0;
				Application.DontDestroyOnLoad(this.gameObject);
				SceneManager.LoadScene(CutSceneName);
			} else if (!name.Contains("CutKeep")) {
				Destroy(this.gameObject);
			}
		}
	}
}
