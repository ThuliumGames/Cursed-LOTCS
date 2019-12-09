using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {

	Rigidbody RB;
	
	public Transform Cam;
	public Animator Anim;
	public Animator FollowerAnim;
	public AudioSource Voice;
	public AudioClip[] VoiceClips;
	public Transform HeadLook;
	public Transform HeadLookRot;
	public Transform Head;
	public Transform LFoot;
	public Transform RFoot;
	CapsuleCollider[] PlayerColliders;
	float Angle;
	
	public float Acceleration;
	public float TurnSpeed;
	public float JumpPower;
	public float MaxStepHeight;
	
	public float maxStamina;
	float minStamina;
	float Stamina;
	float Curse;
	public Image StamIma;
	public Image CurseIma;
	public Transform TargetImage;
	
	Vector3 Pos;
	Vector3 PrevPos;
	public LayerMask LM;
	
	bool WasPaused;
	Vector3 PrevVel;
	RaycastHit Hitikl;
	RaycastHit Hitikr;
	
	public AnimatorOverrideController AOC;
	bool CanLock;
	float LockNum;
	RaycastHit Hit;
	
	public Transform LPoint;
	public Transform RPoint;
	
	public GameObject RD;
	bool Ragdoll;
	float RDT;
	float Slope;
	[HideInInspector]
	public bool CanOG;
	[HideInInspector]
	public bool Jumped;
	[HideInInspector]
	public Collider AttackCollider;
	public Collider FistCollider;
	bool DoAttack;
	Health WasClosAI;
	public List<Health> CantTarget;
	float ResetTargets;
	bool CanAir;
	float TimePressingLS;
	
	void Start () {
		RB = GetComponent<Rigidbody>();
		Stamina = maxStamina;
		CantTarget = new List<Health>();
		System.Array.Resize(ref PlayerColliders, 3);
		PlayerColliders[0] = GetComponents<CapsuleCollider>()[0];
		PlayerColliders[1] = GetComponents<CapsuleCollider>()[1];
		PlayerColliders[2] = FollowerAnim.GetComponent<CapsuleCollider>();
	}
	
	void Update () {
		RB = GetComponent<Rigidbody>();

		GetComponent<Animator>().SetFloat ("ReverseAnim", 1);
		
		//Reset Attack Anim
		if (!GetComponentInChildren<Weapon>()) {
			if (!GetComponentInParent<Animator>().GetBool ("isCut")) {
				GetComponentInParent<Animator>().runtimeAnimatorController = AOC;
			}
			AttackCollider = FistCollider;
		} else {
			FistCollider.enabled = false;
		}
		
		if (Anim.GetBool("Attacking") && DoAttack) {
			if (!AttackCollider.enabled) {
				if (GetComponentInChildren<Weapon>()) {
					if (GetComponentInChildren<Weapon>().gameObject.tag == "CrystalSabre") {
						Curse++;
					}
				}
			}
			AttackCollider.enabled = true;
		} else {
			DoAttack = false;
			AttackCollider.enabled = false;
		}
		if (!GlobVars.Paused && !Anim.GetBool("isDying")) {
			
			if (Jumped) {
				Jumped = false;
			}
			
			//Maintain Velocity When Paused
			Pos = transform.position;
			RB.isKinematic = false;
			if (WasPaused) {
				RB.velocity = PrevVel;
				WasPaused = false;
			}
			
			if ((SSInput.LB[0] == "Down" && !Ragdoll) || (Ragdoll && RD.GetComponentInChildren<Rigidbody>().velocity.magnitude > 0.5f)) {
				Ragdoll = true;
				RDT = 5;
			}
			
			if (SSInput.RT[0] > 0.5f && Ragdoll) {
				RDT = 0;
			}
			
			RDT -= Time.deltaTime;
			
			if (RDT < 0 && Ragdoll) {
				Ragdoll = false;
				RD.SetActive(false);
				foreach(Renderer MR in GetComponentsInChildren<Renderer>()) {
					MR.enabled = true;
				}
			}
			
			if (!GlobVars.PlayerPaused && !GlobVars.Reading && !Ragdoll) {
				
				//Attack
				if (SSInput.X[0] == "Pressed" && !GetComponentInChildren<EquipItems>().usingSpecial) {
					Anim.SetBool("Attacking", true);
				}
				//Special
				if (GetComponentInChildren<Weapon>() && GetComponentInChildren<EquipItems>().usingSpecial) {
					if (SSInput.RB[0] == "Pressed") {
						Anim.SetBool("Attacking", true);
					}
				}
				
				//Moving
				if (Anim.GetBool("OnGround")) {
					if (!Physics.BoxCast ((transform.position+(transform.up/2)), new Vector3 (0.25f, 0.1f, 0.25f), Vector3.up, Quaternion.Euler (Vector3.zero), 1, LM)) {
						if (SSInput.LS[0] == "Pressed") {
							Anim.SetBool("Crouching", !Anim.GetBool("Crouching"));
						}
					} else {
						Anim.SetBool("Crouching", true);
					}
					if (!Physics.BoxCast ((FollowerAnim.transform.position+(FollowerAnim.transform.up/2)), new Vector3 (0.25f, 0.1f, 0.25f), Vector3.up, Quaternion.Euler (Vector3.zero), 1, LM)) {
						FollowerAnim.SetBool("Crouching", Anim.GetBool("Crouching"));
					} else {
						FollowerAnim.SetBool("Crouching", true);
					}
					if (Anim.GetBool("Crouching")) {
						if (SSInput.LS[0] == "Down") {
							TimePressingLS += Time.deltaTime;
						} else {
							TimePressingLS = 0;
						}
						
						foreach (CapsuleCollider Coll in PlayerColliders) {
							Coll.height = 0.5f;
							Coll.center = new Vector3 (0, 0.5f, 0);
						}
					} else {
						TimePressingLS = 0;
						foreach (CapsuleCollider Coll in PlayerColliders) {
							Coll.height = 2;
							Coll.center = new Vector3 (0, 1f, 0);
						}
					}
					if ((Mathf.Abs (SSInput.LHor[0]) > 0.05f || Mathf.Abs (SSInput.LVert[0]) > 0.05f)) {
						if (!Anim.GetBool("isTargeting")) {
							Anim.SetFloat("HSpeed", Mathf.Lerp (Anim.GetFloat("HSpeed"), 0, 10*Time.deltaTime));
							Angle = (Mathf.Atan2(SSInput.LHor[0], SSInput.LVert[0])*Mathf.Rad2Deg)+Cam.eulerAngles.y;
							if (SSInput.B[0] == "Down" && Stamina > minStamina) {
								Stamina -= Time.deltaTime*(int)(Anim.GetFloat("VSpeed")+0.25f);
								Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), Mathf.Clamp01(new Vector2(SSInput.LHor[0], SSInput.LVert[0]).magnitude), 20*Time.deltaTime));
							} else {
								Stamina += Time.deltaTime;
								Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), Mathf.Clamp01(new Vector2(SSInput.LHor[0], SSInput.LVert[0]).magnitude)/2, 20*Time.deltaTime));
							}
						} else {
							Angle = HeadLookRot.eulerAngles.y;
							Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), SSInput.LVert[0], 20*Time.deltaTime));
							Anim.SetFloat("HSpeed", Mathf.Lerp (Anim.GetFloat("HSpeed"), SSInput.LHor[0], 20*Time.deltaTime));
						}
					} else {
						Stamina += Time.deltaTime;
						Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), 0, 10*Time.deltaTime));
						Anim.SetFloat("HSpeed", Mathf.Lerp (Anim.GetFloat("HSpeed"), 0, 10*Time.deltaTime));
					}
					
					if (!Anim.GetBool("Attacking")) {
						transform.rotation = Quaternion.Lerp(transform.rotation, HeadLookRot.rotation, TurnSpeed/(((Mathf.Abs(Anim.GetFloat("VSpeed"))*10)+1)/10)*Time.deltaTime);
					}
				} else {
					foreach (CapsuleCollider Coll in PlayerColliders) {
						Coll.height = 2;
						Coll.center = new Vector3 (0, 1f, 0);
					}
				}
				//Rotation
				bool DoTarget = false;
				Health ClosAI = null;
				if (GameObject.FindObjectsOfType<Health>().Length > 1) {
					float AIDis = 20;
					if (WasClosAI != null) {
						ClosAI = WasClosAI;
						if (Vector3.Distance (transform.position, ClosAI.transform.position) < 40) {
							DoTarget = true;
						}
					} else {
						bool AllNo = true;
						foreach (Health A in GameObject.FindObjectsOfType<Health>()) {
							bool No = false;
							foreach (Health A2 in CantTarget) {
								if (A == A2) {
									No = true;
								}
							}
							if (!No && A != this.GetComponent<Health>()) {
								if (Vector3.Distance (transform.position, A.transform.position) < AIDis) {
									AllNo = false;
									AIDis = Vector3.Distance (transform.position, A.transform.position);
									ClosAI = A;
								}
							}
						}
						
						if (AllNo) {
							CantTarget.Clear();
						}
						
						if (ClosAI != null) {
							DoTarget = true;
						}
					}
				}
				
				if (SSInput.LT[0] >= 0.6f) {
					Anim.SetBool("isTargeting", true);
					Camera.main.rect = new Rect (0, Mathf.Lerp (Camera.main.rect.y, 0.1f, 10*Time.deltaTime), 1, Mathf.Lerp (Camera.main.rect.height, 0.8f, 10*Time.deltaTime));
					if (Anim.GetBool("OnGround")) {
						if (DoTarget) {
							HeadLookRot.LookAt(new Vector3 (ClosAI.transform.position.x, transform.position.y, ClosAI.transform.position.z));
							WasClosAI = ClosAI;
							
							TargetImage.GetComponentInChildren<Image>().enabled = true;
							TargetImage.position = Vector3.Lerp (TargetImage.position, ClosAI.transform.position+(Vector3.up*3f), 10*Time.deltaTime);
						} else {
							HeadLookRot.LookAt(transform.position + transform.forward);
							TargetImage.GetComponentInChildren<Image>().enabled = false;
						}
					}
				} else {
					if (SSInput.LT[0] == 0) {
						if (WasClosAI != null) {
							CantTarget.Add(WasClosAI);
						}
						WasClosAI = null;
						ResetTargets -= Time.deltaTime;
						if (ResetTargets <= 0) {
							TargetImage.GetComponentInChildren<Image>().enabled = false;
							TargetImage.position = transform.position+(transform.forward*10);
							CantTarget.Clear();
						}
					} else {
						ResetTargets = 1f;
					}
					Anim.SetBool("isTargeting", false);
					Camera.main.rect = new Rect (0, Mathf.Lerp (Camera.main.rect.y, 0.1f*SSInput.LT[0], 10*Time.deltaTime), 1, Mathf.Lerp (Camera.main.rect.height, 1-(SSInput.LT[0]/5), 10*Time.deltaTime));
				}
			} else {
				//Stop Moving
				Anim.SetFloat("VSpeed", Mathf.Lerp (Anim.GetFloat("VSpeed"), 0, Acceleration * Time.deltaTime));
				Anim.SetFloat("HSpeed", Mathf.Lerp (Anim.GetFloat("HSpeed"), 0, 10*Time.deltaTime));
				
				if (Ragdoll) {
					foreach(Renderer MR in GetComponentsInChildren<Renderer>()) {
						MR.enabled = false;
					}
					if (!RD.activeSelf) {
						RD.transform.position = transform.position;
						foreach (Transform G in RD.GetComponentsInChildren<Transform>()) {
							if (G.tag == "Respawn") {
								foreach (Transform G2 in GetComponentsInChildren<Transform>()) {
									if (G2.tag == "Respawn") {
										if (G.name == G2.name) {
											G.position = G2.position;
											G.rotation = G2.rotation;
										}
									}
								}
							}
						}
						foreach (Rigidbody RbRd in RD.GetComponentsInChildren<Rigidbody>()) {
							RbRd.velocity = RB.velocity;
						}
						RD.SetActive(true);
					}
					transform.position = RD.GetComponentInChildren<BoxCollider>().transform.position;
				}
			}
			
			//Test If Floor In Front of You
			
			if (!Anim.GetBool("isTargeting")) {
				HeadLookRot.eulerAngles = new Vector3 (0, Angle, 0);
			}
			if (Anim.GetFloat("VSpeed") < 0.1f) {
				TurnSpeed = 15;
			} else if (HeadLookRot.localEulerAngles.y < 5f || HeadLookRot.localEulerAngles.y > 355f) {
				TurnSpeed = 5;
			}
			Physics.BoxCast ((transform.position+transform.up), new Vector3 (0.25f, 0.1f, 0.25f), Vector3.down, out Hit, Quaternion.Euler (Vector3.zero), Mathf.Infinity, LM);
			Slope = 1 - Hit.normal.y;
			if (Hit.distance < 1.75f && Slope < 0.6f && CanOG) {
				if (Anim.GetFloat ("YVel") < -25) {
					this.GetComponent<Health>().Wound += 5;
					Ragdoll = true;
					RDT = 5;
					Anim.SetFloat("YVel", 0);
					return;
				} else if (Anim.GetFloat("YVel") < -17) {
					this.GetComponent<Health>().Wound += 2;
				}
				if (Anim.GetBool("OnGround")) {
					Anim.SetFloat("YVel", 0);
				} else {
					Anim.SetFloat("VSpeed", 0);
					Anim.SetFloat("HSpeed", 0);
				}
				Anim.SetBool("Slide", false);
				Anim.SetBool("OnGround", true);
				LockNum = 0;
				GameObject Gtmp = new GameObject();
				Gtmp.transform.position = transform.position;
				Gtmp.transform.LookAt (transform.position + Hit.normal);
				Gtmp.transform.Translate(0, 0, 1);
				Anim.SetFloat ("Slope", Slope*-Mathf.Clamp(transform.InverseTransformPoint(Gtmp.transform.position).z, -1, 1));
				Destroy(Gtmp);
				RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
				transform.position = new Vector3 (transform.position.x, Hit.point.y, transform.position.z);
				if (!GlobVars.PlayerPaused) {
					if (SSInput.Y[0] == "Pressed" && Stamina > minStamina) {
						if (!Physics.BoxCast ((transform.position+(transform.up/2)), new Vector3 (0.25f, 0.1f, 0.25f), Vector3.up, Quaternion.Euler (Vector3.zero), 1, LM)) {
							RB.constraints = RigidbodyConstraints.FreezeRotation;
							transform.eulerAngles = new Vector3 (0, Angle, 0);
							Jumped = true;
							CanOG = false;
							if (Anim.GetBool("Crouching") && TimePressingLS > 1) {
								Stamina -= 2+(Anim.GetFloat("Slope")*10);
								RB.velocity = new Vector3 (RB.velocity.x, 9, RB.velocity.z);
								Anim.SetBool("Crouching", false);
							} else {
								Stamina -= 0.5f+(Anim.GetFloat("Slope")*10);
								RB.velocity = new Vector3 (RB.velocity.x, 5, RB.velocity.z);
							}
							LockNum = RB.velocity.y;
							PlayASound(VoiceClips[0]);
						}
					}
				}
			} else {
				Anim.SetFloat("YVel", RB.velocity.y);
				Anim.SetBool("OnGround", false);
				RB.constraints = RigidbodyConstraints.FreezeRotation;
				RaycastHit Hit1;
				if (Physics.BoxCast ((transform.position+transform.up*2), new Vector3 (0.25f, 0.25f, 0.25f), transform.forward, out Hit1, Quaternion.Euler (Vector3.zero), 0.25f, LM)) {
					Anim.SetFloat("VSpeed", 0);
					Anim.SetFloat("HSpeed", 0);
				} else {
					Anim.SetFloat("VSpeed", Anim.GetFloat("VSpeed")-(Mathf.Clamp (Anim.GetFloat("VSpeed")*1000, -1, 1)*Time.deltaTime));
					Anim.SetFloat("HSpeed", Anim.GetFloat("HSpeed")-(Mathf.Clamp (Anim.GetFloat("HSpeed")*1000, -1, 1)*Time.deltaTime));
					if (Physics.BoxCast ((transform.position+transform.up*0.5f), new Vector3 (0.25f, 0.25f, 0.25f), transform.forward, out Hit1, Quaternion.Euler (Vector3.zero), 0.25f, LM)) {
						RB.velocity = new Vector3 (RB.velocity.x, LockNum-(Time.deltaTime*7), RB.velocity.z);
					}
				}
				
				if (CanAir) {
					GameObject GetCamRot = new GameObject();
					GetCamRot.transform.position = transform.position;
					GetCamRot.transform.LookAt (transform.position + new Vector3 (Cam.forward.x, 0, Cam.forward.z));
					GetCamRot.transform.Translate(SSInput.LHor[0], 0, SSInput.LVert[0]);
					Anim.SetFloat("VSpeed", Anim.GetFloat("VSpeed")+(transform.InverseTransformPoint(GetCamRot.transform.position).z*(Acceleration/8))*Time.deltaTime);
					Destroy(GetCamRot);
				}
				RB.velocity = new Vector3 (RB.velocity.x, Mathf.Clamp(RB.velocity.y, -53, Mathf.Clamp (LockNum, -53.1f, Mathf.Infinity)), RB.velocity.z);
				if (RB.velocity.y < LockNum) {
					LockNum = RB.velocity.y;
				}
			}
		} else {
			
			if (Anim.GetBool("isDying")) {
				Invoke ("Lose", 5);
			}
			
			//Maintain Velocity When Paused
			if (!WasPaused) {
				PrevVel = RB.velocity;
				WasPaused = true;
			}
			transform.position = Pos;
			RB.isKinematic = true;
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
		StamIma.fillAmount = Stamina/maxStamina;
		
		//Display Curse
		Curse = Mathf.Clamp (Curse, 0, 50);
		CurseIma.fillAmount = Curse/50;
		
		CanAir = true;
	}
	
	void OnCollisionStay () {
		CanAir = false;
	}
	
	void OnAnimatorIK (int LayerIndex) {
		Anim.SetLookAtWeight(0.5f);
		Anim.SetLookAtPosition(HeadLook.position);
		if (Anim.GetFloat("IkDo") == -1 || Anim.GetFloat("IkDo") == 2) {
			Anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
			Anim.SetIKPosition(AvatarIKGoal.LeftFoot, Hitikl.point);
			if (Anim.GetFloat("IkDo") == 2) {
				Physics.Raycast(LFoot.position+Vector3.up, Vector3.down, out Hitikl, Mathf.Infinity, LM);
			}
		} else {
			Physics.Raycast(LFoot.position+Vector3.up, Vector3.down, out Hitikl, Mathf.Infinity, LM);
		}
		
		if (Anim.GetFloat("IkDo") == 1 || Anim.GetFloat("IkDo") == 2) {
			Anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
			Anim.SetIKPosition(AvatarIKGoal.RightFoot, Hitikr.point);
			if (Anim.GetFloat("IkDo") == 2) {
				Physics.Raycast(RFoot.position+Vector3.up, Vector3.down, out Hitikr, Mathf.Infinity, LM);
			}
		} else {
			Physics.Raycast(RFoot.position+Vector3.up, Vector3.down, out Hitikr, Mathf.Infinity, LM);
		}
		
		if (LPoint != null) {
			Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
			Anim.SetIKPosition(AvatarIKGoal.LeftHand, LPoint.position);
		} else {
			Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
		}
		
		if (RPoint != null) {
			Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
			Anim.SetIKPosition(AvatarIKGoal.RightHand, RPoint.position);
		} else {
			Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
		}
		
		LPoint = null;
		RPoint = null;
	}
	
	public void NAtt (string ParamName) {
		//For The Animator To Reset Values
		Anim.SetBool(ParamName, false);
		if (ParamName == "Climb") {
			transform.Translate (0, 1, 1);
		}
	}
	
	public void YAtt () {
		//For The Animator To Reset Values
		DoAttack = true;
	}
	
	void PlayGroundSound () {
		GetComponent<AudioSource>().Play();
	}
	
	void PlayASound (AudioClip AC) {
		Voice.clip = AC;
		Voice.Play();
	}
	
	void Heal (int num) {
		if (num < 0) {
			this.GetComponent<Health>().Wound += num;
		} else {
			Stamina += (float)num;
			if (this.GetComponent<Health>().Wound <= 5 && this.GetComponent<Health>().Wound > 0) {
				this.GetComponent<Health>().Wound -= 1;
			}
		}
	}
	
	void Die () {
		if (!Anim.GetBool("isDying")) {
			Anim.SetBool("isDying", true);
			Anim.Play("Die");
		}
	}
	
	void Lose () {
		Application.LoadLevel("Dungeon");
	}
}
