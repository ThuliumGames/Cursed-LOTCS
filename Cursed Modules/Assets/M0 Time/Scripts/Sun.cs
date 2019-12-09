using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class Sun : MonoBehaviour {

	public float TimeMultiplier = 1f;
	public float Count;

	public float SunChangeSpeed = 0.01f;
	Volume SSV;
	ColorAdjustments CA;
	WhiteBalance WB;

	Light SunLight;
	float GameSpeed;
	
	public GUIStyle style;
	

	void Start () {
		SSV = GetComponentInChildren<Volume>();
		SunLight = GetComponent <Light>();
		GlobVars.Hour = 6;
	}
	

	void Update () {
		GlobVars.DaySpeed = TimeMultiplier*2;
		
		for (int i = 0; i < SSV.profile.components.Count; i++) {
			if(SSV.profile.components[i].name == "ColorAdjustments(Clone)") {
				CA = (ColorAdjustments)SSV.profile.components[i];
			}
			if(SSV.profile.components[i].name == "WhiteBalance(Clone)") {
				WB = (WhiteBalance)SSV.profile.components[i];
			}
		}
		
		GameSpeed = Time.deltaTime * TimeMultiplier;
		
		GlobVars.Days %= 365;
		GameObject.Find("SunAngler").transform.eulerAngles = new Vector3 (0, 90, -15-((Mathf.Sin((-GlobVars.Days+360)*Mathf.Deg2Rad)+1)*23));

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
		
		float Angle = -0.25f * (Count + (GlobVars.Hour * 60));
		
		transform.RotateAround(transform.position, GameObject.Find("SunAngler").transform.right, Angle);
		if (GlobVars.Hour < 18 && GlobVars.Hour >= 6) {
			CA.postExposure.value = Mathf.Lerp (CA.postExposure.value, 0, GameSpeed/10);
			WB.temperature.value = Mathf.Abs(Mathf.Cos((Angle+90)*Mathf.Deg2Rad)*50);
		} else {
			CA.postExposure.value = Mathf.Lerp (CA.postExposure.value, -1, GameSpeed/10);
			WB.temperature.value = -Mathf.Abs(Mathf.Cos((Angle)*Mathf.Deg2Rad)*50);
		}
	}

	void OnGUI () {
		
		string MinsText;
		string HoursText;
		
		if (GlobVars.Mins < 10) {
			MinsText = "0";
		} else {
			MinsText = "";
		}
		
		MinsText += "" + (Mathf.RoundToInt(GlobVars.Mins / 5) * 5);
		
		if (GlobVars.Hour > 12) {
			HoursText = "" + (GlobVars.Hour - 12) + ":" + MinsText + "pm";
		} else {
			HoursText = "" + GlobVars.Hour + ":" + MinsText + "am";
		}
		
		GUI.Label (new Rect(Screen.width - 200, Screen.height - 150, 100, 20), "Day " + (GlobVars.Days+1), style);
		
		GUI.Label (new Rect(Screen.width - 200, Screen.height - 100, 100, 20), HoursText, style);
	}
}