//////////////////////////////////////////////// 
/// Класс описывает обработку взаимодействия персонажа с различными ообъектами.
/// v 1.0.0.2
/// 
using UnityEngine;
using System.Collections;

public class cInteractor : MonoBehaviour {
	
	public GameObject CurrentTarget = null;
	public GameObject PickAnchor = null;
	public cPickable PickedObject = null;
	
	void Update(){
		
		if (Input.GetKeyDown(KeyCode.D)){
			cMessenger.Invoke("Die");
		}
		
		if (Input.GetKeyDown(KeyCode.E)){
		
			if (!CurrentTarget){
			
				if (PickedObject != null){
					cMessenger<GameObject, cInteractor>.Invoke("PickUnpick", this.PickAnchor, this);
					Debug.Log("Drop an item");
				}
			}
			else{
		
				if (CurrentTarget.GetComponentInChildren<cConditionalButton>()){
					cMessenger<cInteractor>.Invoke("GetPlayerItem" , this);
				}				
				else if (CurrentTarget.GetComponentInChildren<c2StateButton>()){
					cMessenger<GameObject>.Invoke("Push button", CurrentTarget);
				}
				else if (CurrentTarget.GetComponentInChildren<cPickable>()){
					cMessenger<GameObject, cInteractor>.Invoke("PickUnpick", this.PickAnchor, this);												
					Debug.Log("Pick an item");
				}
			}
		}
					
	}	
		
	void OnTriggerStay (Collider other){
		
		if (!CurrentTarget){
			if (PickedObject != null && other.gameObject != PickedObject.gameObject){
				CurrentTarget = other.gameObject.transform.parent.gameObject;	
				return;
			}
			
			if (!PickedObject){
				if (other.gameObject.transform.parent){
				CurrentTarget = other.gameObject.transform.parent.gameObject;
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		
		if (!CurrentTarget){
			CurrentTarget = other.gameObject.transform.parent.gameObject;	
			
		}
		else if (other.gameObject.GetComponent<cPickable>()
			     && (!CurrentTarget.GetComponentInChildren<cPickable>())){
			CurrentTarget = other.gameObject.transform.parent.gameObject;	
		}
				
	}
	
	void OnTriggerExit(Collider other){
		
		if (CurrentTarget == other.gameObject.transform.parent.gameObject){
			CurrentTarget = null;	
		}
	}
	
}
