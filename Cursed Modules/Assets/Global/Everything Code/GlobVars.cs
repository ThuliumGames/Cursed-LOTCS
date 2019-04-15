using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobVars : MonoBehaviour {
	
	//Pause Game
	public static bool Paused;
	//Pause The Movement of the player but NOT the Game
	public static bool PlayerPaused;
	public static bool Reading;
	//Quests
	public static string[] Quests = {};
	public static string[] DoneQuests = {};
	//Interact
	public static GameObject InteractObject;
	public static GameObject ClosestInteractable;
	public static bool Interacting;
	public static bool NearInteractable;
	public static string InteractText;
	//Time
	public static int Days = 0;
	public static int Hour;
	public static int Mins;
	
	public static Item EquippedWeapon;
	public static Item EquippedSpecial;
	public static Item EquippedShield;
}
