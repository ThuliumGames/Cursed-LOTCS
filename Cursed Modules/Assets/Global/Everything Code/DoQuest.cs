using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoQuest : MonoBehaviour {
	
	[Header("if False: Quest Complete")]
	public bool QuestGive;
	
	public string QuestName;
	
	void QuestDo () {
		Interactables.StopI();
		if (QuestGive) {
			GameObject.FindObjectOfType<Quest>().GiveQuest(QuestName);
		} else {
			GameObject.FindObjectOfType<Quest>().CompleteQuest(QuestName);
		}
		Destroy(this.gameObject);
	}
}
