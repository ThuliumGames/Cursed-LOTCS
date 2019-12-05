using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItems : MonoBehaviour {
	
	public Transform Hand;
	public bool usingSpecial;
	
	string Slt = "Weapon";
	
	void Update () {
		
		if (GlobVars.EquippedSpecial != null) {
			if (SSInput.RB[0] == "Released") {
				usingSpecial = true;
			}
		} else {
			usingSpecial = false;
		}
		
		if (SSInput.X[0] == "Released") {
			usingSpecial = false;
		}
		
		if (usingSpecial) {
			Slt = "Special";
		} else {
			Slt = "Weapon";
		}
		
		if ((Item)GameObject.FindObjectOfType<GlobVars>().GetType().GetField("Equipped" + Slt).GetValue(GameObject.FindObjectOfType<GlobVars>()) != null) {
			if (GetComponentInChildren<Weapon>() == null) {
				GameObject G = Instantiate (((Item)GameObject.FindObjectOfType<GlobVars>().GetType().GetField("Equipped" + Slt).GetValue(GameObject.FindObjectOfType<GlobVars>())).Model);
				G.transform.SetParent(Hand);
				G.transform.localPosition = G.GetComponent<Weapon>().InHandPosition;
				G.transform.localEulerAngles = G.GetComponent<Weapon>().InHandRotation;
				Destroy (G.GetComponent<Interactables>());
			} else if (GetComponentInChildren<Weapon>().AssociatedItem != (Item)GameObject.FindObjectOfType<GlobVars>().GetType().GetField("Equipped" + Slt).GetValue(GameObject.FindObjectOfType<GlobVars>())) {
				Destroy (GetComponentInChildren<Weapon>().gameObject);
				GameObject G = Instantiate (((Item)GameObject.FindObjectOfType<GlobVars>().GetType().GetField("Equipped" + Slt).GetValue(GameObject.FindObjectOfType<GlobVars>())).Model);
				G.transform.SetParent(Hand);
				G.transform.localPosition = G.GetComponent<Weapon>().InHandPosition;
				G.transform.localEulerAngles = G.GetComponent<Weapon>().InHandRotation;
				Destroy (G.GetComponent<Interactables>());
			}
		} else {
			if (GetComponentInChildren<Weapon>() != null) {
				Destroy (GetComponentInChildren<Weapon>().gameObject);
			}
		}
	}
}
