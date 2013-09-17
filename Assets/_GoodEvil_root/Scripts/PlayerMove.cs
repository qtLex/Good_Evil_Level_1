///////////////////////////////////////////////////
/// Класс описывает движение персонажа в 2х степенях свободы.
/// v 1.0.0.3 
/// 
///  1.0.0.2 // правки mors - убрал вывод в дебаг
///  1.0.0.3 // Добавлен рэгдол и смерт персонажа.

using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour 
{
	// public
	public int Force                  = 50;
	public int MaxSpeed               = 6;
	public int JumpImpuls             = 30;
	
	public float AllowanceTestFlight  = 0.11F;
	public float AllowanceMaxVelocity = 2.5F;
	
	public bool MoveInFlight          = false;
	public bool Fly                   = false;
	public bool NearTheWall;
	
	// private
	private Transform tPlayer;
	private Rigidbody rbPlayer;
	
	private bool Dead = false;
	
	public bool UsingControl = false;
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.I)){			
			cMessenger<string>.Invoke("MessageInvertParallax","Transporter1");
			cMessenger<string>.Invoke("MessageInvertParallax","Transporter2");	
		}	
	}
	
	void FixedUpdate () 
	{			
		if (Dead){
			return;
		}
		
		ForceMove();
		VelocityControl();
	}

	void ForceMove()
	{		
		 	UsingControl = false;
		
			if((Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) && !Fly)
			{
				UsingControl = true;
				rbPlayer.velocity = Vector3.zero;
			}	
			
			if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
			{
				// поворачиваем персонажа
				var NeedRotation = false;
				var Vec = new Vector3(tPlayer.position.x,0,0);
				
				if(Input.GetKey(KeyCode.RightArrow)){Vec = new Vector3(tPlayer.position.x + Force,tPlayer.position.y,0);}
				
				else if( Input.GetKey(KeyCode.LeftArrow)){Vec = new Vector3(tPlayer.position.x - Force,tPlayer.position.y,0);	}
			
				//Debug.DrawLine(tPlayer.position, Vec , Color.red);
			
				var RotationNew = Quaternion.Slerp(tPlayer.rotation,Quaternion.LookRotation(Vec - tPlayer.position),180);
				NeedRotation = !(tPlayer.rotation == RotationNew);			
				
				// двигаем персонажа
				if(!NeedRotation)
				{					
					if(rbPlayer.velocity.x <= MaxSpeed && rbPlayer.velocity.x >= -MaxSpeed && (!Fly||MoveInFlight)&&!NearTheWall)
					{
						rbPlayer.AddForce(new Vector3(tPlayer.forward.x,0,0)*Force*(Time.deltaTime*100),ForceMode.Force);
					}
				}
				else
				{
					tPlayer.rotation = RotationNew;
					// в последствии запуск анимации персонажа
				}
				UsingControl = true;
			}
		
			if(Input.GetKey(KeyCode.Space) && !Fly)
			{
				var direction = new Vector3(0,0,0);
				if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)){
					direction = new Vector3(tPlayer.forward.x*JumpImpuls/1.4f, JumpImpuls/1.4f, 0);
				}
				else
				{
					direction = new Vector3(0, JumpImpuls, 0);
				}
				rbPlayer.AddForce(direction*JumpImpuls*Time.deltaTime, ForceMode.Impulse);
				Fly = true;
				UsingControl = true;
			}
		//Debug.Log(rbPlayer.velocity);
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
	
	}
	
	void FlightTest()
	{	
		// выпускаем луч
		bool hit = Physics.Raycast(tPlayer.position,  -Vector3.up,tPlayer.localScale.y+AllowanceTestFlight, (int)(1 << LayerMask.NameToLayer("Default")));
		//Debug.Log(hit);
		Fly = !hit;	
	}
	
	void NearTheWallTest()
	{
		// выпускаем луч
		bool hit = Physics.Raycast(tPlayer.position,  tPlayer.forward,tPlayer.localScale.x+AllowanceTestFlight, (int)(1 << LayerMask.NameToLayer("Default")));
		//Debug.Log(hit);
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
	
		Transform RootBone = transform.FindChild("Bip001");
		
		Rigidbody[] ChildBones = RootBone.GetComponentsInChildren<Rigidbody>();
		
		foreach (Rigidbody ChildBone in ChildBones){
			
			ChildBone.isKinematic = true;
			ChildBone.detectCollisions = false;
			
		}
		
		Collider[] ChildBonesColliders = RootBone.GetComponentsInChildren<Collider>();
		
		foreach (Collider ChildBone in ChildBonesColliders){
			ChildBone.enabled = false;
		}
		
		animation.Play("idle");
	}
	
	void PlayerDie(){
		
		if (!Dead){
			Dead = true;
			animation.Stop();
			
			rigidbody.isKinematic = true;
			rigidbody.detectCollisions = false;
			
			collider.enabled = false;
			
			Transform RootBone = transform.FindChild("Bip001");
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
			
			Transform RootBone = transform.FindChild("Bip001");
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
			
		}
		
	}
	// Mors
}
