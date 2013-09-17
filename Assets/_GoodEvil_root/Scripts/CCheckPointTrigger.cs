using UnityEngine;
using System.Collections;

public class CCheckPointTrigger : MonoBehaviour {
	
	
	void OnTriggerEnter(Collider other) 
	{
		CGlobalStatic.SaveCurrentPosition = other.transform.position;
		DestroyObject(this.collider);
	}	
	
}
