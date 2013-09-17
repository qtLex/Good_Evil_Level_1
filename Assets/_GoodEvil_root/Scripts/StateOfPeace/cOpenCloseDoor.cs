////////////////////////////////////////////
/// Класс открывает и закрывает двери в зависимости от состояния мира.
/// v 1.0.0.1
/// 

using UnityEngine;
using System.Collections;

public class cOpenCloseDoor : MonoBehaviour {

	public cGlobalData.StateOfTheWorld OpenState  = cGlobalData.StateOfTheWorld.Evil;
	public cGlobalData.StateOfTheWorld CurrentState = cGlobalData.StateOfTheWorld.Good; 
	public bool                        DisebledColliders  = true;
	
	
	void OnEnable(){
		cMessenger<cGlobalData.StateOfTheWorld>.AddListener("Changed the state of the world", ChangedState);
	}
	
	void OnDisable(){
		cMessenger<cGlobalData.StateOfTheWorld>.RemoveListener("Changed the state of the world", ChangedState);
	}
	
	void ChangedState(cGlobalData.StateOfTheWorld State){
		if(CurrentState == State){
			return;
		}
		
		CurrentState = State;
		bool NeedOpen = (State == OpenState);
		
		string AnimationName = "";
		if(NeedOpen){
			AnimationName = "DoorOpen";
		}
		else{
			AnimationName = "DoorClose";
		}
		
		animation.CrossFade(AnimationName);
		
		collider.enabled = !NeedOpen;
		
	}
}
