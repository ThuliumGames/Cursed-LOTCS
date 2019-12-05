using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
	
	public Animator Anim;
	public Collider WeaponColl;
	public float PatrolArea;
	bool Attacking;
	float coolDown;
	Vector3 DefPos;
	Vector3 Offset;
	
	public NavMeshAgent NMA;
	
	public LayerMask LM;
	
	void Start () {
		DefPos = transform.position;
	}
	
	void Update () {
		if (Vector3.Distance (transform.position, GameObject.Find("Player").transform.position) < 20) {
			NMA.SetDestination(GameObject.Find("Player").transform.position);
			if (Vector3.Distance (transform.position, GameObject.Find("Player").transform.position) < 3) {
				if (Physics.BoxCast ((transform.position+transform.up), new Vector3 (0.125f, 1, 0.125f), transform.forward, Quaternion.Euler (Vector3.zero), 3f, LM)) {
					if (!Attacking && coolDown <= 0) {
						Attacking = true;
						Anim.Play("Attack1h1");
						Anim.SetFloat("speedv", 0);
						coolDown = 2;
					}
				}
			}
		} else {
			NMA.SetDestination(DefPos+Offset);
		}
		
		if (Attacking) {
			WeaponColl.enabled = true;
		} else {
			WeaponColl.enabled = false;
		}
		
		coolDown -= Time.deltaTime;
		coolDown = Mathf.Clamp (coolDown, -1, 10);
		
		if (coolDown <= 0) {
			Attacking = false;
			if (Vector3.Distance (transform.position, NMA.destination) < NMA.stoppingDistance) {
				Offset += new Vector3 (Random.Range(-PatrolArea, PatrolArea), 0, Random.Range(-PatrolArea, PatrolArea));
				Anim.SetFloat("speedv", Mathf.Lerp (Anim.GetFloat("speedv"), 0, Time.deltaTime));
			} else {
				Anim.SetFloat("speedv", Mathf.Lerp (Anim.GetFloat("speedv"), 1, Time.deltaTime));
			}
		}
		
		Offset = new Vector3 (Mathf.Clamp (Offset.x, -PatrolArea, PatrolArea), 0, Mathf.Clamp (Offset.z, -PatrolArea, PatrolArea));
		
		if (!Attacking && Vector3.Distance (transform.position, NMA.destination) < NMA.stoppingDistance) {
			GameObject G = new GameObject();
			G.transform.position = transform.position;
			G.transform.LookAt(new Vector3 (NMA.destination.x, transform.position.y, NMA.destination.z));
			transform.rotation = Quaternion.Lerp(transform.rotation, G.transform.rotation, 2*Time.deltaTime);
			Destroy(G);
		}
		
		NMA.speed = 3.5f*Anim.GetFloat ("speedv");
	}
}
