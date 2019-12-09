using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	
	//TalkPre
	public Canvas DialogueCanvas;
	public Image BackgroundImage;
	public Text RegWrite;
	public Text AnsWrite;
	public Image[] AnsBox;
	public Text NameWrite;
	public GameObject GoToNext;
	//Time
	public static int Days = 0;
	public static int Hour;
	public static int Mins;
	public static float DaySpeed;
	
	public static Item EquippedWeapon;
	public static Item EquippedSpecial;
	public static Item EquippedShield;
	
	public static int PlayingChar;
	public static GameObject CurrentPlayer;
	
	public GameObject[] P1Ena;
	public GameObject[] P2Ena;
	int was;
	
	public void Update () {
		if (was == PlayingChar) {
			if (PlayingChar == 0) {
				if (P2Ena[0].activeSelf) {
					
					System.Reflection.FieldInfo[] fields = P1Ena[0].GetComponent<Health>().GetType().GetFields(); 
					
					foreach (System.Reflection.FieldInfo field in fields) {
						field.SetValue(P1Ena[0].GetComponent<Health>(), field.GetValue(P2Ena[1].GetComponent<Health>()));
						field.SetValue(P1Ena[1].GetComponent<Health>(), field.GetValue(P2Ena[0].GetComponent<Health>()));
					}
				}
				
				P1Ena[0].SetActive(true);
				P1Ena[1].SetActive(true);
				P2Ena[0].SetActive(false);
				P2Ena[1].SetActive(false);
				
				P1Ena[0].GetComponent<Health>().isPlayer = true;
				P2Ena[0].GetComponent<Health>().isPlayer = true;
				P1Ena[1].GetComponent<Health>().isPlayer = false;
				P2Ena[1].GetComponent<Health>().isPlayer = false;
				
				CurrentPlayer = P1Ena[0];
				
			} else if (PlayingChar == 2) {
				if (P1Ena[0].activeSelf) {
					
					System.Reflection.FieldInfo[] fields = P2Ena[0].GetComponent<Health>().GetType().GetFields(); 
					
					foreach (System.Reflection.FieldInfo field in fields) {
						field.SetValue(P2Ena[0].GetComponent<Health>(), field.GetValue(P1Ena[1].GetComponent<Health>()));
						field.SetValue(P2Ena[1].GetComponent<Health>(), field.GetValue(P1Ena[0].GetComponent<Health>()));
					}
				}
				
				P2Ena[0].SetActive(true);
				P2Ena[1].SetActive(true);
				P1Ena[0].SetActive(false);
				P1Ena[1].SetActive(false);
				
				P1Ena[0].GetComponent<Health>().isPlayer = true;
				P2Ena[0].GetComponent<Health>().isPlayer = true;
				P1Ena[1].GetComponent<Health>().isPlayer = false;
				P2Ena[1].GetComponent<Health>().isPlayer = false;
				
				CurrentPlayer = P2Ena[0];
			}
		}
		
		if (PlayingChar > was) {
			was++;
		} else if (PlayingChar < was) {
			was--;
		}
	}
	
	public void Player (int P) {
		PlayingChar = P;
	}
}
