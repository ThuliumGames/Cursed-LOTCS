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
	
	void Start () {
		GlobalOffset = (((transform.right).normalized*GlobalOffset.x) + ((transform.up).normalized*GlobalOffset.y) + ((transform.forward).normalized*GlobalOffset.z));
	}
	
	void Update () {
		
		if (Vector3.Distance (transform.position+GlobalOffset, GlobVars.CurrentPlayer.transform.position) <= Range) {
			TryInteract();
		}
	}
	
	void TryInteract () {
		
		float Dist = 10000;
		
		float DFP = Vector3.Distance (transform.position+GlobalOffset, GlobVars.CurrentPlayer.transform.position);
		
		foreach (Interactables I in GameObject.FindObjectsOfType<Interactables>()) {
			
			DFP = Vector3.Distance (I.transform.position+I.GlobalOffset, GlobVars.CurrentPlayer.transform.position);
			
			if (DFP < Dist && DFP <= Range) {
				if (DFP < Dist) {
					Dist = DFP;
				}
			}
		}
		
		DFP = Vector3.Distance (transform.position+GlobalOffset, GlobVars.CurrentPlayer.transform.position);
		
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
		if (Application.isPlaying) {
			Gizmos.DrawWireSphere(transform.position + GlobalOffset, Range);
		} else {
			Gizmos.DrawWireSphere(transform.position + (((transform.right).normalized*GlobalOffset.x) + ((transform.up).normalized*GlobalOffset.y) + ((transform.forward).normalized*GlobalOffset.z)), Range);
		}
	}
}
