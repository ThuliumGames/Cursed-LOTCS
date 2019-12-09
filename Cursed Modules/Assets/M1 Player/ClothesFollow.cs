using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesFollow : MonoBehaviour {
	
	public Transform[] Player;
	public Vector3 Offset;
	public string[] Relations;
	public SkinnedMeshRenderer[] Clothes;
	
	void Update () {
		
		transform.position = Player[GlobVars.PlayingChar/2].position+(Player[GlobVars.PlayingChar/2].right*Offset.x)+(Player[GlobVars.PlayingChar/2].up*Offset.y)+(Player[GlobVars.PlayingChar/2].forward*Offset.z);
		Vector3 TmpV3 = new Vector3 (0, 0, 0);
		
		
		if (Relations[0] == "x") {
			TmpV3.x = Player[GlobVars.PlayingChar/2].eulerAngles.x;
		}
		if (Relations[1] == "x") {
			TmpV3.y = Player[GlobVars.PlayingChar/2].eulerAngles.x;
		}
		if (Relations[2] == "x") {
			TmpV3.z = Player[GlobVars.PlayingChar/2].eulerAngles.x;
		}
		
		
		if (Relations[0] == "y") {
			TmpV3.x = Player[GlobVars.PlayingChar/2].eulerAngles.y;
		}
		if (Relations[1] == "y") {
			TmpV3.y = Player[GlobVars.PlayingChar/2].eulerAngles.y;
		}
		if (Relations[2] == "y") {
			TmpV3.z = Player[GlobVars.PlayingChar/2].eulerAngles.y;
		}
		
		
		if (Relations[0] == "z") {
			TmpV3.x = Player[GlobVars.PlayingChar/2].eulerAngles.z;
		}
		if (Relations[1] == "z") {
			TmpV3.y = Player[GlobVars.PlayingChar/2].eulerAngles.z;
		}
		if (Relations[2] == "z") {
			TmpV3.z = Player[GlobVars.PlayingChar/2].eulerAngles.z;
		}
		
		
		transform.eulerAngles = TmpV3;
		
		if (Clothes.Length > 0) {
			foreach (SkinnedMeshRenderer T in Clothes) {
				T.bones = Player[GlobVars.PlayingChar/2].GetComponent<SkinnedMeshRenderer>().bones;
			}
		}
	}
}
