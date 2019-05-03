using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {

	public Animator Anim;
	
	public float Acceleration;
	public float TurnSpeed;
	public float JumpPower;
	public float MaxStepHeight;
	
	public float maxStamina;
	float minStamina;
	float Stamina;
	public Image StamIma;
	
	Vector3 Pos;
	Vector3 PrevPos;
	public LayerMask LM;
	
	bool WasPaused;
	Vector3 PrevVel;
	
	Quaternion Q;
	
	public AnimatorOverrideController AOC;
	
	void Start () {
		Stamina = maxStamina;
	}
	
	void Update () {

		GetComponent<Animator>().SetFloat ("ReverseAnim", 1);
		
		//Reset Attack Anim
		if (!GetComponentInChildren<Weapon>()) {
			GetComponentInParent<Animator>().runtimeAnimatorController = AOC;
		}
		
		if (!GlobVars.Paused) {
			
			//Maintain Velocity When Paused
			Pos = transform.position;
			if (!Anim.GetBool("Climb")) {
				GetComponent<Rigidbody>().isKinematic = false;
			}
			if (WasPaused) {
				GetComponent<Rigidbody>().velocity = PrevVel;
				WasPaused = false;
			}
			
			if (!GlobVars.PlayerPaused && !GlobVars.Reading) {
				
				//Attack
				if (SSInput.X[0] == "Pressed") {
					Anim.SetBool("Attacking", true);
				}
				
				if (Anim.GetBool("OnGround") && !Anim.GetBool("Attacking")) {
					
					//Jumping
					if (SSInput.Y[0] == "Pressed" && Stamina > minStamina) {
						
						//Fall When Jump
						GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
						
						Stamina -= 0.5f;
						transform.position += new Vector3 (0, 1f, 0);
						transform.rotation = Q;
						GetComponent<Rigidbody>().velocity = new Vector3 (GetComponent<Rigidbody>().velocity.x, JumpPower, GetComponent<Rigidbody>().velocity.z);
					} else if (SSInput.Y[0] == "Pressed") {
						
						//Fall When Jump
						GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
						
						//Jump Less If Out Of Stamina
						transform.position += new Vector3 (0, 1f, 0);
						transform.rotation = Q;
						GetComponent<Rigidbody>().velocity = new Vector3 (GetComponent<Rigidbody>().velocity.x/2, JumpPower/2, GetComponent<Rigidbody>().velocity.z/2);
					}
					
					//Sprinting
					if (SSInput.B[0] == "Down" && Stamina > minStamina) {
						Stamina -= Time.deltaTime*(int)(Anim.GetFloat("VSpeed")+0.25f);
						Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), new Vector2 (SSInput.LHor[0], SSInput.LVert[0]).magnitude, Acceleration * Time.deltaTime));
					} else {
						//Dont Sprint If Out Of Stamina
						Stamina += Time.deltaTime;
						Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), new Vector2 (SSInput.LHor[0], SSInput.LVert[0]).magnitude*0.5f, Acceleration * Time.deltaTime));
					}
					
					//Fix Smooth Rotation Problems
					if (new Vector2 (SSInput.LHor[0], SSInput.LVert[0]).magnitude > 0.01f) {
						GameObject G = new GameObject();
						G.transform.position = transform.position;
						G.transform.eulerAngles = new Vector3 (0, Camera.main.transform.eulerAngles.y + Mathf.Atan2(SSInput.LHor[0], SSInput.LVert[0])*Mathf.Rad2Deg, 0);
						Q = G.transform.rotation;
						Destroy (G);
					}
					transform.rotation = Quaternion.Slerp (transform.rotation, Q, (TurnSpeed/(Mathf.Pow(Anim.GetFloat("VSpeed")+1, 2)))*Time.deltaTime);
				}
			} else {
				//Stop Moving
				Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), 0, Acceleration * Time.deltaTime));
			}
			
			//Test If Floor In Front of You
			RaycastHit Hit1;
			if (Physics.BoxCast ((transform.position)+(transform.up*MaxStepHeight), new Vector3 (0.25f, 0.1f, 0.25f), Vector3.down, out Hit1, Quaternion.Euler (Vector3.zero), MaxStepHeight+1, LM)) {
				//Dont Fall If On Ground
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
				if (Hit1.normal.y > 0.75f) {
					transform.position = new Vector3 (transform.position.x, Hit1.point.y+0.125f, transform.position.z);
					if (!Anim.GetBool("OnGround")) {
						if (GetComponent<Rigidbody>().velocity.y < -20) {
							GetComponent<Health>().Wound += (-GetComponent<Rigidbody>().velocity.y / GetComponent<Health>().BaseResistance);
						}
					}
					Anim.SetBool("OnGround", true);
				} else {
					Anim.SetFloat("VSpeed", 0);
					Anim.SetBool("OnGround", false);
					transform.position += new Vector3 (Hit1.normal.x*10*Time.deltaTime, -10*Time.deltaTime, Hit1.normal.z*10*Time.deltaTime);
				}
			} else {
				//Fall if Not On Ground
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
				Anim.SetBool("OnGround", false);
				Anim.SetFloat("YVel", GetComponent<Rigidbody>().velocity.y/10);
				//Test If Wall in Front of You
				if (Physics.Raycast ((transform.position)+(transform.up*MaxStepHeight), transform.forward, out Hit1, 1, LM)) {
					Anim.SetFloat("VSpeed", 0);
					//Test If Not Higher Wall in Front of You
					if (!Physics.Raycast ((transform.position)+(transform.up*MaxStepHeight*2f), transform.forward, out Hit1, 1.5f, LM)) {
						GetComponent<Rigidbody>().isKinematic = true;
						while (Physics.Raycast ((transform.position)+(transform.up*MaxStepHeight*1.3f), transform.forward, 1.5f, LM)) {transform.Translate(0, 0.1f, 0);}
						while (!Physics.Raycast ((transform.position)+(transform.up*MaxStepHeight*1.2f), transform.forward, 1.5f, LM)) {transform.Translate(0, -0.1f, 0);}
						Anim.SetBool("Climb", true);
					}
				}
			}
		} else {
			//Maintain Velocity When Paused
			if (!WasPaused) {
				PrevVel = GetComponent<Rigidbody>().velocity;
				WasPaused = true;
			}
			transform.position = Pos;
			GetComponent<Rigidbody>().isKinematic = true;
		}
		
		//Display Stamina
		Stamina = Mathf.Clamp (Stamina, 0, maxStamina);
		if (Stamina > minStamina) {
			minStamina = 0;
			StamIma.color = Color.green;
		} else {
			minStamina = maxStamina-0.1f;
			StamIma.color = Color.red;
		}
		StamIma.transform.localScale = new Vector3 (Stamina/maxStamina, 1, 1);
	}
	
	public void NAtt (string ParamName) {
		//For The Animator To Reset Values
		Anim.SetBool(ParamName, false);
		if (ParamName == "Climb") {
			transform.Translate (0, 1, 1);
		}
	}
	
	void PlayGroundSound () {
		GetComponent<AudioSource>().Play();
	}
}
