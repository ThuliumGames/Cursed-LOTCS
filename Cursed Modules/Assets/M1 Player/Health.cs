using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
	
	public float Wound;
	
	public float BaseResistance = 5;
	public float RecoveryTime;
	
	string[] ArmorTypes = {"L", "C", "M"};
	ItemObject[] ArmorObject = {null, null, null};
	
	public int HurtLayer;
	
	float T;
	
	bool CanChange;
	
	public bool isPlayer;
	public Image[] Graphics;
	
	float invulnTimer;
	
	void Update () {
		
		//SetUp Armor
		for (int i = 0; i < 3; i++) {
			foreach (ItemObject IO in GetComponentsInChildren<ItemObject>()) {
				if (IO.AssociatedItem.Type == ArmorTypes[i] + "Armor") {
					ArmorObject[i] = IO;
				}
			}
			
			if (ArmorObject[i] != null) {
				if (ArmorObject[i].ProgNum <= 0) {
					Destroy(ArmorObject[i].gameObject);
				}
			}
		}
		//
		
		//Change Wounds
		if (Wound >= 11) {
			SendMessage("Die");
		}
		if (Wound != 0) {
			T += Time.deltaTime;
			
			if (Wound >= 5) {
				if (T >= RecoveryTime) {
					++Wound;
					T = 0;
				}
			} else {
				if (T >= RecoveryTime) {
					--Wound;
					T = 0;
				}
			}
		}
		//
		
		//Display Health
		if (isPlayer) {

			Graphics[0].fillAmount = (-(Wound/10))+1;
			
			for (int i = 0; i < 3; i++) {
				if (ArmorObject[i] != null) {
					Graphics[i+1].fillAmount = ArmorObject[i].ProgNum/ArmorObject[i].AssociatedItem.ProgNum;
				} else {
					Graphics[i+1].fillAmount = 0;
				}
			}
		}
		//
		
		invulnTimer -= Time.deltaTime;
		invulnTimer = Mathf.Clamp01 (invulnTimer);
	}
	
	void OnTriggerEnter (Collider Other) {
		//Damage
		if (Other.gameObject.layer == HurtLayer && Other.GetComponentInParent<ItemObject>() != null && invulnTimer <= 0) {
			invulnTimer = 1;
			if ((ArmorObject[0] == null && ArmorObject[1] == null && ArmorObject[2] == null)) {
				Wound += (Other.GetComponentInParent<ItemObject>().AssociatedItem.ProgNum/BaseResistance);
			} else {
				if (ArmorObject[2] != null) {
					ArmorObject[2].ProgNum -= Other.GetComponentInParent<ItemObject>().AssociatedItem.ProgNum/BaseResistance;
					ArmorObject[2].ProgNum = (int)ArmorObject[2].ProgNum;
				} else if (ArmorObject[1] != null) {
					ArmorObject[1].ProgNum -= Other.GetComponentInParent<ItemObject>().AssociatedItem.ProgNum/BaseResistance;
					ArmorObject[1].ProgNum = (int)ArmorObject[1].ProgNum;
				} else {
					ArmorObject[0].ProgNum -= Other.GetComponentInParent<ItemObject>().AssociatedItem.ProgNum/BaseResistance;
					ArmorObject[0].ProgNum = (int)ArmorObject[0].ProgNum;
				}
			}
			Wound = (int)Wound;
		}
	}
}
