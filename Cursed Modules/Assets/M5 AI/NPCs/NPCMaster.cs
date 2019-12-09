using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Task {
	public Transform Location;
	public float StoppingDist;
	public bool SnapTo;
	public float EndTime;
	public string AnimToPlay;
	public string GetThereAnim;
	public bool nextDay;
	public float Importance;
	
	public int DialogueNumberThere = -1;
	public int DialogueNumberNotThere = -1;
}

public class NPCMaster : MonoBehaviour {
	public List<Task> Tasks;
	
	public Transform FoodLocation;
	public float FoodNeed;
	
	public int FoodDialogueNumberThere = -1;
	public int FoodDialogueNumberNotThere = -1;
	
	public GameObject NPC;
	[HideInInspector]
	public Transform player;
	
	void Update () {
		
		player = GlobVars.CurrentPlayer.transform;
		
		float time = (float)GlobVars.Hour+(((float)GlobVars.Mins)/60);
		
		FoodNeed += (Time.deltaTime/960)*100*GlobVars.DaySpeed;
		
		if ((time >= Tasks[0].EndTime && !Tasks[0].nextDay) || (time >= Tasks[0].EndTime && time <= Tasks[Tasks.Count-1].EndTime && Tasks[0].nextDay)) {
			Task TmpT = Tasks[0];
			Tasks.Add(TmpT);
			Tasks.RemoveAt(0);
		}
		
		//Add If Close
		if (Vector3.Distance (Tasks[0].Location.position, player.position) < 500) {
			if (!GameObject.Find(NPC.name)) {
				GameObject npc = Instantiate (NPC);
				npc.GetComponent<NavMeshAgent>().Warp (Tasks[0].Location.position);
				npc.name = NPC.name;
				npc.GetComponent<NPCAI>().NPCmaster = this;
			}
		}
	}
}
