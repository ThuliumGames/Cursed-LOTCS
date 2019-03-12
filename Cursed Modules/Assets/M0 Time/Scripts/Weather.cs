using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Types {
	public string WeatherType;
	public float Probability;
	//public Vector3[] WeatherPos;
}
public class Weather : MonoBehaviour {

	public Types[] WeatherTypes;
	public Vector3 ZoneSize;
	public Transform Player;
	int RandomChance;
	float TotalChance;
	


	void Start () {
		for (int i = 0; i < WeatherTypes.Length; i++) {
			TotalChance += WeatherTypes[i].Probability;
		}
		RandomChance = (int) Random.Range(0, TotalChance);
	}
	

	void Update () {
		print (TotalChance);
		Vector3 RelativePoint = transform.InverseTransformPoint(Player.position);
		if (RelativePoint.z < ZoneSize.z / 2 && RelativePoint.z > -ZoneSize.z / 2 && RelativePoint.x < ZoneSize.x / 2 && RelativePoint.x > -ZoneSize.x / 2) {
			print ("is");
		}
		System.Array.Sort (WeatherTypes, delegate(Types A, Types B) {return B.Probability.CompareTo (A.Probability);});
	}

	void OnDrawGizmos () {
		Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.matrix = rotationMatrix;
		Gizmos.DrawWireCube (transform.position, ZoneSize);
	}
}
