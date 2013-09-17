////////////////////////////////////////////////
/// Кнопка с 2мя состояниями
/// v 1.0.0.2

using UnityEngine;
using System.Collections;

public class c2StateButton : MonoBehaviour {
	
	public enum eButtonState{On, Off};
	
	public eButtonState State = eButtonState.Off;
	public bool Disabled = false;
	
	void OnEnable(){
		
		cMessenger<GameObject>.AddListener("Push button", PushButton);
		
	}
	
	void OnDisable(){
		
		cMessenger<GameObject>.RemoveListener("Push button", PushButton);
		
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
