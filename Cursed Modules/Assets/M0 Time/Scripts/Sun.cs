using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

	public float TimeMultiplier = 1f;
	public float Count;

	public float SunDimTime = 0.001f;
	float[] SunIntensity = {0.5f, 1f, 0.25f, 0f};

	public DayPhase _dayPhase;

	public enum DayPhase {Dawn, Day, Dusk, Night}

	Light SunLight;
	float GameSpeed;
	
	public GUIStyle style;
	

	void Start () {
		SunLight = GetComponent <Light>();
		_dayPhase = DayPhase.Dawn;
		SunLight.intensity = SunIntensity[0];
		GlobVars.Hour = 6;
	}
	

	void Update () {
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

		if (GlobVars.Hour >= GlobVars.SunChangeTime[0] && GlobVars.Hour < GlobVars.SunChangeTime[1]) { //Dawn
			
			_dayPhase = DayPhase.Dawn;
			SunLight.intensity = Mathf.Lerp (SunLight.intensity, SunIntensity[0], SunDimTime * GameSpeed);

		} else if (GlobVars.Hour >= GlobVars.SunChangeTime[1] && GlobVars.Hour < GlobVars.SunChangeTime[2]) { //Day
			
			_dayPhase = DayPhase.Day;
			SunLight.intensity = Mathf.Lerp (SunLight.intensity, SunIntensity[1], SunDimTime * GameSpeed);

		} else if (GlobVars.Hour >= GlobVars.SunChangeTime[2] && GlobVars.Hour < GlobVars.SunChangeTime[3]) { //Dusk
			
			_dayPhase = DayPhase.Dusk;
			SunLight.intensity = Mathf.Lerp (SunLight.intensity, SunIntensity[2], SunDimTime * GameSpeed);

		} else { //Night
			
			_dayPhase = DayPhase.Night;
			SunLight.intensity = Mathf.Lerp (SunLight.intensity, SunIntensity[3], SunDimTime * GameSpeed);
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