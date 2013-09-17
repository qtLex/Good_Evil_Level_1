using UnityEngine;
using System.Collections;

public class cManagerGlobalState : MonoBehaviour {
	public cGlobalData.StateOfTheWorld State = cGlobalData.StateOfTheWorld.Good;
	
	
	void Start(){
		cMessenger<cGlobalData.StateOfTheWorld>.Invoke("Changed the state of the world",State);
	}
	
	void Update(){
		if(!Input.GetKeyUp(KeyCode.Tab)){
			return;
		}
		
		if(State == cGlobalData.StateOfTheWorld.Good){
			State = cGlobalData.StateOfTheWorld.Evil;
		}
		else{
			State = cGlobalData.StateOfTheWorld.Good;
		}
		
		cMessenger<cGlobalData.StateOfTheWorld>.Invoke("Changed the state of the world",State);
	}
}
