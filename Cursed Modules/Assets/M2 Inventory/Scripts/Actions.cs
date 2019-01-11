using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour {
	
	Inventory In;
	
	void Start () {
		In = GameObject.Find("Inventory").GetComponent<Inventory>();
	}
	
	void Drop (int position) {
		GameObject G = Instantiate (In.Items[position].Model, GameObject.Find("Player").transform.position, Quaternion.identity);
		G.AddComponent<Rigidbody>();
		In.Items.RemoveAt(position);
	}
	
	void DropOne (int position) {
		Drop (position);
	}
	
	void DropAll (int position) {
		
		Item n = In.Items[position];
		int AmountFound = 0;
		
		for (int i = In.Items.Count-1; i >= 0; --i) {
			if (In.Items[i] == n && AmountFound < n.StackSize) {
				GameObject G = Instantiate (In.Items[i].Model, GameObject.Find("Player").transform.position+new Vector3 (0, AmountFound, 0), Quaternion.identity);
				G.AddComponent<Rigidbody>();
				In.Items.RemoveAt(i);
				++AmountFound;
			}
		}
	}
}
