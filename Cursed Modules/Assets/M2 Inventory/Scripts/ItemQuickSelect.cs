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
	
	int WantedSword = 0;
	int WantedShield = 0;
	int WantedSpecial = 0;
	
	int SwordIm = 0;
	int ShieldIm = 0;
	int SpecialIm = 0;
	
	public SVGImage DPad;
	public Sprite[] AltSprites;
	
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
			SwoText.text = "Fist";
			GlobVars.EquippedWeapon = null;
		} else {
			Sword.GetComponentInChildren<Image>().color = Color.white;
		}
		if (TotShields == 0) {
			Shield.GetComponentInChildren<Image>().color = Color.clear;
			Shield.GetComponentInChildren<Text>().text = "";
			ShiText.text = "None";
			GlobVars.EquippedShield = null;
		} else {
			Shield.GetComponentInChildren<Image>().color = Color.white;
		}
		if (TotSpecials == 0) {
			Special.GetComponentInChildren<Image>().color = Color.clear;
			Special.GetComponentInChildren<Text>().text = "";
			SpeText.text = "Fist";
			GlobVars.EquippedSpecial = null;
		} else {
			Special.GetComponentInChildren<Image>().color = Color.white;
		}
		
		if (!GlobVars.PlayerPaused && !GlobVars.Reading) {
			
			bool AnyDPress = false;
			
			if ((SSInput.DUp[0] == "Pressed" || SSInput.DDown[0] == "Pressed" || SSInput.DLeft[0] == "Pressed" || SSInput.DRight[0] == "Pressed")
				||
				(SSInput.DUp[0] == "Down" || SSInput.DDown[0] == "Down" || SSInput.DLeft[0] == "Down" || SSInput.DRight[0] == "Down")) {
				AnyDPress = true;
			} else {
				DPad.sprite = AltSprites[0];
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
					DPad.sprite = AltSprites[1];
					++WantedSpecial;
					if (WantedSpecial > TotSpecials) {
						WantedSpecial = 0;
					}
				}
				if (SSInput.DLeft[0] == "Pressed") {
					DPad.sprite = AltSprites[2];
					++WantedShield;
					if (WantedShield > TotShields) {
						WantedShield = 0;
					}
				}
				if (SSInput.DRight[0] == "Pressed") {
					DPad.sprite = AltSprites[3];
					++WantedSword;
					if (WantedSword > TotSwords) {
						WantedSword = 0;
					}
				}
				
				if (SSInput.DDown[0] == "Pressed") {
					DPad.sprite = AltSprites[4];
				}
			}
			
			if (SSInput.DDown[0] == "Released" && !AnyDPress) {
				QSC.enabled = false;
			}
			
		} else {
			QSC.enabled = false;
		}
		
		if (WantedSword == 0) {
			GlobVars.EquippedWeapon = null;
			Sword.GetComponentInChildren<Image>().color = Color.clear;
			Sword.GetComponentInChildren<Text>().text = "";
			SwoText.text = "Fist";
		}
		
		if (WantedShield == 0) {
			GlobVars.EquippedShield = null;
			Shield.GetComponentInChildren<Image>().color = Color.clear;
			Shield.GetComponentInChildren<Text>().text = "";
			ShiText.text = "None";
		}
		
		if (WantedSpecial == 0) {
			GlobVars.EquippedSpecial = null;
			Special.GetComponentInChildren<Image>().color = Color.clear;
			Special.GetComponentInChildren<Text>().text = "";
			SpeText.text = "Fist";
		}
		
	}
	
	void Compare (anItem I) {
		if (I.item != null) {
			if (I.item.Type == "Sword") {
				++SwordIm;
				if (WantedSword != 0) {
					if (SwordIm == WantedSword) {
						Sword.GetComponentInChildren<Image>().sprite = I.item.sprite;
						Sword.GetComponentInChildren<Text>().text = ""+I.Amount;
						SwoText.text = I.item.name;
						GlobVars.EquippedWeapon = I.item;
					}
				}
			}
			if (I.item.Type == "Shield") {
				++ShieldIm;
				if (WantedShield != 0) {
					if (ShieldIm == WantedShield) {
						Shield.GetComponentInChildren<Image>().sprite = I.item.sprite;
						Shield.GetComponentInChildren<Text>().text = ""+I.Amount;
						ShiText.text = I.item.name;
						GlobVars.EquippedShield = I.item;
					}
				}
			}
			if (I.item.Type == "Special") {
				++SpecialIm;
				if (WantedSpecial != 0) {
					if (SpecialIm == WantedSpecial) {
						Special.GetComponentInChildren<Image>().sprite = I.item.sprite;
						Special.GetComponentInChildren<Text>().text = ""+I.Amount;
						SpeText.text = I.item.name;
						GlobVars.EquippedSpecial = I.item;
					}
				}
			}
		}
	}
}
