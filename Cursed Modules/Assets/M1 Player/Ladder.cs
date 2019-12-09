using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {
	
	public int Steps;
	public int StartStep;
	public Vector3 Offset;
	Vector3 OnStep;
	public bool Climbing;
	public float PlayerMass;
	GameObject Player;
	
	bool canClimb = true;
	float climbTimer = 0;
	bool l;
	bool lt;
	bool Down;
	float AnimSwitchTime;
	
	public AnimatorOverrideController[] AOC;
	
	public bool StopClimb;
	
	void Update () {
		if (Climbing) {
			
			Player.transform.position = transform.position + (OnStep/2) + transform.right*Offset.x + transform.up*Offset.y + transform.forward*Offset.z;
			
			AnimSwitchTime += Time.deltaTime;
			int AOC1 = 2;
			int AOC2 = 3;
			int AOC3 = 1;
			if (l) {
				AOC1 = 3;
				AOC2 = 2;
				AOC3 = 0;
			}
			
			if (l != lt) {
				lt = l;
				AnimSwitchTime = 0;
			}
			if (AnimSwitchTime < 0.22222f) {
				if (AnimSwitchTime == 0) {
					Player.GetComponent<Animator>().Play ("Climb", -1, 0f);
				}
				if (Down) {
					Player.GetComponent<Animator>().SetFloat ("ReverseAnim", -1f);
					Player.GetComponent<Animator>().runtimeAnimatorController = AOC[AOC1];
					Player.transform.position = transform.position + (OnStep/2) + transform.right*Offset.x + transform.up*Offset.y + transform.forward*Offset.z + new Vector3 (0, 0.5f, 0);
				} else {
					Player.GetComponent<Animator>().SetFloat ("ReverseAnim", 1f);
					Player.GetComponent<Animator>().runtimeAnimatorController = AOC[AOC2];
				}
			} else {
				Player.GetComponent<Animator>().runtimeAnimatorController = AOC[AOC3];
			}
			
			if (SSInput.LVert[0] > 0.25f) {
				climbTimer += Time.deltaTime;
				if (canClimb || climbTimer > 0.5f) {
					Down = false;
					l = !l;
					climbTimer = 0.3f;
					canClimb = false;
					++OnStep.y;
				}
			} else if (SSInput.LVert[0] < -0.25f) {
				climbTimer += Time.deltaTime;
				if (canClimb || climbTimer > 0.5f) {
					Down = true;
					l = !l;
					climbTimer = 0.3f;
					canClimb = false;
					--OnStep.y;
				}
			} else {
				climbTimer = 0;
				canClimb = true;
			}
			
			if (SSInput.B[0] == "Pressed" || OnStep.y < 0 || OnStep.y > Steps) {
				GetOff ();
			}
			OnStep.y = Mathf.Clamp(OnStep.y, 0, Steps);
			
			StopClimb = true;
		} else {
			if (StopClimb) {
				StopClimb = false;
				Invoke ("DontClimb", 0.5f);
			}
		}
	}
	
	public void Climb () {
		Player = GlobVars.CurrentPlayer;
		if (GlobVars.InteractObject == this.gameObject) {
			Climbing = true;
			GlobVars.Reading = true;
			Player.GetComponent<Movement>().enabled = false;
			OnStep.y = StartStep;
			Player.transform.rotation = transform.rotation;
			Player.transform.parent = this.gameObject.transform;
			PlayerMass = Player.GetComponent<Rigidbody>().mass;
			Destroy(Player.GetComponent<Rigidbody>());
		} else {
			Climbing = false;
		}
	}
	
	void GetOff () {
		GlobVars.Reading = false;
		Player.transform.parent = GameObject.Find("All Level Objects").transform;
		Player.GetComponent<Movement>().enabled = true;
		if (Player.GetComponent<Rigidbody>() == null) {
			Player.AddComponent<Rigidbody>();
			Rigidbody RB;
			RB = Player.GetComponent<Rigidbody>();
			RB.mass = PlayerMass;
			RB.collisionDetectionMode = CollisionDetectionMode.Continuous;
			RB.interpolation = RigidbodyInterpolation.None;
			RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
		}
		Interactables.StopI();
		Climbing = false;
	}
	
	void DontClimb () {
		Player.GetComponent<Animator>().SetBool("Climb", false);
	}
}
