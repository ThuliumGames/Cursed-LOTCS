using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System.Runtime.InteropServices;

public class DisplayInventory : MonoBehaviour {
	
	//Mouse Input
	public struct Point {
		public int x;
		public int y;
	}
	
	public Point Pos;
	
	[DllImport("user32.dll")]
	public static extern bool SetCursorPos(int X, int Y);
	
	[DllImport("user32.dll")]
	public static extern bool GetCursorPos(out Point Pos);
	//End Mouse Input
	
	public Inventory Inv;
	
	public GameObject[] Slots;
	
	public Camera UICam;
	public GameObject AntiInteract;
	public PostProcessVolume PPV;
	
	public bool Right;
	
	int i = 0;
	
	void Update () {
		
		if (SSInput.Strt[0] == "Pressed") {
			GlobVars.PlayerPaused = !GlobVars.PlayerPaused;
		}
		
		if (GlobVars.PlayerPaused && !GlobVars.Reading) {
			Point cursorPos = new Point();
			GetCursorPos(out cursorPos);
			
			if (SSInput.LS[0] == "Down") {
				SetCursorPos(cursorPos.x+(int)(Mathf.Clamp(SSInput.LHor[0]*0.707f*20, -10, 10)), cursorPos.y-(int)(Mathf.Clamp(SSInput.LVert[0]*0.707f*20, -10, 10)));
			} else {
				SetCursorPos(cursorPos.x+(int)(Mathf.Clamp(SSInput.LHor[0]*0.707f*10, -10, 10)), cursorPos.y-(int)(Mathf.Clamp(SSInput.LVert[0]*0.707f*10, -10, 10)));
			}
			
			if (SSInput.RB[0] == "Pressed") {
				Right = true;
			}
			if (SSInput.LB[0] == "Pressed") {
				Right = false;
			}
			
			UICam.enabled = true;
			AntiInteract.SetActive(false);
			PPV.weight = Mathf.Lerp(PPV.weight, 1, 7*Time.deltaTime);
			
		} else {
			
			Right = false;
			
			UICam.enabled = false;
			AntiInteract.SetActive(true);
			PPV.weight = Mathf.Lerp(PPV.weight, 0, 7*Time.deltaTime);
			
		}
		
		if (Right) {
			GameObject.Find(name + "/InventMove").transform.localPosition = Vector3.Lerp (GameObject.Find(name + "/InventMove").transform.localPosition, new Vector3 (-1920, 0, 0), 25*Time.deltaTime);
		} else {
			GameObject.Find(name + "/InventMove").transform.localPosition = Vector3.Lerp (GameObject.Find(name + "/InventMove").transform.localPosition, new Vector3 (0, 0, 0), 25*Time.deltaTime);
		}
		
		i = 0;
		foreach (anItem BS in Inv.BackItems) {
			Disp (BS);
		}
		foreach (anItem SS in Inv.SideItems) {
			Disp (SS);
		}
		foreach (anItem BPS in Inv.BackpackItems) {
			Disp (BPS);
		}
		foreach (anItem AL in Inv.ArmorL) {
			Disp (AL);
		}
		foreach (anItem AC in Inv.ArmorC) {
			Disp (AC);
		}
		foreach (anItem AM in Inv.ArmorM) {
			Disp (AM);
		}
	}
	
	void Disp (anItem It) {
		if (It.item != null) {
			Slots[i].GetComponentInChildren<Image>().color = Color.white;
			Slots[i].GetComponentInChildren<Image>().sprite = It.item.sprite;
			if (It.Amount > 1) {
				Slots[i].GetComponentInChildren<Text>().text = ""+It.Amount;
			} else {
				Slots[i].GetComponentInChildren<Text>().text = "";
			}
			
		} else {
			Slots[i].GetComponentInChildren<Image>().color = Color.clear;
			Slots[i].GetComponentInChildren<Text>().text = "";
		}
		++i;
	}
}
