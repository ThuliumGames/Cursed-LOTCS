using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemQuickSelect : MonoBehaviour {
	
	public Canvas QSC;
	
	public GameObject Sword;
	public GameObject Shield;
	public GameObject Special;
	
	public Text SwoText;
	public Text ShiText;
	public Text SpeText;
	
	int WantedSword = 1;
	int WantedShield = 1;
	int WantedSpecial = 1;
	
	int SwordIm = 0;
	int ShieldIm = 0;
	int SpecialIm = 0;
	
	void Update () {
		
		int TotSwords = 0;
		int TotShields = 0;
		int TotSpecials = 0;
		
		foreach (anItem I in GameObject.FindObjectOfType<Inventory>().BackItems) {
			if (I.item != null) {
				if (I.item.Type == "Sword") {
					++TotSwords;
				}
				if (I.item.Type == "Shield") {
					++TotShields;
				}
				if (I.item.Type == "Special") {
					++TotSpecials;
				}
			}
		}
		
		foreach (anItem I in GameObject.FindObjectOfType<Inventory>().SideItems) {
			if (I.item != null) {
				if (I.item.Type == "Sword") {
					++TotSwords;
				}
				if (I.item.Type == "Shield") {
					++TotShields;
				}
				if (I.item.Type == "Special") {
					++TotSpecials;
				}
			}
		}
		
		if (TotSwords == 0) {
			Sword.GetComponentInChildren<Image>().color = Color.clear;
			Sword.GetComponentInChildren<Text>().text = "";
			SwoText.text = "";
		} else {
			Sword.GetComponentInChildren<Image>().color = Color.white;
		}
		if (TotShields == 0) {
			Shield.GetComponentInChildren<Image>().color = Color.clear;
			Shield.GetComponentInChildren<Text>().text = "";
			ShiText.text = "";
		} else {
			Shield.GetComponentInChildren<Image>().color = Color.white;
		}
		if (TotSpecials == 0) {
			Special.GetComponentInChildren<Image>().color = Color.clear;
			Special.GetComponentInChildren<Text>().text = "";
			SpeText.text = "";
		} else {
			Special.GetComponentInChildren<Image>().color = Color.white;
		}
		
		if (!GlobVars.PlayerPaused && !GlobVars.Reading) {
			
			bool AnyDPress = false;
			
			if ((SSInput.DUp[0] == "Pressed" || SSInput.DDown[0] == "Pressed" || SSInput.DLeft[0] == "Pressed" || SSInput.DRight[0] == "Pressed")
				||
				(SSInput.DUp[0] == "Down" || SSInput.DDown[0] == "Down" || SSInput.DLeft[0] == "Down" || SSInput.DRight[0] == "Down")) {
				AnyDPress = true;
			}
			
			if (AnyDPress) {
				QSC.enabled = true;
				
				//Display Item
				SwordIm = 0;
				ShieldIm = 0;
				SpecialIm = 0;
				
				foreach (anItem I in GameObject.FindObjectOfType<Inventory>().BackItems) {
					Compare (I);
				}
				
				foreach (anItem I in GameObject.FindObjectOfType<Inventory>().SideItems) {
					Compare (I);
				}
				
				if (SSInput.DUp[0] == "Pressed") {
					++WantedSpecial;
					if (WantedSpecial > TotSpecials) {
						WantedSpecial = 1;
					}
				}
				if (SSInput.DLeft[0] == "Pressed") {
					++WantedShield;
					if (WantedShield > TotShields) {
						WantedShield = 1;
					}
				}
				if (SSInput.DRight[0] == "Pressed") {
					++WantedSword;
					if (WantedSword > TotSwords) {
						WantedSword = 1;
					}
				}
			}
			
			if (SSInput.DDown[0] == "Released" && !AnyDPress) {
				QSC.enabled = false;
			}
			
		} else {
			QSC.enabled = false;
		}
	}
	
	void Compare (anItem I) {
		if (I.item != null) {
			if (I.item.Type == "Sword") {
				++SwordIm;
				if (SwordIm == WantedSword) {
					Sword.GetComponentInChildren<Image>().sprite = I.item.sprite;
					Sword.GetComponentInChildren<Text>().text = ""+I.Amount;
					SwoText.text = I.item.name;
				}
			}
			if (I.item.Type == "Shield") {
				++ShieldIm;
				if (ShieldIm == WantedShield) {
					Shield.GetComponentInChildren<Image>().sprite = I.item.sprite;
					Shield.GetComponentInChildren<Text>().text = ""+I.Amount;
					ShiText.text = I.item.name;
				}
			}
			if (I.item.Type == "Special") {
				++SpecialIm;
				if (SpecialIm == WantedSpecial) {
					Special.GetComponentInChildren<Image>().sprite = I.item.sprite;
					Special.GetComponentInChildren<Text>().text = ""+I.Amount;
					SpeText.text = I.item.name;
				}
			}
		}
	}
}
