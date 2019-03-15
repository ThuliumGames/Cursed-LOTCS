using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class weather_zones {
	public float Size;
	public Vector3 Pos;
	public Vector2 ZoneTempRange;
	public Vector2 ZoneCloudRange;
	public bool LockWeatherType;
	public enum LockedWeather {Clear, Cloud, Fog, Rain, Storm, Thunder, Snow, Blizzard};
	public LockedWeather LockWeatherTo;

}

public class NewWeather : MonoBehaviour {
	public Transform Player;
	public weather_zones[] WeatherZones;
	float Temp;
	float CloudLevel;
	void Start () {

	}
	

	void Update () {
		Vector3 RelativePoint = transform.InverseTransformPoint(Player.position);
		foreach (weather_zones Zone in WeatherZones) {
			if (Vector3.Distance(Player.position, Zone.Pos) > Zone.Size) {
				print ("Yes");
			} else {
				print ("no");
			}
		}
		
	}

	void OnDrawGizmos () {
		foreach (weather_zones Zone in WeatherZones) {
			Gizmos.DrawWireSphere (Zone.Pos, Zone.Size);
		}
	}
}