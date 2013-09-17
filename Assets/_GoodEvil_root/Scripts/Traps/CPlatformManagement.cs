/// <summary>
/// C platform management. класс описывает движение платформы по горизонтальной или вертикальной оси
/// </summary>

using UnityEngine;
using System.Collections;

public class CPlatformManagement : MonoBehaviour {
	// enum
	public enum PlatformOrientation{Horizontal, Vertical};
	public enum DirectionOfTravel{StartFinish,FinishStart};
	
	// public
	public bool                PositiveDirection = true;
	public float               Start;
	public float               Finish;
	public float               Parallax;
	public PlatformOrientation Orientation = PlatformOrientation.Vertical;
	public bool                Loop        = false;
	public DirectionOfTravel   Direction   = DirectionOfTravel.StartFinish;
	
	// private
	private bool               Eneble      = false;
	
	
	void OnEnable(){
	
		cMessenger<string>.AddListener("MessageActivatePlatform", MessageActivatePlatform);
	}
	
	void OnDisable(){
		
		cMessenger<string>.RemoveListener("MessageActivatePlatform", MessageActivatePlatform);
	}
		
	void Update(){
		if(!Eneble){
			return;
		}
		
		// проверим надоли дальше двигать
			if(Direction == DirectionOfTravel.StartFinish){
				if(Orientation == PlatformOrientation.Vertical){
					if(transform.position.y == Finish){
						Direction = DirectionOfTravel.FinishStart;
						Eneble = Loop;
						return;
					}
				}
				else{
					if(transform.position.x == Finish){
						Direction = DirectionOfTravel.FinishStart;
						Eneble = Loop;
						return;
					}					
				}
			}
			else{
				if(Orientation == PlatformOrientation.Vertical){
					if(transform.position.y == Start){
						Direction = DirectionOfTravel.StartFinish;
						Eneble = Loop;
						return;
					}
				}
				else{
					if(transform.position.x == Start){
						Direction = DirectionOfTravel.StartFinish;
						Eneble = Loop;
						return;
					}					
				}				
			}
		
		// двигаем объект
		if(Direction ==DirectionOfTravel.StartFinish){
			if(PositiveDirection){
				if(Orientation == PlatformOrientation.Vertical){
					if((Finish - transform.position.y) < Parallax){
						transform.position = transform.position + CalculateParralaxVector(Finish - transform.position.y);
					}
					else{
						transform.position = transform.position + CalculateParralaxVector(Parallax);
					}
				}
				else{
					if((Finish - transform.position.x) < Parallax){
						transform.position = transform.position + CalculateParralaxVector(Finish - transform.position.x);
					}
					else{
						transform.position = transform.position + CalculateParralaxVector(Parallax);
					}
				}
			}
			else{
				if(Orientation == PlatformOrientation.Vertical){
					if((transform.position.y - Finish) < Parallax){
						transform.position = transform.position - CalculateParralaxVector(transform.position.y - Finish);
					}
					else{
						transform.position = transform.position - CalculateParralaxVector(Parallax);
					}
				}
				else{
					if((transform.position.x - Finish) < Parallax){
						transform.position = transform.position - CalculateParralaxVector(transform.position.x - Finish);
					}
					else{
						transform.position = transform.position - CalculateParralaxVector(Parallax);
					}
				}
				
			}
		}
		else{
			if(!PositiveDirection){
				if(Orientation == PlatformOrientation.Vertical){
					if((Start - transform.position.y) < Parallax){
						transform.position = transform.position + CalculateParralaxVector(Start - transform.position.y);
					}
					else{
						transform.position = transform.position + CalculateParralaxVector(Parallax);
					}
				}
				else{
					if((Start - transform.position.x) < Parallax){
						transform.position = transform.position + CalculateParralaxVector(Start - transform.position.x);
					}
					else{
						transform.position = transform.position + CalculateParralaxVector(Parallax);
					}
				}
			}
			else{
				if(Orientation == PlatformOrientation.Vertical){
					if((transform.position.y - Start) < Parallax){
						transform.position = transform.position - CalculateParralaxVector(transform.position.y - Start);
					}
					else{
						transform.position = transform.position - CalculateParralaxVector(Parallax);
					}
				}
				else{
					if((transform.position.x - Start) < Parallax){
						transform.position =  transform.position - CalculateParralaxVector(transform.position.x - Start);
					}
					else{
						transform.position = transform.position - CalculateParralaxVector(Parallax);;
					}
				}	
			}
			
		}
	}
	
	void MessageActivatePlatform(string liftname){
		if(this.name == liftname){
			Eneble = !Eneble;
		}
	}
	
	Vector3 CalculateParralaxVector(float Value){
		if(Orientation == PlatformOrientation.Vertical){
			return new Vector3(0,Value,0);
		}
		else{
			return new Vector3(Value,0,0);
		}
	}
	
}
