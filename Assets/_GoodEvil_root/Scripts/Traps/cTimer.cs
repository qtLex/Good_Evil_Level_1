using UnityEngine;
using System.Collections;

public class cTimer : MonoBehaviour {
	
	private string TempEventName = "";
	private float StandbyTime = 0f;
	private float TimeelapsedTime = 0f;
	private bool Eneble = false;
	private MonoBehaviour EventObj;
	
	// Update is called once per frame
	void Update () {
		if(!Eneble){
			return;
		}
		TimeelapsedTime = TimeelapsedTime + Time.deltaTime;
		if(TimeelapsedTime >= StandbyTime){
			Eneble = false;			
			cMessenger<MonoBehaviour>.Invoke(TempEventName, EventObj);
		}
	}
	
	public void StartTimer(string EventName, float dTime, MonoBehaviour Obj){
		TempEventName = EventName; Eneble = true; 
		StandbyTime = dTime; TimeelapsedTime = 0f;
		EventObj = Obj;
	}
}
