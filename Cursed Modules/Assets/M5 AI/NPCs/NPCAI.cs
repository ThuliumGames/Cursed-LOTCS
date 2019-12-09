using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour {
	
	public Animator Anim;
	public AnimatorOverrideController AOC;
	public NavMeshAgent NMA;
	public NPCMaster NPCmaster;
	
	Vector3 PrePos;
	
	void Start () {
		if (AOC != null) {
			Anim.runtimeAnimatorController = AOC;
		}
	}
	
	void Update () {
		
		//Where To Go
		if (GlobVars.InteractObject == this.gameObject) {
			NMA.stoppingDistance = 100;
			NMA.SetDestination (NPCmaster.player.position);
		} else {
			NMA.stoppingDistance = NPCmaster.Tasks[0].StoppingDist;
			
			if (NPCmaster.FoodNeed > NPCmaster.Tasks[0].Importance) {
				NMA.SetDestination (NPCmaster.FoodLocation.position);
			} else {
				NMA.SetDestination (NPCmaster.Tasks[0].Location.position);
			}
		}
		
		RaycastHit Hit;
		if (Physics.BoxCast ((transform.position+transform.up), new Vector3 (0.25f, 1, 0.25f), transform.forward, out Hit, Quaternion.Euler (Vector3.zero), 1f)) {
			if (Hit.collider.gameObject.tag == "Door") {
				if (!Hit.collider.gameObject.GetComponent<PlayAnimation>().Anim.GetBool (Hit.collider.gameObject.GetComponent<PlayAnimation>().Play)) {
					Hit.collider.gameObject.GetComponent<PlayAnimation>().PlayA();
				}
			}
		}
		
		float Dist = Vector3.Distance (transform.position, PrePos);
		
		if (Dist < 0.01f) {
			if (GlobVars.InteractObject == this.gameObject) {
				
				transform.LookAt (new Vector3 (NPCmaster.player.position.x, transform.position.y, NPCmaster.player.position.z));
			
				Anim.Play ("Talk");
			
			}
		} else {
			if (Vector3.Distance (transform.position, NMA.destination) > NMA.stoppingDistance) {
				Anim.Play(NPCmaster.Tasks[0].GetThereAnim);
				
				if (GetComponent<Dialogue>() != null && NPCmaster.Tasks[0].DialogueNumberNotThere != -1 && GlobVars.InteractObject != this.gameObject) {
					GetComponent<Dialogue>().TextToRead = NPCmaster.Tasks[0].DialogueNumberNotThere;
				}
			}
		}
		
		if (Vector3.Distance (transform.position, NMA.destination) <= NMA.stoppingDistance) {
			
			if (GlobVars.InteractObject != this.gameObject) {
				if (NPCmaster.FoodNeed > NPCmaster.Tasks[0].Importance) {
					
					Anim.Play ("Talk");
					
					if (GetComponent<Dialogue>() != null && NPCmaster.FoodDialogueNumberThere != -1 && GlobVars.InteractObject != this.gameObject) {
						GetComponent<Dialogue>().TextToRead = NPCmaster.FoodDialogueNumberThere;
					}
					
					Invoke ("ResetFood", 5);
					
				} else {
					Anim.Play (NPCmaster.Tasks[0].AnimToPlay);
					
					if (GetComponent<Dialogue>() != null && NPCmaster.Tasks[0].DialogueNumberThere != -1 && GlobVars.InteractObject != this.gameObject) {
						GetComponent<Dialogue>().TextToRead = NPCmaster.Tasks[0].DialogueNumberThere;
					}
					
					if (NPCmaster.Tasks[0].SnapTo) {
						transform.position = NPCmaster.Tasks[0].Location.position;
						transform.rotation = NPCmaster.Tasks[0].Location.rotation;
					} else {
						transform.rotation = Quaternion.Lerp(transform.rotation, NPCmaster.Tasks[0].Location.rotation, 20*Time.deltaTime);
					}
				}
			}
		}
		
		//Destroy If Far
		if (Vector3.Distance (transform.position, NPCmaster.player.position) > 500) {
			Destroy(this.gameObject);
		}
		
		PrePos = transform.position;
	}
	
	void ResetFood () {
		NPCmaster.FoodNeed = 0;
	}
}
