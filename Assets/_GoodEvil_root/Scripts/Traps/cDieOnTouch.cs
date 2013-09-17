////////////////////////////////////////////////////////////////////
// Вызов события Die при коллизии с объектом.

using UnityEngine;
using System.Collections;

public class cDieOnTouch : MonoBehaviour {
	
 public Collider PlayerCollider;
	
 void OnTriggerEnter(Collider other){
		if (other == PlayerCollider){
			cMessenger.Invoke("Die");
		}
	}
	
 void OnCollisionEnter(Collision collision){
		
		if (collision.collider == PlayerCollider){
			cMessenger.Invoke("Die");
		}
		
	}
		
}
