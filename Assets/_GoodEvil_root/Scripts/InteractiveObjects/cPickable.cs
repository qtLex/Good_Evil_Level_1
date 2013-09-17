////////////////////////////////////////////
/// Класс описывает поведение объектов, которые cInteractor может подбирать.
/// v 1.0.0.1
/// 

using UnityEngine;
using System.Collections;

public class cPickable : MonoBehaviour {
	
	public GameObject Parent = null;
	
	void OnEnable(){
		cMessenger<GameObject, cInteractor>.AddListener("PickUnpick", PickUnpick);
	}
	
	void OnDisable(){
		cMessenger<GameObject, cInteractor>.RemoveListener("PickUnpick", PickUnpick);
	}
	
	void PickUnpick(GameObject ParentObject, cInteractor interactor){
		
		if (!Parent){
			Parent = ParentObject;
			interactor.PickedObject = this;
			interactor.CurrentTarget = null;
		}
		else{
			Parent = null;
			interactor.PickedObject = null;
		}
		
	}
	
	void Update(){
		
		GameObject ParentObject = transform.parent.gameObject;
		
		if (Parent){			
			ParentObject.rigidbody.Sleep();
			ParentObject.collider.enabled = false;
			this.collider.enabled = false;
			ParentObject.transform.position = Parent.transform.position;
			ParentObject.transform.rotation = Parent.transform.rotation;
		}else{
			ParentObject.rigidbody.WakeUp();
			ParentObject.collider.enabled = true;
			this.collider.enabled = true;
		}
		
	}
	
}
