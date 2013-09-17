//////////////////////////////////////////////////////////////////////////////
// Скрипт описывает поведение объекта кнопки, для вклюбчения которой
// необходимо принести несколько объектов типа cPickable
// v 1.0.0.2

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class cConditionalButton : c2StateButton {
	
	public bool DestroyConditionalObjects = false;
	
	[SerializeField]
	private cPickable[] ConditionsArray;
	
	public Dictionary<cPickable,bool> PickableConditionsList;
	
	void Awake(){
		
		PickableConditionsList = new Dictionary<cPickable, bool>();
		foreach(cPickable PickableObject in ConditionsArray){
			if (PickableObject){
				if (!PickableConditionsList.ContainsKey(PickableObject)){
					PickableConditionsList.Add(PickableObject, false);
				}
			}
		}
		
	}
	
	
	void  OnEnable(){		
		cMessenger<cInteractor>.AddListener("GetPlayerItem" , GetPlayerItem);
		cMessenger<GameObject>.AddListener("Push button", PushButton);
	}
	
	void OnDisable(){
		cMessenger<cInteractor>.RemoveListener("GetPlayerItem" , GetPlayerItem);
		cMessenger<GameObject>.RemoveListener("Push button", PushButton);
	}
		
	void GetPlayerItem(cInteractor interactor){
		// getting current player item
		if (Disabled){
			return;
		}
		
		if (interactor){
			
			if (interactor.PickedObject == null){
				return;
			}
			
			if (PickableConditionsList.ContainsKey(interactor.PickedObject)){
				
				if (!DestroyConditionalObjects){
					PickableConditionsList[interactor.PickedObject] = true;
				}
				else
				{
					PickableConditionsList.Remove(interactor.PickedObject);
					Destroy(interactor.PickedObject.gameObject.transform.parent.gameObject);
				}
				
			}
			
		}
		
		bool OpenCondition = true;
		if (!DestroyConditionalObjects){
			foreach(KeyValuePair<cPickable, bool> Condition in PickableConditionsList)
			{	
	 			OpenCondition &= Condition.Value;				
			}
		}else{
			OpenCondition = PickableConditionsList.Count == 0;
		}
			
		// Final condition
		if (OpenCondition){
			cMessenger<GameObject>.Invoke("Push button", transform.parent.gameObject);
		}
		
		
	}		
	
	// push button event handler. callback fuction.
	void PushButton(GameObject ButtonPressed){
		
		if (ButtonPressed == transform.parent.gameObject){
		
			if (!Disabled){ 
				if(State == eButtonState.Off){
					State = eButtonState.On;			
				} else{
					State = eButtonState.Off;
				}
				
			}
			
			cMessenger<MonoBehaviour>.Invoke("linkedButton", this);
			
		};
	
	}
}
