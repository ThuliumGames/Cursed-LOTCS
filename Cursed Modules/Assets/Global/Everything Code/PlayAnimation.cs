using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour {
	
	public Animator Anim;
	public bool doVarSwitch;
	public string Play;
	
	public void PlayA () {
		if (!doVarSwitch) {
			Anim.Play (Play);
		} else {
			Anim.SetBool (Play, !Anim.GetBool (Play));
		}
	}
}
