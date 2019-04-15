using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItems : MonoBehaviour {
	
	void Update () {
		if (GlobVars.EquippedWeapon != null) {
			if (GetComponentInChildren<Weapon>() == null) {
				GameObject G = Instantiate (GlobVars.EquippedWeapon.Model);
				G.transform.SetParent(GameObject.Find ("hand.L").transform);
				G.transform.localPosition = G.GetComponent<Weapon>().InHandPosition;
				G.transform.localEulerAngles = G.GetComponent<Weapon>().InHandRotation;
				Destroy (G.GetComponent<Interactables>());
			} else if (GetComponentInChildren<Weapon>().AssociatedItem != GlobVars.EquippedWeapon) {
				Destroy (GetComponentInChildren<Weapon>().gameObject);
				GameObject G = Instantiate (GlobVars.EquippedWeapon.Model);
				G.transform.SetParent(GameObject.Find ("hand.L").transform);
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
