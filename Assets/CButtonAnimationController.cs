using UnityEngine;
using System.Collections;

public class CButtonAnimationController : MonoBehaviour {
	public bool State = false;
	public c2StateButton[] TriggerButtons;
	
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
	if (!NotYorButton){
			if (OpenCondition){ 
				animation.CrossFade("PullDown");
				
			}
			else{
					animation.CrossFade("PullUp");
			}
		}
	}
}
