//////////////////////////////////////////////////////////////////////
// Скрипт содержит класс инициализации диалога или подсказки
// Версия: 1.0.0.1
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class CTalksTrigger : MonoBehaviour {
	
	public int m_NumberTalks;
	
	private CTalksControls pTalksControls;

	// Use this for initialization
	void Awake () {
		
		collider.isTrigger = true;
		pTalksControls =  (CTalksControls)GameObject.Find("Player").GetComponent("CTalksControls");
	}
	
	void OnTriggerEnter(Collider other) 
	{
		pTalksControls.ShowTalks(m_NumberTalks);
		DestroyObject(this.collider);
	}	
}
