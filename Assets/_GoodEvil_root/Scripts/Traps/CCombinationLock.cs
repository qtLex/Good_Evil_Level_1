/// <summary>
/// C combination lock. имитация кодового замка и здвух возможных значений элемента
/// </summary>

using UnityEngine;
using System.Collections;

public class CCombinationLock : MonoBehaviour {
	// public
	public c2StateButton[] TriggerButtonsOn;
	public c2StateButton[] TriggerButtonsOff;
	public bool            OnlyOnce = true;
	
	private bool State  = false;
	private bool Eneble = true;
	
	void OnEnable(){
	
		cMessenger<MonoBehaviour>.AddListener("linkedButton", ProcessMessage);
	}
	
	void OnDisable(){
		
		cMessenger<MonoBehaviour>.RemoveListener("linkedButton", ProcessMessage);
	}
		
	// Update is called once per frame
	void Update () {
		if(!Eneble){
			return;
		}
		
		if(!animation.isPlaying && State){
			cMessenger<string>.Invoke("MessageActivatePlatform", "lift_platforma");
			State = false;
			Eneble = !OnlyOnce;
		}
	}
	
	void ProcessMessage(MonoBehaviour behaviour){
		bool AllOn  = true;
		bool AllOff = true;
		
		if (!Eneble){
			return;
		}
		for(int i = 0; i < TriggerButtonsOn.Length; i++){
			if(TriggerButtonsOn[i].State == c2StateButton.eButtonState.Off){
				AllOn = false;
			}
		}
		
		for(int i = 0; i < TriggerButtonsOff.Length; i++){
			if(TriggerButtonsOff[i].State == c2StateButton.eButtonState.On){
				AllOff = false;
			}
		}
		
		if(AllOn && AllOff){
			State = true;
			animation.CrossFade("Open");
		}
	}
}
