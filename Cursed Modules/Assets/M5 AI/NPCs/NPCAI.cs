using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour {
	public Animator Anim;
	public NavMeshAgent NMA;
	public NPCMaster NPCmaster;
	
	void Update () {
		
		//Where To Go
		if (GlobVars.Reading && GlobVars.InteractObject == this.gameObject) {
			NMA.SetDestination (NPCmaster.player.position);
		} else {
			NMA.SetDestination (NPCmaster.Tasks[0].Location.position);
		}
		
		RaycastHit Hit;
		if (Physics.BoxCast ((transform.position+transform.up), new Vector3 (0.25f, 1, 0.25f), transform.forward, out Hit, Quaternion.Euler (Vector3.zero), 1f)) {
			if (Hit.collider.gameObject.tag == "Door") {
				if (!Hit.collider.gameObject.GetComponent<PlayAnimation>().Anim.GetBool (Hit.collider.gameObject.GetComponent<PlayAnimation>().Play)) {
					Hit.collider.gameObject.GetComponent<PlayAnimation>().PlayA();
				}
			}
		}
		if (Vector3.Distance (transform.position, NPCmaster.Tasks[0].Location.position) <= NMA.stoppingDistance) {
			Anim.Play(NPCmaster.Tasks[0].AnimToPlay);
			transform.rotation = Quaternion.Lerp(transform.rotation, NPCmaster.Tasks[0].Location.rotation, 20*Time.deltaTime);
		} else {
			Anim.Play("Walk");
		}
		
		//Destroy If Far
		if (Vector3.Distance (transform.position, NPCmaster.player.position) > 100) {
			Destroy(this.gameObject);
		}
	}
}
