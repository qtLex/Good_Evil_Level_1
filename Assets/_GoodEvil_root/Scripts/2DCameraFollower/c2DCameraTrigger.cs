////////////////////////////////////////////////
/// Класс триггера камеры. Работает в паре с 2dCameraFollower
/// v.1.0.0.1
/// 
using UnityEngine;
using System.Collections;

public class c2DCameraTrigger : MonoBehaviour {
	
	public enum LockType{
		Left, Right, Top, Bottom, None
	};
	
	public bool ZoomCamera  = false;
	public bool ShakeCamera = false;
	public bool LockCamera  = false;
	
	public LockType LockAxis = LockType.None;
	
	// zoom trigger
	public float NewZoom;
	
	// Shake trigger
	public float Amplitude;
	public float Interval;
	public float Duration;
	
	public Camera CameraFollowerParent; // main camera
	
	protected c2DCameraFollower CameraFollower;
	
	// Use this for initialization
	void Awake () {
		
		if(!collider){
			gameObject.AddComponent<BoxCollider>();
		};
		
		collider.isTrigger = true;
		CameraFollower = CameraFollowerParent.GetComponent<c2DCameraFollower>();
	
	}
	
	// On collision
	void OnTriggerEnter(Collider other) 
	{
		if (CameraFollower != null)
		{
			
			if (CameraFollower.Target != null &&
				other  == CameraFollower.Target.collider)
			{
				
					// Zoom option
					if (ZoomCamera){
							CameraFollower.CameraDistance = NewZoom;
					};
					
					// Shake option
					if (ShakeCamera){
							CameraFollower.ShakeAmplitude = Amplitude;
							CameraFollower.ShakeInterval  = Interval;
							CameraFollower.ShakeTime      = Duration;
					};
					
					// Lock option
					if (LockCamera){
							CameraFollower.LockAxis = LockAxis;
						};						
				
			} // target condition
			
		} // camera follower
		
	}
	void OnTriggerExit(Collider other) 
	{
		if (CameraFollower != null)
		{
			
			if (CameraFollower.Target != null &&
				other  == CameraFollower.Target.collider)
			{					
					// Lock option
					if (LockCamera){
							CameraFollower.LockAxis = LockType.None;
						};						
				
			} // target condition
			
		} // camera follower
	}
}
