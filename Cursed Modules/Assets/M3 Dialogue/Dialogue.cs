using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CostomCode {
	public MonoBehaviour component;
	public string Command;
	[Header("Only 1")]
	public string[] StringInput;
	public bool[] BoolInput;
	public int[] IntInput;
	public float[] FloatInput;
	
}

[System.Serializable]
 public class SubArray {
    public Color TextColor;
	public int TextSize;
	public string Text;
	public int NextText;
	public string AnsText;
	public int[] NextAnsText;
	public float TimedEnd;
	[Header("Test For Item In Inventory")]
	public bool FoundItem;
	public int NextTextIfTrue;
	[Header("To Modify Another Dialogue System")]
	public Dialogue ModOther;
	public int OtherNextText;
	public string DiffName;
	[Header("To Set Camera Position")]
	public Vector3 CamRotation;
	public Vector3 CamPos;
	public float MoveSpeed;
	[Header("")]
	[Header("For Costom Code Execution")]
	public CostomCode[] CodeToExecute;
 }

public class Dialogue : MonoBehaviour {
	bool StopInteract;
	bool CanGo = true;
	[Header("")]
	public Canvas DialogueCanvas;
	public Image BackgroundImage;
	public Sprite BackgroundToUse;
	public Text RegWrite;
	public Text AnsWrite;
	public Text NameWrite;
	public GameObject GoToNext;
	[Header("")]
	public bool isSign;
	public TextAnchor TextAlign;
	public float WT = 0.05f;
	[Header("")]
	[Header("Commands: > = Line : # = Blue : * = Large : % = Small")]
	[Header("[ = Auto : $ = Question : _ = End : ~ = Command")]
	[Header("")]
	public SubArray[] DialogueVariables;
	
	int TextToRead = 0;
	int AmOfAns = -1;
	Vector3 OrigCamPos;
	bool isMoving;
	
	bool DoneReading = false;
	bool DoneTalking = false;
	
	bool Writing = false;
	bool isQuest = false;
	bool WaitForInput;
	
	string Words;
	
	void Update () {
		
		if (StopInteract) {
			StopInteract = false;
			GlobVars.Interacting = false;
		}
		
		if (isMoving) {
			Camera.main.GetComponentInParent<CamControl>().enabled = false;
			Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, DialogueVariables[TextToRead].CamPos, DialogueVariables[TextToRead].MoveSpeed*Time.deltaTime);
			Camera.main.transform.rotation = Quaternion.Slerp (Camera.main.transform.rotation, Quaternion.Euler (DialogueVariables[TextToRead].CamRotation), DialogueVariables[TextToRead].MoveSpeed*Time.deltaTime);
		} else {
			if (!Camera.main.GetComponentInParent<CamControl>().enabled) {
				Camera.main.GetComponentInParent<CamControl>().enabled = true;
			}
		}
		
