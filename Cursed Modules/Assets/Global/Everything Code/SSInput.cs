using UnityEngine;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class SSInput : MonoBehaviour {
	
	public static float[] LT = {0,0,0,0};
	public static float[] RT = {0,0,0,0};
	public static string[] DUp = {"Up","Up","Up","Up"};
	public static string[] DRight = {"Up","Up","Up","Up"};
	public static string[] DDown = {"Up","Up","Up","Up"};
	public static string[] DLeft = {"Up","Up","Up","Up"};
	public static string[] Strt = {"Up","Up","Up","Up"};
	public static string[] Bck = {"Up","Up","Up","Up"};
	public static string[] LB = {"Up","Up","Up","Up"};
	public static string[] RB = {"Up","Up","Up","Up"};
	public static string[] A = {"Up","Up","Up","Up"};
	public static string[] B = {"Up","Up","Up","Up"};
	public static string[] X = {"Up","Up","Up","Up"};
	public static string[] Y = {"Up","Up","Up","Up"};
	public static string[] LS = {"Up","Up","Up","Up"};
	public static string[] RS = {"Up","Up","Up","Up"};
	public static float[] LHor = {0,0,0,0};
	public static float[] LVert = {0,0,0,0};
	public static float[] RHor = {0,0,0,0};
	public static float[] RVert = {0,0,0,0};

	GamePadState State;
	
	public static bool AnythingPressed;
	
    void Update() {

		AnythingPressed = false;
	
		for (int i = 0; i < 4; i++) {
			
			PlayerIndex Index = (PlayerIndex)i;
			
			State = GamePad.GetState (Index);
			
			if (State.IsConnected) {
				LT[i] = State.Triggers.Left;
				RT[i] = State.Triggers.Right;
				LHor[i] = State.ThumbSticks.Left.X;
				LVert[i] = State.ThumbSticks.Left.Y;
				RHor[i] = State.ThumbSticks.Right.X;
				RVert[i] = State.ThumbSticks.Right.Y;


				
				if (DUp[i] == "Up" || DUp[i] == "Released") {
					if ("" + State.DPad.Up == "Pressed") {
						DUp[i] = "Pressed";
						return;
					} else {
						DUp[i] = "Up";
					}
				}
				if (DUp[i] == "Down" || DUp[i] == "Pressed") {
					if ("" + State.DPad.Up == "Pressed") {
						DUp[i] = "Down";
						AnythingPressed = true;
					} else {
						DUp[i] = "Released";
						return;
					}
				}
				
				
				
				if (DRight[i] == "Up" || DRight[i] == "Released") {
					if ("" + State.DPad.Right == "Pressed") {
						DRight[i] = "Pressed";
						return;
					} else {
						DRight[i] = "Up";
					}
				}
				if (DRight[i] == "Down" || DRight[i] == "Pressed") {
					if ("" + State.DPad.Right == "Pressed") {
						DRight[i] = "Down";
						AnythingPressed = true;
					} else {
						DRight[i] = "Released";
						return;
					}
				}
				
				
				
				if (DDown[i] == "Up" || DDown[i] == "Released") {
					if ("" + State.DPad.Down == "Pressed") {
						DDown[i] = "Pressed";
						return;
					} else {
						DDown[i] = "Up";
					}
				}
				if (DDown[i] == "Down" || DDown[i] == "Pressed") {
					if ("" + State.DPad.Down == "Pressed") {
						DDown[i] = "Down";
						AnythingPressed = true;
					} else {
						DDown[i] = "Released";
						return;
					}
				}
				
				
				
				if (DLeft[i] == "Up" || DLeft[i] == "Released") {
					if ("" + State.DPad.Left == "Pressed") {
						DLeft[i] = "Pressed";
						return;
					} else {
						DLeft[i] = "Up";
					}
				}
				if (DLeft[i] == "Down" || DLeft[i] == "Pressed") {
					if ("" + State.DPad.Left == "Pressed") {
						DLeft[i] = "Down";
						AnythingPressed = true;
					} else {
						DLeft[i] = "Released";
						return;
					}
				}
				
				
				
				if (Strt[i] == "Up" || Strt[i] == "Released") {
					if ("" + State.Buttons.Start == "Pressed") {
						Strt[i] = "Pressed";
						return;
					} else {
						Strt[i] = "Up";
					}
				}
				if (Strt[i] == "Down" || Strt[i] == "Pressed") {
					if ("" + State.Buttons.Start == "Pressed") {
						Strt[i] = "Down";
						AnythingPressed = true;
					} else {
						Strt[i] = "Released";
						return;
					}
				}
				
				
				
				if (Bck[i] == "Up" || Bck[i] == "Released") {
					if ("" + State.Buttons.Back == "Pressed") {
						Bck[i] = "Pressed";
						return;
					} else {
						Bck[i] = "Up";
					}
				}
				if (Bck[i] == "Down" || Bck[i] == "Pressed") {
					if ("" + State.Buttons.Back == "Pressed") {
						Bck[i] = "Down";
						AnythingPressed = true;
					} else {
						Bck[i] = "Released";
						return;
					}
				}
				
				
				if (LB[i] == "Up" || LB[i] == "Released") {
					if ("" + State.Buttons.LeftShoulder == "Pressed") {
						LB[i] = "Pressed";
						return;
					} else {
						LB[i] = "Up";
					}
				}
				if (LB[i] == "Down" || LB[i] == "Pressed") {
					if ("" + State.Buttons.LeftShoulder == "Pressed") {
						LB[i] = "Down";
						AnythingPressed = true;
					} else {
						LB[i] = "Released";
						return;
					}
				}
				
				
				
				if (RB[i] == "Up" || RB[i] == "Released") {
					if ("" + State.Buttons.RightShoulder == "Pressed") {
						RB[i] = "Pressed";
						return;
					} else {
						RB[i] = "Up";
					}
				}
				if (RB[i] == "Down" || RB[i] == "Pressed") {
					if ("" + State.Buttons.RightShoulder == "Pressed") {
						RB[i] = "Down";
						AnythingPressed = true;
					} else {
						RB[i] = "Released";
						return;
					}
				}
				
				
				
				if (A[i] == "Up" || A[i] == "Released") {
					if ("" + State.Buttons.A == "Pressed") {
						A[i] = "Pressed";
						return;
					} else {
						A[i] = "Up";
					}
				}
				if (A[i] == "Down" || A[i] == "Pressed") {
					if ("" + State.Buttons.A == "Pressed") {
						A[i] = "Down";
						AnythingPressed = true;
					} else {
						A[i] = "Released";
						return;
					}
				}
				
				
				
				if (B[i] == "Up" || B[i] == "Released") {
					if ("" + State.Buttons.B == "Pressed") {
						B[i] = "Pressed";
						return;
					} else {
						B[i] = "Up";
					}
				}
				if (B[i] == "Down" || B[i] == "Pressed") {
					if ("" + State.Buttons.B == "Pressed") {
						B[i] = "Down";
						AnythingPressed = true;
					} else {
						B[i] = "Released";
						return;
					}
				}
				
				
				
				if (X[i] == "Up" || X[i] == "Released") {
					if ("" + State.Buttons.X == "Pressed") {
						X[i] = "Pressed";
						return;
					} else {
						X[i] = "Up";
					}
				}
				if (X[i] == "Down" || X[i] == "Pressed") {
					if ("" + State.Buttons.X == "Pressed") {
						X[i] = "Down";
						AnythingPressed = true;
					} else {
						X[i] = "Released";
						return;
					}
				}
				
				
				
				if (Y[i] == "Up" || Y[i] == "Released") {
					if ("" + State.Buttons.Y == "Pressed") {
						Y[i] = "Pressed";
						return;
					} else {
						Y[i] = "Up";
					}
				}
				if (Y[i] == "Down" || Y[i] == "Pressed") {
					if ("" + State.Buttons.Y == "Pressed") {
						Y[i] = "Down";
						AnythingPressed = true;
					} else {
						Y[i] = "Released";
						return;
					}
				}
				
				
				
				if (LS[i] == "Up" || LS[i] == "Released") {
					if ("" + State.Buttons.LeftStick == "Pressed") {
						LS[i] = "Pressed";
						return;
					} else {
						LS[i] = "Up";
					}
				}
				if (LS[i] == "Down" || LS[i] == "Pressed") {
					if ("" + State.Buttons.LeftStick == "Pressed") {
						LS[i] = "Down";
						AnythingPressed = true;
					} else {
						LS[i] = "Released";
						return;
					}
				}
				
				
				
				if (RS[i] == "Up" || RS[i] == "Released") {
					if ("" + State.Buttons.RightStick == "Pressed") {
						RS[i] = "Pressed";
						return;
					} else {
						RS[i] = "Up";
					}
				}
				if (RS[i] == "Down" || RS[i] == "Pressed") {
					if ("" + State.Buttons.RightStick == "Pressed") {
						RS[i] = "Down";
						AnythingPressed = true;
					} else {
						RS[i] = "Released";
						return;
					}
				}
			}
		}
    }
}
