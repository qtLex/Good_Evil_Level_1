using UnityEngine;
using System.Collections;

public class cPlatformController : MonoBehaviour {
	
	public bool Stage = false;
	public float TimeDuwn = 1f;
	public float TimeUp = 3f;	
	
	private cTimer TimerObj;
	private bool PlatformIsLowered = false;
	
	void OnEnable(){
	
		cMessenger<MonoBehaviour>.AddListener("PlatformUp", PlatformUp);
		cMessenger<MonoBehaviour>.AddListener("PlatformDown", PlatformDown);
	}
	
	void OnDisable(){
		
		cMessenger<MonoBehaviour>.RemoveListener("PlatformUp", PlatformUp);
		cMessenger<MonoBehaviour>.RemoveListener("PlatformDown", PlatformDown);
	}
	
	void Start(){
		 //TimerObj = GetComponent<cTimer>();
	}
	
	void PlatformDown(MonoBehaviour Obj){
		if(Obj == this){
			PlatformIsLowered = false; Stage = true;
			animation.CrossFade("Open");
			GetComponent<cTimer>().StartTimer("PlatformUp", TimeUp, this);
		}
	}
	
	void PlatformUp(MonoBehaviour Obj){
		if(Obj == this){
			PlatformIsLowered = false; Stage = false;
			animation.CrossFade("Close");
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.collider.enabled = !Stage; 
	}
	
	void OnCollisionEnter(){
		if(PlatformIsLowered){
			return;
		}
		
		GetComponent<cTimer>().StartTimer("PlatformDown", TimeDuwn,this);
		PlatformIsLowered = true;
	}
	
}