		if (!GlobVars.PlayerPaused || GlobVars.Reading) {
			if (GlobVars.InteractObject == this.gameObject) {
			
				if (!isQuest) {
					if (SSInput.A[0] == "Pressed" || SSInput.B[0] == "Pressed") {
						if (DoneReading) {
							if (DoneTalking) {
								DoneReading = false;
								DoneTalking = false;
								if (WaitForInput) {
									Exe (DialogueVariables[TextToRead].CodeToExecute.Length-1);
									WaitForInput = false;
								}
								if (DialogueVariables[TextToRead].CodeToExecute.Length > 0) {
									if (DialogueVariables[TextToRead].CodeToExecute[0].Command == "TestForItem" && DialogueVariables[TextToRead].FoundItem) {
										GameObject.FindObjectOfType<Inventory>().RemoveItem(DialogueVariables[TextToRead].CodeToExecute[0].StringInput[0]);
										TextToRead = DialogueVariables[TextToRead].NextTextIfTrue;
									} else {
										TextToRead = DialogueVariables[TextToRead].NextText;
									}
								} else {
									TextToRead = DialogueVariables[TextToRead].NextText;
								}
								DialogueCanvas.gameObject.SetActive(false);
								Writing = false;
								StopInteract = true;
								GlobVars.InteractObject = null;
								GlobVars.Reading = false;
								GoToNext.SetActive(false);
								isMoving = false;
							} else {
								DoneReading = false;
								DoneTalking = false;
								TextToRead = DialogueVariables[TextToRead].NextText;
								StartCoroutine(Write ());
								Writing = true;
							}
						}
					}
				} else {
					if (SSInput.A[0] == "Pressed") {
						if (DoneReading) {
							DoneReading = false;
							DoneTalking = false;
							TextToRead = DialogueVariables[TextToRead].NextAnsText[0];
							StartCoroutine(Write ());
							Writing = true;
						}
					}
					if (SSInput.B[0] == "Pressed") {
						if (DoneReading && AmOfAns > 1) {
							DoneReading = false;
							DoneTalking = false;
							TextToRead = DialogueVariables[TextToRead].NextAnsText[1];
							StartCoroutine(Write ());
							Writing = true;
						}
					}
					if (SSInput.X[0] == "Pressed") {
						if (DoneReading && AmOfAns > 2) {
							DoneReading = false;
							DoneTalking = false;
							TextToRead = DialogueVariables[TextToRead].NextAnsText[2];
							StartCoroutine(Write ());
							Writing = true;
						}
					}
					if (SSInput.Y[0] == "Pressed") {
						if (DoneReading && AmOfAns > 3) {
							DoneReading = false;
							DoneTalking = false;
							TextToRead = DialogueVariables[TextToRead].NextAnsText[3];
							StartCoroutine(Write ());
							Writing = true;
						}
					}
				}
			}
		} else {
			CanGo = true;
		}
	}
		
	IEnumerator Write () {
		
		if (DialogueVariables[TextToRead].CamPos != new Vector3 (0, 0, 0)) {
			isMoving = true;
		} else {
			isMoving = false;
		}
		
		string ColorText = "";
		string ColorLetters = "";
		string SizeText = "";
		string SmallText = "";
		string SizeLetters = "";
		string SmallLetters = "";
		int Code = 0;
		RegWrite.color = DialogueVariables[TextToRead].TextColor;
		RegWrite.fontSize = DialogueVariables[TextToRead].TextSize;
		bool DontFin = false;
		if (DialogueVariables[TextToRead].ModOther != null) {
			DialogueVariables[TextToRead].ModOther.TextToRead = DialogueVariables[TextToRead].OtherNextText;
		}
		GoToNext.SetActive(false);
		if (DialogueVariables[TextToRead].DiffName == "") {
			NameWrite.text = name;
		} else {
			NameWrite.text = DialogueVariables[TextToRead].DiffName;
		}
		RegWrite.text = "";
		AnsWrite.text = "";
		AmOfAns = -1;
		isQuest = false;
		int Skip = 0;
		bool isColor = false;
		bool isSize = false;
		bool isSmall = false;
		foreach (char C in DialogueVariables[TextToRead].Text) {
			++Skip;
			if (C == '[') {
				DontFin = true;
				yield return new WaitForSeconds (DialogueVariables[TextToRead].TimedEnd);
				DoneReading = false;
				DoneTalking = false;
				if (DialogueVariables[TextToRead].CodeToExecute.Length > 0) {
					if (DialogueVariables[TextToRead].CodeToExecute[0].Command == "TestForItem" && DialogueVariables[TextToRead].FoundItem) {
						GameObject.FindObjectOfType<Inventory>().RemoveItem(DialogueVariables[TextToRead].CodeToExecute[0].StringInput[0]);
						TextToRead = DialogueVariables[TextToRead].NextTextIfTrue;
					} else {
						TextToRead = DialogueVariables[TextToRead].NextText;
					}
				} else {
					TextToRead = DialogueVariables[TextToRead].NextText;
				}
				StartCoroutine(Write ());
				Writing = true;
			} else {
				if (C == '_') {
					DoneTalking = true;
				} else if (C == '$') {
					isQuest = true;
				} else if (C == '~') {
					char L = DialogueVariables[TextToRead].Text[Skip];
					if (L == '_') {
						WaitForInput = true;
					} else {
						Exe (Code);
						++Code;
					}
				} else if (C == '#') {
					if (isColor) {
						isColor = false;
					} else {
						ColorLetters = "";
						ColorText = RegWrite.text;
						isColor = true;
					}
				} else if (C == '*') {
					if (isSize) {
						isSize = false;
					} else {
						SizeLetters = "";
						SizeText = RegWrite.text;
						isSize = true;
					}
				} else if (C == '%') {
					if (isSmall) {
						isSmall = false;
					} else {
						SmallLetters = "";
						SmallText = RegWrite.text;
						isSmall = true;
					}
				} else {
					if (C == '>') {
						RegWrite.text += "\n";
					} else {
						if (!isSign || Skip > 2) {
							if (isSign && Skip < 4) {
								char F = DialogueVariables[TextToRead].Text[0];
								char S = DialogueVariables[TextToRead].Text[1];
								RegWrite.text += F;
								RegWrite.text += S;
							}
							if (isColor) {
								ColorLetters += C;
								RegWrite.text = ColorText + "<color=#0088ff>" + ColorLetters + "</color>";
							} else if (isSize) {
								SizeLetters += C;
								RegWrite.text = SizeText + "<size="+ DialogueVariables[TextToRead].TextSize*2 + ">" + SizeLetters + "</size>";
							} else if (isSmall) {
								SmallLetters += C;
								RegWrite.text = SmallText + "<size="+ DialogueVariables[TextToRead].TextSize/2 + ">" + SmallLetters + "</size>";
							} else {
								RegWrite.text += C;
							}
						}
					}
					if ((SSInput.B[0] != "Down" || Skip < 3) && (!isSign || Skip < 2)) {
						if (C == '.') {
							yield return new WaitForSeconds (WT*8);
						} else if (C == '?') {
							yield return new WaitForSeconds (WT*8);
						} else if (C == '!') {
							yield return new WaitForSeconds (WT*8);
						} else if (C == ',') {
							yield return new WaitForSeconds (WT*4);
						} else {
							yield return new WaitForSeconds (WT);
						}
					}
				}
			}
		}
		
		if (isQuest) {
			foreach (char C in DialogueVariables[TextToRead].AnsText) {
				if (AmOfAns == -1) {
					AmOfAns = (int)(C)-48;
				} else {
					if (C == '>') {
						AnsWrite.text += "\n";
					} else {
						AnsWrite.text += C;
					}
					if ((SSInput.B[0] != "Down" || Skip < 3) && (!isSign || Skip < 2)) {
						yield return new WaitForSeconds (WT/10);
					}
				}
			}
		}
		
		if (!DontFin) {
			yield return new WaitForSeconds (DialogueVariables[TextToRead].TimedEnd);
			GoToNext.SetActive(true);
			DoneReading = true;
		}
	}
	void Exe (int Num) {
		if (DialogueVariables[TextToRead].CodeToExecute[Num].Command == "TestForItem") {
			DialogueVariables[TextToRead].FoundItem = GameObject.FindObjectOfType<Inventory>().TestForItem(DialogueVariables[TextToRead].CodeToExecute[Num].StringInput[0]);
		} else {
			if (DialogueVariables[TextToRead].CodeToExecute[Num].StringInput.Length != 0) {
				DialogueVariables[TextToRead].CodeToExecute[Num].component.StartCoroutine(DialogueVariables[TextToRead].CodeToExecute[Num].Command, DialogueVariables[TextToRead].CodeToExecute[Num].StringInput[0]);
			} else if (DialogueVariables[TextToRead].CodeToExecute[Num].BoolInput.Length != 0) {
				DialogueVariables[TextToRead].CodeToExecute[Num].component.StartCoroutine(DialogueVariables[TextToRead].CodeToExecute[Num].Command, DialogueVariables[TextToRead].CodeToExecute[Num].BoolInput[0]);
			} else if (DialogueVariables[TextToRead].CodeToExecute[Num].IntInput.Length != 0) {
				DialogueVariables[TextToRead].CodeToExecute[Num].component.StartCoroutine(DialogueVariables[TextToRead].CodeToExecute[Num].Command, DialogueVariables[TextToRead].CodeToExecute[Num].IntInput[0]);
			} else if (DialogueVariables[TextToRead].CodeToExecute[Num].FloatInput.Length != 0) {
				DialogueVariables[TextToRead].CodeToExecute[Num].component.StartCoroutine(DialogueVariables[TextToRead].CodeToExecute[Num].Command, DialogueVariables[TextToRead].CodeToExecute[Num].FloatInput[0]);
			} else {
				DialogueVariables[TextToRead].CodeToExecute[Num].component.StartCoroutine(DialogueVariables[TextToRead].CodeToExecute[Num].Command);
			}
		}
	}
	
	void Initiate () {
		CanGo = false;
		GlobVars.Reading = true;
		DialogueCanvas.gameObject.SetActive(true);
		BackgroundImage.sprite = BackgroundToUse;
		RegWrite.alignment = TextAlign;
		RegWrite.text = "";
		if (!Writing) {
			DoneReading = false;
			DoneTalking = false;
			StartCoroutine(Write ());
			Writing = true;
		}
	}
}
