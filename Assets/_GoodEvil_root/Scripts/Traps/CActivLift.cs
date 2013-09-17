/// <summary>
/// C activ lift. Запуск ливта при нахождении на нем
/// </summary>


using UnityEngine;
using System.Collections;

public class CActivLift : MonoBehaviour {

	void OnEnable(){
		
		cMessenger<GameObject>.AddListener("Push button", PushButton);
		
	}
	
	void OnDisable(){
		
		cMessenger<GameObject>.RemoveListener("Push button", PushButton);
		
	}
	
	// push button event handler. callback fuction.
	void PushButton(GameObject ButtonPressed){
		
		if (ButtonPressed == transform.parent.gameObject){
			cMessenger<string>.Invoke("MessageActivatePlatform", "lift_platforma");
		}
		
	}
}
