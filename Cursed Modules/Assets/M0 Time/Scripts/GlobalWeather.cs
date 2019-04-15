using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalWeather : MonoBehaviour {
	
	public static float temperature;
	public static float windDirection;
	public static float windSpeed;
	public static float precipitationLevel;
	
	public static int season;
	public string[] SNames = {"Winter", "Spring", "Summer", "Autumn"};
	public int[] SeasonDate = {79, 172, 266, 355};
	
	public GUIStyle style;
	
	void Update () {
		if (GlobVars.Days >= SeasonDate[season] && GlobVars.Days < SeasonDate[season]+2f) {
			season++;
		}
		season %= 4;
		
		if (!GlobVars.Paused) {
			
		}
	}
	
	void OnGUI () {
		GUI.Label (new Rect(100, Screen.height - 100, 100, 20), SNames[season], style);
	}
}
