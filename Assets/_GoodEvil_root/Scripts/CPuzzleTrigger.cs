using UnityEngine;
using System.Collections;

public class CPuzzleTrigger : MonoBehaviour {
	
	//public static int CountPuzzleLevel;
	
	private CIndicationControl pIndicationControl;


	void OnTriggerEnter(Collider other)
	{
		pIndicationControl = (CIndicationControl)other.GetComponent("CIndicationControl");
		pIndicationControl.anime = true;

		CGlobalStatic.CountCurrentPuzzle++;
		DestroyObject(this.gameObject);
		
	}
}
