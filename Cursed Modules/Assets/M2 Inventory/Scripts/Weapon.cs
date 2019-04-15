﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemObject {
	
	public Vector3 InHandPosition;
	public Vector3 InHandRotation;
	
	public AnimatorOverrideController AOC;
	
	float T = 0;
	
	void LateUpdate () {
		GetComponentInParent<Animator>().runtimeAnimatorController = AOC;
	}
}
