using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Task {
	public Transform Location;
	public float EndTime;
	public string AnimToPlay;
	public bool nextDay;
}

public class NPCMaster : MonoBehaviour {
	public List<Task> Tasks;
	public GameObject NPC;
	[HideInInspector]
	public Transform player;
	
	void Start () {
		player = GameObject.Find("Player").transform;
	}
	
	void Update () {
		
		float time = (float)GlobVars.Hour+(((float)GlobVars.Mins)/60);
		
		if ((time >= Tasks[0].EndTime && !Tasks[0].nextDay) || (time >= Tasks[0].EndTime && time <= Tasks[Tasks.Count-1].EndTime && Tasks[0].nextDay)) {
			Task TmpT = Tasks[0];
			Tasks.Add(TmpT);
			Tasks.RemoveAt(0);
		}
		
		//Add If Close
		if (Vector3.Distance (Tasks[0].Location.position, player.position) < 50) {
			if (!GameObject.Find(NPC.name)) {
				GameObject npc = Instantiate (NPC, Tasks[0].Location.position, Quaternion.identity);
				npc.name = NPC.name;
				npc.GetComponent<NPCAI>().NPCmaster = this;
			}
		}
	}
}
