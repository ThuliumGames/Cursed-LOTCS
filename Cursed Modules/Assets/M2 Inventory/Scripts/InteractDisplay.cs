using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractDisplay : MonoBehaviour {
	
	public Canvas Disp;
	
	void Update () {
		if (GlobVars.NearInteractable) {
			Disp.enabled = true;
			Disp.GetComponentInChildren<Text>().text = GlobVars.InteractText;
		} else {
			Disp.enabled = false;
		}
		GlobVars.NearInteractable = false;
	}
}
