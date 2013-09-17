////////////////////////////////////////////
/// Класс описывает поведение двери с 2мя состояниями.
/// v 1.0.0.1
/// 
using UnityEngine;
using System.Collections;

public class cTriggeredDoor : MonoBehaviour {
	
	private enum doorState{Open, Closed, Disabled};
	
	public bool State = false;
	public c2StateButton[] TriggerButtons;
	public cConditionalButton[] ConditionalButtons;
		
	void OnEnable(){
	
		cMessenger<MonoBehaviour>.AddListener("linkedButton", ProcessMessage);
	}
	
	void OnDisable(){
		
		cMessenger<MonoBehaviour>.RemoveListener("linkedButton", ProcessMessage);
	}
		
	void ProcessMessage(MonoBehaviour behaviour){
		
		bool OpenCondition = true;
		bool NotYorButton = true;
		
		foreach (c2StateButton target in TriggerButtons){
			
			if (target == null) {continue;};
			
			if (target == behaviour){
				NotYorButton = false;	
			}
			
			if (target.State != c2StateButton.eButtonState.On){
				OpenCondition = false;
				break;
			}
			
		}
		
		foreach (cConditionalButton target in ConditionalButtons){
			
			if (target == null) {continue;};
			
			if (target == behaviour){
				NotYorButton = false;	
			}
			
			if (target.State != c2StateButton.eButtonState.On){
				OpenCondition = false;
				break;
			}
			
		}
		
		if (!NotYorButton){
			if (OpenCondition){ 
				animation.CrossFade("DoorOpen");
				Debug.Log("Open");
				
			}else{
				if (State){
					animation.CrossFade("DoorClose");			
					Debug.Log("Close");
				}
			}
			State = OpenCondition;
		}
			
	}
	
	void Update(){
		
		this.collider.enabled = !State;
		
	}
	
}
