using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour {
	
	public bool AutoInteract;
	public float Range;
	public Vector3 GlobalOffset;
	public string ObjectText;
	public string MessageName;
	public bool KeepInteract;
	
	bool MustLeave;
	
	void Update () {
		
		float Dist = 10000;
		
		float DFP = Vector3.Distance (transform.position+GlobalOffset, GameObject.Find("Player").transform.position);
		
		foreach (Interactables I in GameObject.FindObjectsOfType<Interactables>()) {
			
			DFP = Vector3.Distance (I.transform.position+I.GlobalOffset, GameObject.Find("Player").transform.position);
			
			if (DFP < Dist && DFP <= Range) {
				if (DFP < Dist) {
					Dist = DFP;
				}
			}
		}
		
		DFP = Vector3.Distance (transform.position+GlobalOffset, GameObject.Find("Player").transform.position);
		
		if (Dist == DFP) {
			GlobVars.ClosestInteractable = this.gameObject;
		} else {
			MustLeave = false;
		}

		if (((Dist == DFP || AutoInteract) && DFP <= Range) && !MustLeave) {
			if (!AutoInteract) {
				GlobVars.NearInteractable = true;
				GlobVars.InteractText = ObjectText;
			}
			if ((SSInput.A[0] == "Pressed" || AutoInteract) && !GlobVars.PlayerPaused && !GlobVars.Reading && !GlobVars.Interacting) {
				if (KeepInteract) {
					if (AutoInteract) {
						MustLeave = true;
					}
					GlobVars.InteractObject = this.gameObject;
					GlobVars.Interacting = true;
				}
				SendMessage(MessageName);
			}
		}
	}
	
	public static void StopI () {
		GlobVars.InteractObject = null;
		GlobVars.Interacting = false;
	}
	
	void OnDrawGizmosSelected () {
		Gizmos.DrawWireSphere(transform.position+GlobalOffset, Range);
	}
}
