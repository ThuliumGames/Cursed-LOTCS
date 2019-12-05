using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	
	public GameObject ObjToSpawn;
	public int Num;
	public float Range;
	
	void Start () {
		for (int i = 0; i < Num; i++) {
			GameObject G = Instantiate (ObjToSpawn, transform.position + new Vector3 (Random.Range(-Range, Range), 0, Random.Range(-Range, Range)), Quaternion.Euler(Vector3.zero));
		}
		Destroy (GetComponentInParent<OnlyLoadIfClose>().gameObject);
	}
}