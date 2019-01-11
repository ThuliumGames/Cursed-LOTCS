using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class anItem {
	public Item item;
	public int Amount;
}

public class Inventory : MonoBehaviour {
	
	public List<Item> Items;
	public anItem[] BackItems;
	public anItem[] SideItems;
	public anItem[] BackpackItems;
	public anItem[] ArmorL;
	public anItem[] ArmorC;
	public anItem[] ArmorM;
	
	bool Placed = false;
	
	int z = 0;
	
	void Update () {
		
		for (int i = 0; i < BackItems.Length; ++i) {
			BackItems[i].item = null;
			BackItems[i].Amount = 0;
		}
		
		for (int i = 0; i < SideItems.Length; ++i) {
			SideItems[i].item = null;
			SideItems[i].Amount = 0;
		}
		
		for (int i = 0; i < BackpackItems.Length; ++i) {
			BackpackItems[i].item = null;
			BackpackItems[i].Amount = 0;
		}
		
		for (int i = 0; i < ArmorL.Length; ++i) {
			ArmorL[i].item = null;
			ArmorL[i].Amount = 0;
		}
		
		for (int i = 0; i < ArmorC.Length; ++i) {
			ArmorC[i].item = null;
			ArmorC[i].Amount = 0;
		}
		
		for (int i = 0; i < ArmorM.Length; ++i) {
			ArmorM[i].item = null;
			ArmorM[i].Amount = 0;
		}
		
		int b = 0;
		
		bool HasSabre = false;
		int SabrePlace;
		Item Temp = null;
		
		foreach (Item I in Items) {
			if (I.name == "Crystal Sabre") {
				HasSabre = true;
				SabrePlace = b;
				Temp = I;
			}
			if (!HasSabre) {
				++b;
			}
		}
		
		if (HasSabre) {
			Items.RemoveAt(b);
			Items.Insert(0, Temp);
		}
		
		b = 0;
		
		foreach (Item I in Items) {
			Placed = false;
			
			if (I != null) {
				
				if (!I.CanBeEquipedIn[3]) {
					
					if (I.CanBeEquipedIn[0]) {
						if (!Placed)
							foreach (anItem BI in BackItems) { Sorting (BI, I); }
					}
					
					if (I.CanBeEquipedIn[1]) {
						if (!Placed)
							foreach (anItem SI in SideItems) { Sorting (SI, I); }
					}
					
					if (I.CanBeEquipedIn[2]) {
						if (!Placed)
							foreach (anItem BPI in BackpackItems) { Sorting (BPI, I); }
					}
				} else {
					if (I.CanBeEquipedIn[0]) {
						if (!Placed)
							foreach (anItem AL in ArmorL) { Sorting (AL, I); }
					}
					
					if (I.CanBeEquipedIn[1]) {
						if (!Placed)
							foreach (anItem AC in ArmorC) { Sorting (AC, I); }
					}
					
					if (I.CanBeEquipedIn[2]) {
						if (!Placed)
							foreach (anItem AM in ArmorM) { Sorting (AM, I); }
					}
				}
				if (!Placed) {
					GameObject G = Instantiate (I.Model, GameObject.Find("Player").transform.position, Quaternion.identity);
					G.AddComponent<Rigidbody>();
				}
			}
			++b;
		}
		
		Items.Clear();
		
		z = 0;
		foreach (anItem It in BackItems) {
			Insert (It);
		}
		foreach (anItem It in SideItems) {
			Insert (It);
		}
		foreach (anItem It in BackpackItems) {
			Insert (It);
		}
		foreach (anItem It in ArmorL) {
			Insert (It);
		}
		foreach (anItem It in ArmorC) {
			Insert (It);
		}
		foreach (anItem It in ArmorM) {
			Insert (It);
		}
	}
	
	void Sorting (anItem AI, Item It) {
		if (!Placed) {
			if (AI.item == null || (It == AI.item && AI.Amount < It.StackSize)) {
				Placed = true;
				AI.item = It;
				++AI.Amount;
			}
		}
	}
	
	void Insert (anItem It) {
		if (It.item != null) {
			for (int i = 0; i < It.Amount; ++i) {
				Items.Insert(z, It.item);
				++z;
			}
		}
	}
}
