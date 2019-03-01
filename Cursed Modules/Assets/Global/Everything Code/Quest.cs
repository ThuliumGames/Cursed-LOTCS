using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour {
	
	public Animator Anim;
	public AudioSource AS;
	
	void Update () {
		int i = 0;
		if (GlobVars.Quests != null && GlobVars.DoneQuests != null) {
			foreach (string S in GlobVars.Quests) {
				int a = 0;
				foreach (string C in GlobVars.DoneQuests) {
					if (S == C) {
						GlobVars.Quests[i] = GlobVars.Quests[GlobVars.Quests.Length - 1];
						Array.Resize(ref GlobVars.Quests, GlobVars.Quests.Length - 1);
					}
					++a;
				}
				++i;
			}
		}
	}
	
	public void GiveQuest (string QuestName) {
		bool CanGive = true;
		foreach (string S in GlobVars.Quests) {
			if (QuestName == S) {
				CanGive = false;
			}
		}
		foreach (string S in GlobVars.DoneQuests) {
			if (QuestName == S) {
				CanGive = false;
			}
		}
		if (CanGive) {
			if (AS != null)
			AS.Play();
			Array.Resize (ref GlobVars.Quests, GlobVars.Quests.Length + 1);
			GlobVars.Quests[GlobVars.Quests.Length - 1] = QuestName;
			Anim.gameObject.GetComponentInChildren<Text>().text = "New Quest\n" + QuestName;
			Anim.Play("Quest");
		}
	}
	
	public void CompleteQuest (string QuestName) {
		bool CanGive = true;
		foreach (string S in GlobVars.DoneQuests) {
			if (QuestName == S) {
				CanGive = false;
			}
		}
		if (CanGive) {
			if (AS != null)
			AS.Play();
			Array.Resize (ref GlobVars.DoneQuests, GlobVars.DoneQuests.Length + 1);
			GlobVars.DoneQuests[GlobVars.DoneQuests.Length - 1] = QuestName;
			Anim.gameObject.GetComponentInChildren<Text>().text = "Quest Complete\n" + QuestName;
			Anim.Play("Quest");
		}
	}
}
