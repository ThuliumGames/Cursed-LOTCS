using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerFollow : MonoBehaviour {
	
	public Transform FollowObj;
	public bool JustAFollow;
	
	void Update () {
		FollowObj.position = transform.position;
		if (!JustAFollow) {
			GetComponent<NavMeshAgent>().SetDestination(GlobVars.CurrentPlayer.transform.position);
			if (Vector3.Distance(transform.position, GlobVars.CurrentPlayer.transform.position) > 50) {
				GetComponent<NavMeshAgent>().Warp(GlobVars.CurrentPlayer.transform.position);
			}
			
			if (GetComponent<Animator>().GetBool("Crouching")) {
				GetComponent<NavMeshAgent>().speed = 3;
			} else {
				if (Vector3.Distance(transform.position, GlobVars.CurrentPlayer.transform.position) > 10) {
					GetComponent<NavMeshAgent>().speed = 15;
				} else {
					GetComponent<NavMeshAgent>().speed = 7;
				}
			}
			
			if (Vector3.Distance(transform.position, GlobVars.CurrentPlayer.transform.position) <= GetComponent<NavMeshAgent>().stoppingDistance) {
				if (GetComponent<Animator>().GetBool("Crouching")) {
					GetComponent<Animator>().Play("Crouching");
				} else {
					GetComponent<Animator>().Play("Idle");
				}
			} else {
				if (GetComponent<Animator>().GetBool("Crouching")) {
					GetComponent<Animator>().Play("CrouchWalk");
				} else {
					if (Vector3.Distance(transform.position, GlobVars.CurrentPlayer.transform.position) > 10) {
						GetComponent<Animator>().Play("Sprint");
					} else {
						GetComponent<Animator>().Play("Walk");
					}
				}
			}
		}
	}
}
