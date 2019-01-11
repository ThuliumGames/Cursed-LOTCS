using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	
	public float Wound;
	
	public float BaseResistance = 5;
	
	public LayerMask HurtLayer;
	
	float T;
	
	void Update () {
		if (Wound >= 11) {
			SendMessage("Die");
		}
		if (Wound != 0) {
			T += Time.deltaTime;
			
			if (Wound >= 5) {
				if (T >= 60) {
					++Wound;
					T = 0;
				}
			} else {
				if (T >= 60) {
					--Wound;
					T = 0;
				}
			}
		}
	}
	
	void OnTriggerEnter (Collider Other) {
		if (Other.gameObject.layer == HurtLayer && Other.GetComponent<ItemObject>()) {
			Wound += Other.GetComponent<ItemObject>().AssociatedItem.ProgNum/BaseResistance;
			Wound = (int)Wound;
		}
	}
}
