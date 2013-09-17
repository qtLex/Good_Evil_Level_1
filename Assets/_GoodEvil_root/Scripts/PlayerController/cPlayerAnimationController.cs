using UnityEngine;
using System.Collections;

public class cPlayerAnimationController : MonoBehaviour {
	
	
	public cPlayerController PlayerScript;
	public Rigidbody rbPlayer;
	public Animation aPlayerAnimation;
	
	public float RunVelocityCorrector = 0.0f;
	
	private bool InTheAir      = false;
	private bool NearTheWall   = false;
	private bool UsingControls = false;
	
	private float PrevVelocity = 0.0f;
	
	
	public static bool InTheVent = false;
	private double Delay = 0.0f;
	
	// For player move read
	public static bool animDontMove = false;
	
	
	void Awake () {
		
		aPlayerAnimation = (Animation)rbPlayer.gameObject.GetComponentInChildren(typeof(Animation));
	
	}
	
	
	void Update () {
		
		if (!PlayerScript || !rbPlayer || !aPlayerAnimation){return;};
		
		if (Delay >= 0){
		
			Delay -= Time.deltaTime;
			return;
			
		}else{
			
			Delay = 0;	
			animDontMove = false;
			
		};
		
		NearTheWall   = PlayerScript.NearTheWall;
		InTheAir      = PlayerScript.Fly;	
		UsingControls = PlayerScript.UsingControls;
		
		// y - down, / x - left / z - in camera.
		
		Vector3 PlayerVelocity = rbPlayer.velocity;
		
		bool MovingUp   = PlayerVelocity.y > 0;
		bool MovingLeft = PlayerVelocity.x < 0;
				
		bool Standing = PlayerVelocity == Vector3.zero;
		
		bool lookongLeft = PlayerScript.transform.forward.x < 0;
		
		if (InTheAir){
			
			if (InTheVent){
				
				if (lookongLeft){
					aPlayerAnimation.CrossFade("FreeFly_L");
				}
				else{
					aPlayerAnimation.CrossFade("FreeFly_R");
				};	
				return;
			}
			
			if (MovingUp){
				
				if (lookongLeft){
					aPlayerAnimation.CrossFade("JumpPose_L");
				}
				else{
					aPlayerAnimation.CrossFade("JumpPose_R");
				};
		
			}
			// falling down
			else{
				
				if (PlayerVelocity.sqrMagnitude > 180){
					
					if (lookongLeft){
						aPlayerAnimation.CrossFade("FreeFly_L");
					}
					else{
						aPlayerAnimation.CrossFade("FreeFly_R");
					};										
					
					
				}else
				{
					if (lookongLeft){
						aPlayerAnimation.CrossFade("FallPose_L");
					}
					else{
						aPlayerAnimation.CrossFade("FallPose_R");
					};										
				}
				
			}
			
		}
		// on the foot
		else{
			
			if (Input.GetKeyDown(KeyCode.E)){
				
				if (lookongLeft){
					aPlayerAnimation.CrossFade("Action_L");		
					Delay = aPlayerAnimation["Action_L"].length;
					animDontMove = true;
				}else{
					aPlayerAnimation.CrossFade("Action_R");
					Delay = aPlayerAnimation["Action_R"].length;
					animDontMove = true;
				}				
					
				return;
			}
			
			if (Standing && !UsingControls){
			
				if (aPlayerAnimation.IsPlaying("Action_L") || aPlayerAnimation.IsPlaying("Action_R")){
					return;
				}
				
				if (lookongLeft){
					aPlayerAnimation.CrossFade("Idle1_L");
				}else{
					aPlayerAnimation.CrossFade("Idle1_R");
				}
				
				return;
			};
			
			if (MovingLeft){
				if (!NearTheWall){
					if (UsingControls){
						aPlayerAnimation["Run"].speed = rbPlayer.velocity.magnitude * RunVelocityCorrector;
						aPlayerAnimation.CrossFade("Run");
					}else{
						if (PlayerVelocity.sqrMagnitude > 20){
							aPlayerAnimation.CrossFade("FloorSlide_L");
						}else{
							aPlayerAnimation.CrossFade("Idle1_L");
						}
					}
				}
				else
				{
					
					if (!aPlayerAnimation.IsPlaying("Push_L"))
					{
						aPlayerAnimation.CrossFade("Push_L_start");
						aPlayerAnimation.CrossFadeQueued("Push_L");
					}
					
				}
			}
			else
			{
				if (!NearTheWall){
					if (UsingControls){
						aPlayerAnimation["Run"].speed = rbPlayer.velocity.magnitude * RunVelocityCorrector;
						aPlayerAnimation.CrossFade("Run");
					}else{
						if (PlayerVelocity.sqrMagnitude > 20){
							aPlayerAnimation.CrossFade("FloorSlide_R");
						}else{
							aPlayerAnimation.CrossFade("Idle1_R");
						}
					}
				}
				else
				{
					
					if (!aPlayerAnimation.IsPlaying("Push_R"))
					{
						aPlayerAnimation.CrossFade("Push_R_start");
						aPlayerAnimation.CrossFadeQueued("Push_R");
					};
					
				};
			};
			
		};
		
		PrevVelocity = rbPlayer.velocity.sqrMagnitude;
		
	}
	
	
}
