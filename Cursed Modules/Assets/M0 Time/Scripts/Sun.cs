﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Sun : MonoBehaviour {

	public float TimeMultiplier = 1f;
	public float Count;

	public float SunChangeSpeed = 0.01f;
	public PostProcessVolume PPV;
	ColorGrading CG;

	public DayPhase _dayPhase;

	public enum DayPhase {Dawn, Day, Dusk, Night}

	Light SunLight;
	float GameSpeed;
	
	public GUIStyle style;
	

	void Start () {
		SunLight = GetComponent <Light>();
		_dayPhase = DayPhase.Dawn;
		GlobVars.Hour = 6;
	}
	

	void Update () {
		
		PPV.profile.TryGetSettings(out CG);
		
		GameSpeed = Time.deltaTime * TimeMultiplier;
		
		if (GlobVars.Days > 365 || GlobVars.Days < 1) {
			GlobVars.Days = 1;
		}

		if (!GlobVars.Paused) {
			Count += GameSpeed;
			if (Count >= 60) {
				Count = 0;
				GlobVars.Hour ++;
				if (GlobVars.Hour > 24) {
					GlobVars.Hour = 1;
					GlobVars.Days ++;
				}
			}
		}
		GlobVars.Mins = (int) Count;
		transform.localEulerAngles = new Vector3 (-90, 0, 0);
		transform.RotateAround(transform.position, GameObject.Find("SunAngler").transform.right, -0.25f * (Count + (GlobVars.Hour * 60)));
		if (GlobVars.Hour < 18) {
			CG.temperature.value = Mathf.Sin((-0.25f * (Count + (GlobVars.Hour * 60)))*Mathf.Deg2Rad)*50;
		} else {
			CG.temperature.value = Mathf.Sin((-0.25f * (Count + (GlobVars.Hour * 60)))*Mathf.Deg2Rad)*-50;
		}
	}

	void OnGUI () {
		GUI.Label (new Rect(Screen.width - 200, Screen.height - 150, 100, 20), "Day " + GlobVars.Days, style);
		if (GlobVars.Mins < 10) {
			if (GlobVars.Hour <= 12) {
				GUI.Label (new Rect(Screen.width - 200, Screen.height - 100, 100, 20), GlobVars.Hour + ":" + 0 + GlobVars.Mins, style);
			} else {
				GUI.Label (new Rect(Screen.width - 200, Screen.height - 100, 100, 20), (GlobVars.Hour-12) + ":" + 0 + GlobVars.Mins, style);
			}
		} else {
			if (GlobVars.Hour <= 12) {
				GUI.Label (new Rect(Screen.width - 200, Screen.height - 100, 100, 20), GlobVars.Hour + ":" + GlobVars.Mins, style);
			} else {
				GUI.Label (new Rect(Screen.width - 200, Screen.height - 100, 100, 20), (GlobVars.Hour-12) + ":" + GlobVars.Mins, style);
			}
		}
	}
}