/////////////////////////////////////////////////////////////////////
/// Зона смещения персонажа. локальный ветер.
/// v. 1.0.0.1


using UnityEngine;
using System.Collections;

public class cVent_UpDown : MonoBehaviour {
	
	public float Force = 20;
	
	public ForceMode Mode = ForceMode.Acceleration;
	
	public bool DisableGravityOnTrigger = false;
	
	public bool Enabled
	{
		set {
			pEnabled = value; 
			if (!pEnabled){
				particleSystem.Stop();
			}
			else{
				particleSystem.Play();
			}
			
		}
		get {return pEnabled;}
	}
	
	//explicit serialization
	[SerializeField]
	private bool pEnabled = true;
	
	[SerializeField]
	private Vector3 Direction = Vector3.up;
	
	// Use this for initialization
	void Awake () {
		
		if (!pEnabled){
		    particleSystem.Stop();
		}
		else{
			particleSystem.Play();
		};
		
		Direction = transform.forward;
		
	}
	
	void OnTriggerStay (Collider other) {
		
		if (pEnabled){
		other.attachedRigidbody.AddForce(Direction * Force, Mode);};
		
	}
	
	void OnTriggerEnter (Collider other) {
	
		
		if (pEnabled && DisableGravityOnTrigger){
		other.attachedRigidbody.useGravity = false;};
		
		if (other.gameObject.GetComponentInChildren<cInteractor>()){ 
			cPlayerAnimationController.InTheVent = true;
		}
		
	}
	
	void OnTriggerExit (Collider other) {
	
		if (pEnabled && DisableGravityOnTrigger){
		other.attachedRigidbody.useGravity = true;}
		
		if (other.gameObject.GetComponentInChildren<cInteractor>()){ 
			cPlayerAnimationController.InTheVent = false;
		}
		
	}
	
}
