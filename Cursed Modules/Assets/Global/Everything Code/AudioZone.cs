using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour {
	
	public static bool inZone;
	
	public bool isMaster;
	
	public bool Rectangular;
	
	[Header("Only Use X For Spherical")]
	public Vector3 Size;
	
	public float SwitchSpeed = 1;
	
	Transform Player;
	
	void Update () {
		
		Player = GlobVars.CurrentPlayer.transform;
		
		if (!isMaster) {
			Vector3 Tmp = transform.InverseTransformPoint(Player.position);
			bool NoOther = true;
			foreach (AudioZone AZ in GameObject.FindObjectsOfType<AudioZone>()) {
				if (AZ != this && !AZ.isMaster && AZ.GetComponent<AudioSource>().isPlaying) {
					NoOther = false;
				}
			}
			if (((Vector3.Distance (Player.position, transform.position) < Size.x && !Rectangular) || (Mathf.Abs (Tmp.x) < Size.x/2 && Mathf.Abs (Tmp.y) < Size.y/2 && Mathf.Abs (Tmp.z) < Size.z/2 && Rectangular)) && NoOther) {
				inZone = true;
				if (!GetComponent<AudioSource>().isPlaying) {
					GetComponent<AudioSource>().Play();
				}
				GetComponent<AudioSource>().volume = Mathf.Lerp (GetComponent<AudioSource>().volume, 1, SwitchSpeed*Time.deltaTime);
			} else {
				GetComponent<AudioSource>().volume = Mathf.Lerp (GetComponent<AudioSource>().volume, 0, SwitchSpeed*Time.deltaTime);
				if (GetComponent<AudioSource>().volume < 0.1f) {
					GetComponent<AudioSource>().Stop();
				}
			}
		}
	}
	
	void LateUpdate () {
		if (isMaster) {
			if (inZone) {
				GetComponent<AudioSource>().volume = Mathf.Lerp (GetComponent<AudioSource>().volume, 0, 5*Time.deltaTime);
				if (GetComponent<AudioSource>().volume < 0.1f) {
					GetComponent<AudioSource>().Stop();
				}
			} else {
				if (!GetComponent<AudioSource>().isPlaying) {
					GetComponent<AudioSource>().Play();
				}
				GetComponent<AudioSource>().volume = Mathf.Lerp (GetComponent<AudioSource>().volume, 1, Time.deltaTime);
			}
			inZone = false;
		}
	}
	
	void OnDrawGizmosSelected () {
		Gizmos.color = Color.yellow;
		if (Rectangular) {
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Size);
		} else {
			Gizmos.DrawWireSphere(transform.position, Size.x);
		}
	}
}
