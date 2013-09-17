// скрипт управления движением по эсколатору
// направление движения задается в Parallax
// по клавише I меняется направление на противоположное

using UnityEngine;
using System.Collections;

public class cConveyorController : MonoBehaviour {
	
	
	// Длина смещения
	public float Strength = 0.05f;
	
	public bool Invert = false;
	
	[SerializeField]
	private Vector2 Parallax;
	
	[SerializeField]
	private bool PrecalculateParralax = true;
	
	void Awake(){
		if (PrecalculateParralax){
			Parallax = CalculateParralaxVector();
		}
	}
	
	void Start(){
		if (PrecalculateParralax){
			Parallax = CalculateParralaxVector();
		}		
	}
	
	void OnEnable(){
	
		cMessenger<string>.AddListener("MessageInvertParallax", MessageInvertParallax);
	}
	
	void OnDisable(){
		
		cMessenger<string>.RemoveListener("MessageInvertParallax", MessageInvertParallax);
	}
	
	// Рассчитывает вектор смещения.
	Vector2 CalculateParralaxVector()
	{		
		return (Vector2) (transform.rotation * Vector3.right * Strength);		
		
	}	
	
	void OnCollisionEnter(Collision CollisionObj){
		
		RunOffset(CollisionObj);
		
	}
	
	void OnCollisionStay(Collision CollisionObj){
		
		RunOffset(CollisionObj);
			
	}
	
	void InvertParallax(){
		Invert =  !Invert;
	}
	
	void MessageInvertParallax(string ObjectName){
		if(this.name == ObjectName){
			InvertParallax();
		}
	}
	
	// выполняется смещение
	void RunOffset(Collision CollisionObj){
		if (!PrecalculateParralax){
			Parallax = CalculateParralaxVector();
		}
		if (Invert){
			CollisionObj.transform.position = CollisionObj.transform.position + (Vector3)Parallax;
		}else{
			CollisionObj.transform.position = CollisionObj.transform.position - (Vector3)Parallax;
		}
	}
}
