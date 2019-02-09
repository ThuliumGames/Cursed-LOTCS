using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	
	public int Place = 0;
	public int Slot = 0;
	
	Inventory DI;
	
	public Transform MenuParent;
	public GameObject[] Menu;
	
	public static int InvenNum;
	
	bool inside;
	
	void Start () {
		DI = GetComponentInParent<DisplayInventory>().Inv;
	}
	
	void Update () {
		if (SSInput.LB[0] == "Pressed" || SSInput.RB[0] == "Pressed" || SSInput.Strt[0] == "Pressed") {
			MenuParent.gameObject.SetActive(false);
		}
		
		if (SSInput.A[0] == "Pressed" && inside && (GetComponentInChildren<Image>().sprite != null || Place >= 6)) {
			Click();
		}
		GameObject.Find ("Item Name").transform.localPosition = Input.mousePosition-new Vector3(Screen.width/2, (Screen.height/2)-100, 0);
	}
	
	public void OnPointerEnter (PointerEventData eventData) {
		inside = true;
		
		if (Place < 6) {
			
			if (Place == 0 && DI.BackItems[Slot].item != null) {
				GameObject.Find ("Item Name").GetComponent<Text>().text = DI.BackItems[Slot].item.name;
			}
			
			if (Place == 1 && DI.SideItems[Slot].item != null) {
				GameObject.Find ("Item Name").GetComponent<Text>().text = DI.SideItems[Slot].item.name;
			}
			
			if (Place == 2 && DI.BackpackItems[Slot].item != null) {
				GameObject.Find ("Item Name").GetComponent<Text>().text = DI.BackpackItems[Slot].item.name;
			}
			
			if (Place == 3 && DI.ArmorL[Slot].item != null) {
				GameObject.Find ("Item Name").GetComponent<Text>().text = DI.ArmorL[Slot].item.name;
			}
			
			if (Place == 4 && DI.ArmorC[Slot].item != null) {
				GameObject.Find ("Item Name").GetComponent<Text>().text = DI.ArmorC[Slot].item.name;
			}
			
			if (Place == 5 && DI.ArmorM[Slot].item != null) {
				GameObject.Find ("Item Name").GetComponent<Text>().text = DI.ArmorM[Slot].item.name;
			}
			
		} else {
			GameObject.Find ("Item Name").GetComponent<Text>().text = "";
		}
	}
	
	public void OnPointerExit (PointerEventData eventData) {
		inside = false;
	}
	
	public void Click () {
		
		if (Place < 6) {
			foreach (GameObject G in Menu) {
				G.SetActive(true);
				G.GetComponentInChildren<Text>().text = "Option";
			}
		}
		
		int BIA = DI.BackItems[0].Amount+DI.BackItems[1].Amount+DI.BackItems[2].Amount+DI.BackItems[3].Amount;
		int SIA = DI.SideItems[0].Amount+DI.SideItems[1].Amount+DI.SideItems[2].Amount+DI.SideItems[3].Amount;
		
		int BPIA = 0;
		for (int i = 0; i < 10; ++i) {
			BPIA += DI.BackpackItems[i].Amount;
		}
		
		int LAA = DI.ArmorL[0].Amount;
		int CAA = DI.ArmorC[0].Amount;
		
		if (Place == 0) {
			if (DI.BackItems[Slot].item != null) {
				InvenNum = 0;
				
				for (int i = 0; i < Slot; ++i) {
					InvenNum += DI.BackItems[i].Amount;
				}
				
				int a = 0;
				MenuParent.position = transform.position;
				foreach (string S in DI.BackItems[Slot].item.MenuOptions) {
					Menu[a+(4-DI.BackItems[Slot].item.MenuOptions.Length)].GetComponentInChildren<Text>().text = S;
					++a;
				}
			}
		}
		
		
		if (Place == 1) {
			if (DI.SideItems[Slot].item != null) {
				InvenNum = BIA;
				
				for (int i = 0; i < Slot; ++i) {
					InvenNum += DI.SideItems[i].Amount;
				}
				int a = 0;
				MenuParent.position = transform.position;
				foreach (string S in DI.SideItems[Slot].item.MenuOptions) {
					Menu[a+(4-DI.SideItems[Slot].item.MenuOptions.Length)].GetComponentInChildren<Text>().text = S;
					++a;
				}
			}
		}
		
		if (Place == 2) {
			if (DI.BackpackItems[Slot].item != null) {
				InvenNum = BIA+SIA;
				
				for (int i = 0; i < Slot; ++i) {
					InvenNum += DI.BackpackItems[i].Amount;
				}
				int a = 0;
				MenuParent.position = transform.position;
				foreach (string S in DI.BackpackItems[Slot].item.MenuOptions) {
					Menu[a+(4-DI.BackpackItems[Slot].item.MenuOptions.Length)].GetComponentInChildren<Text>().text = S;
					++a;
				}
			}
		}
		
		if (Place == 3) {
			if (DI.ArmorL[Slot].item != null) {
				InvenNum = BIA+SIA+BPIA;
				int a = 0;
				MenuParent.position = transform.position;
				foreach (string S in DI.ArmorL[Slot].item.MenuOptions) {
					Menu[a+(4-DI.ArmorL[Slot].item.MenuOptions.Length)].GetComponentInChildren<Text>().text = S;
					++a;
				}
			}
		}
		
		if (Place == 4) {
			if (DI.ArmorC[Slot].item != null) {
				InvenNum = BIA+SIA+BPIA+LAA;
				int a = 0;
				MenuParent.position = transform.position;
				foreach (string S in DI.ArmorC[Slot].item.MenuOptions) {
					Menu[a+(4-DI.ArmorC[Slot].item.MenuOptions.Length)].GetComponentInChildren<Text>().text = S;
					++a;
				}
			}
		}
		
		if (Place == 5) {
			if (DI.ArmorM[Slot].item != null) {
				InvenNum = BIA+SIA+BPIA+LAA+CAA;
				int a = 0;
				MenuParent.position = transform.position;
				foreach (string S in DI.ArmorM[Slot].item.MenuOptions) {
					Menu[a+(4-DI.ArmorM[Slot].item.MenuOptions.Length)].GetComponentInChildren<Text>().text = S;
					++a;
				}
			}
		}
		
		if (Place == 10) {
			GameObject.Find("PossibleActions").GetComponent<Actions>().StartCoroutine(GetComponentInChildren<Text>().text+"", InvenNum);
		}
		
		if (Place >= 6) {
			MenuParent.gameObject.SetActive(false);
		} else {
			MenuParent.gameObject.SetActive(true);
		}
		
		
		foreach (GameObject G in Menu) {
			G.SetActive(true);
			if (G.GetComponentInChildren<Text>().text == "Option") {
				G.SetActive(false);
			}
		}
	}
}
