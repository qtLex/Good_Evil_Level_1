///////////////////////////////////////////////////
/// Класс описывает движение персонажа в 2х степенях свободы.
/// v 1.0.0.3 
/// 
///  1.0.0.2 // правки mors - убрал вывод в дебаг
///  1.0.0.3 // Добавлен рэгдол и смерт персонажа.

using UnityEngine;
using System.Collections;

public class cPlayerController : MonoBehaviour 
{
	// public
	public int Force                  = 60;
	public int MaxSpeed               = 6;
	public int JumpImpuls             = 15;
	public int SlopeVectorJump        = 45;
	
	public float AllowanceTestFlight  = 0.11F;
	public float AllowanceMaxVelocity = 2.5F;
	
	public bool MoveInFlight          = false;
	public bool Fly                   = false;
	public bool NearTheWall           = false;
	public bool UsingControls         = false;
	
	// private
	private Transform tPlayer;
	private Rigidbody rbPlayer;
	private Animation aPlayer;
	
	private bool Dead = false;
	private float TimerDead = 0.7f;
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.I)){			
			cMessenger<string>.Invoke("MessageInvertParallax","Transporter1");
			cMessenger<string>.Invoke("MessageInvertParallax","Transporter2");	
		}	
	}
	
	void FixedUpdate () 
	{			
		if ((Dead)&&(TimerDead <= 0)){
			cMessenger.Invoke("Die");
			TimerDead = 0.7f;
			return;
		}
		else if (Dead)
		{
			TimerDead -= Time.deltaTime;
			return;
		}
		
		ForceMove();
		VelocityControl();
	}

	void ForceMove()
	{
		
			UsingControls = false;
		
			if((Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) && !Fly)
			{
				rbPlayer.velocity = Vector3.zero;
			
				UsingControls = true;
			}	
			
			if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
			{
				// поворачиваем персонажа
				var NeedRotation = false;
				var Vec = new Vector3(tPlayer.position.x,0,0);
				
				if(Input.GetKey(KeyCode.RightArrow)){Vec = new Vector3(tPlayer.position.x + Force,tPlayer.position.y,0);}
				
				else if( Input.GetKey(KeyCode.LeftArrow)){Vec = new Vector3(tPlayer.position.x - Force,tPlayer.position.y,0);	}
						
				var RotationNew = Quaternion.Slerp(tPlayer.rotation,Quaternion.LookRotation(Vec - tPlayer.position),180);
				NeedRotation = !(tPlayer.rotation == RotationNew);			
				
				// двигаем персонажа
				if(!NeedRotation)
				{					
					if(rbPlayer.velocity.x <= MaxSpeed && rbPlayer.velocity.x >= -MaxSpeed && (!Fly||MoveInFlight)&&!NearTheWall)
					{
						rbPlayer.AddForce(new Vector3(tPlayer.forward.x,0,0)*Force,ForceMode.Force);
						if (!Fly){
						//	aPlayer.CrossFade("Run");
						}
					}
					UsingControls = true;
				}
				else
				{
					tPlayer.rotation = RotationNew;
					// в последствии запуск анимации персонажа
				}
			}
		
			if(Input.GetKey(KeyCode.Space) && !Fly)
			{
				UsingControls = true;
				var direction = new Vector3(0,0,0);
				if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)){
					if(tPlayer.forward.x > 0){
						direction = Quaternion.Euler(0,0,SlopeVectorJump) * tPlayer.forward * JumpImpuls;
					}
					else{
						direction = Quaternion.Euler(0,0,-SlopeVectorJump) * tPlayer.forward * JumpImpuls;
					}
				}
				else
				{
					direction = tPlayer.transform.up * JumpImpuls;
				}
				
				if (transform.forward.x > 0)
					 {aPlayer.Play("JumpPose_R");}
			   	else {aPlayer.Play("JumpPose_L");}

				rbPlayer.AddForce(direction, ForceMode.Impulse);
	
				Fly = true;
			}
	}
	
	void VelocityControl()
	{
		if(!(rbPlayer.velocity.x <= MaxSpeed*AllowanceMaxVelocity && rbPlayer.velocity.x >= -MaxSpeed*AllowanceMaxVelocity))
		{
			float VelocityX = MaxSpeed*AllowanceMaxVelocity ;
			if(rbPlayer.velocity.x < 0){VelocityX = -VelocityX;}
			rbPlayer.velocity  = new Vector3(VelocityX, rbPlayer.velocity.y,rbPlayer.velocity.z);
		}
	}
	
	void OnCollisionEnter()
	{	
		if (Dead){
			return;
		}
		
		FlightTest();
		NearTheWallTest();
		
		if(!Fly){
			if (transform.forward.x > 0)
			{//aPlayer.Play("Idle1_R");
			}
		else{//aPlayer.Play("Idle1_L");
			}
		}
	}
	
	void OnCollisionStay()
	{
		if (Dead){
			return;
		}
		
		FlightTest();
		NearTheWallTest();
	}
	
	void OnCollisionExit()
	{
		if (Dead){
			return;
		}
		
		FlightTest();
		NearTheWallTest();
		if(Fly)
		{
		//	if (transform.forward.x > 0)
		//		 {aPlayer.CrossFade("JumpPose_R");}
		//	else {aPlayer.CrossFade("JumpPose_L");}
		}
		
	}
	
	void FlightTest()
	{	
		// выпускаем луч
		bool hit = Physics.Raycast(tPlayer.position,  -Vector3.up,tPlayer.localScale.y+AllowanceTestFlight, (int)(1 << LayerMask.NameToLayer("Default")));
		Fly = !hit;	
	}
	
	void NearTheWallTest()
	{
		// выпускаем луч
		bool hit = Physics.Raycast(tPlayer.position,  tPlayer.forward,tPlayer.localScale.x+AllowanceTestFlight, (int)(1 << LayerMask.NameToLayer("Default")));
		NearTheWall = hit;	
	}
	
	// Mors
	void OnEnable(){
		cMessenger.AddListener("Die",PlayerDie);
	}
	
	void OnDisable(){
		cMessenger.RemoveListener("Die",PlayerDie);
	}
	
	void Awake () 
	{
	
		tPlayer  = transform;
		rbPlayer = rigidbody;
		aPlayer  = transform.FindChild("robo").animation;
	
		Transform RootBone = transform.Find("robo/Root/robo_controller/SpineTopBone");
		
		Rigidbody[] ChildBones = RootBone.GetComponentsInChildren<Rigidbody>();
		
		foreach (Rigidbody ChildBone in ChildBones){
			
			ChildBone.isKinematic = true;
			ChildBone.detectCollisions = false;
			
		}
		
		Collider[] ChildBonesColliders = RootBone.GetComponentsInChildren<Collider>();
		
		foreach (Collider ChildBone in ChildBonesColliders){
			ChildBone.enabled = false;
		}
		
		//aPlayer.Play("Idle1_R");
	}
	
	void PlayerDie(){
		
		if (!Dead){
			Dead = true;
			aPlayer.Stop();
			
			rigidbody.isKinematic = true;
			rigidbody.detectCollisions = false;
			
			collider.enabled = false;
			
			Transform RootBone = transform.Find("robo/Root/robo_controller/SpineTopBone");
			Rigidbody[] ChildBones = RootBone.GetComponentsInChildren<Rigidbody>();
			c2DCameraFollower Follower = Camera.main.GetComponent<c2DCameraFollower>();
			
			if (Follower){
				Follower.Target = RootBone.gameObject;		
				Follower.SmoothTime = 0.1f;
				Follower.CameraDistance -= 5.0f;
			}
			
			Vector3 Impulse = Vector3.zero;
			
			foreach (Rigidbody ChildBone in ChildBones){
				ChildBone.isKinematic = false;
				ChildBone.detectCollisions = true;
				ChildBone.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
				
				Impulse = rigidbody.velocity.normalized * (ChildBone.mass * rigidbody.velocity.magnitude);
				
				ChildBone.AddForce(Impulse,ForceMode.Impulse);
			}
			
			Collider[] ChildBonesColliders = RootBone.GetComponentsInChildren<Collider>();
			
			foreach (Collider ChildBone in ChildBonesColliders){
				ChildBone.enabled = true;
			}
			
		}else{
			Dead = false;
			
			rigidbody.isKinematic = false;
			rigidbody.detectCollisions = true;
			
			collider.enabled = true;
			
			Transform RootBone = transform.Find("robo/Root/robo_controller/SpineTopBone");
			Rigidbody[] ChildBones = RootBone.GetComponentsInChildren<Rigidbody>();
			c2DCameraFollower Follower = Camera.main.GetComponent<c2DCameraFollower>();
			
			if (Follower){
				Follower.Target = gameObject;		
				Follower.SmoothTime = 0.3f;
				Follower.CameraDistance += 5.0f;
			}
			
			foreach (Rigidbody ChildBone in ChildBones){
				
				ChildBone.isKinematic = true;
				ChildBone.detectCollisions = false;
				
			}
			
			Collider[] ChildBonesColliders = RootBone.GetComponentsInChildren<Collider>();
			
			foreach (Collider ChildBone in ChildBonesColliders){
				ChildBone.enabled = false;
			}
			
			//aPlayer.Play("Idle1_R");
			
			this.transform.position = CGlobalStatic.SaveCurrentPosition;
		}
		
	}
	// Mors
}
