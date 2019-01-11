using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesFollow : MonoBehaviour {
	
	public SkinnedMeshRenderer Player;
	public SkinnedMeshRenderer[] Clothes;
	
	void Update () {
		
		foreach (SkinnedMeshRenderer T in Clothes) {
			T.bones = Player.bones;
		}
	}
}
