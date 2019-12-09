using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour {
	
	public AudioClip C;
	public GameObject ObjToInst;
	public static int scale;
	
	public void AudioPlay () {
		GameObject.Find("MainAudio").GetComponent<AudioSource>().clip = C;
		GameObject.Find("MainAudio").GetComponent<AudioSource>().Play();
	}
	
	public void JustInst () {
		for (int i = 0; i < 100; ++i) {
			GameObject G = Instantiate (ObjToInst, transform.position+(Vector3.up*(i*50))+(transform.right*50), Quaternion.Euler (Vector3.zero));
			G.transform.localScale = new Vector3 (10*scale, 10*scale, 10*scale);
			++scale;
		}
	}
}
