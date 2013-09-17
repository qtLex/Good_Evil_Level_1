//////////////////////////////////////////////////////////////////
/// Клас камеры следующей за персонажем с 2мя стеменями свободы.
/// v. 1.0.0.1
/// 

using UnityEngine;
using System.Collections;

public class c2DCameraFollower : MonoBehaviour {
		
	public bool changeAngles = true;
	
	public Vector2 Offset;
	
	public GameObject Target; // target object
	
	public float CameraDistance = 20; // initial camera distance
	public float SmoothTime = 0.3F;   // follow time
	
	// cache
	private Transform CameraTransform;
	private Transform TargetTransform;
	
	// zero vector
	protected Vector3 velocity = Vector3.zero;
	
	// Shake trigger
	public float ShakeAmplitude = 0.3F;
	public float ShakeAngularAmplitude = 0.3F;
	
	public float ShakeInterval = 0.0F;
	public float ShakeTime     = 0.0F;
	
	public c2DCameraTrigger.LockType LockAxis = c2DCameraTrigger.LockType.None;
	
	protected float ShakeDuration = 0.0F;
	
	void Awake () {
		
			if (CameraDistance == 0) { CameraDistance = 20 ; };
			
		
			if (Target)
			{
				CameraTransform = transform;
			
				TargetTransform  = Target.transform;
			
				// Init position
				CameraTransform.position  = new Vector3(TargetTransform.position.x + Offset.x,
														TargetTransform.position.y + Offset.y,
														TargetTransform.position.z - CameraDistance);
			
				Random.seed = 12345;
			};
		
		}
		
	
	// Update is called once per frame
	void Update () {
		
		TargetTransform  = Target.transform;
		
		float dTime = Time.deltaTime;
		
		
		// Generating Shake
		if (ShakeTime > 0){
			
			
			if (ShakeInterval <= ShakeDuration){
				
				GenerateShakeState(ShakeAmplitude);
				ShakeTime -= ShakeDuration;
				ShakeDuration = 0.0F;
				
			}
			else{
				
				ShakeDuration += dTime;
				
			}; // duration check
			
		}
		else{
			
			ShakeTime = 0.0F;
			ShakeDuration = 0.0F;
			
		};
		
		
		// following target object, wigh fixing angle if changed.
		Vector3 NewPosition;
		
		switch (LockAxis){
			case c2DCameraTrigger.LockType.None:
				NewPosition = TargetTransform.position;
				break;
			case c2DCameraTrigger.LockType.Left:
				NewPosition   = TargetTransform.position;
				NewPosition.x = Mathf.Max(TargetTransform.position.x,CameraTransform.position.x);
				break;
			case c2DCameraTrigger.LockType.Right:
				NewPosition   = TargetTransform.position;
				NewPosition.x = Mathf.Min(TargetTransform.position.x,CameraTransform.position.x);
				break;
			case c2DCameraTrigger.LockType.Top:
				NewPosition   = TargetTransform.position;
				NewPosition.x = Mathf.Min(TargetTransform.position.y,CameraTransform.position.y);
				break;
			case c2DCameraTrigger.LockType.Bottom:
				NewPosition   = TargetTransform.position;
				NewPosition.x = Mathf.Max(TargetTransform.position.y,CameraTransform.position.y);
				break;
			default:
				NewPosition = TargetTransform.position;
				break;
		};
		
		
		
		NewPosition.z = TargetTransform.position.z - CameraDistance;
		
		// -offset on camera position
		CameraTransform.Translate(-Offset.x, -Offset.y, 0, Space.Self);
		
		//NewPosition = Vector3.SmoothDamp(CameraTransform.position, NewPosition, ref velocity, SmoothTime);		
		NewPosition = Vector3.Lerp(CameraTransform.position, NewPosition, Time.deltaTime/SmoothTime);
			
		CameraTransform.position = NewPosition;
		
		// Offset translation
		CameraTransform.Translate(Offset.x, Offset.y, 0, Space.Self);
		CameraTransform.LookAt(TargetTransform);
		
		if (changeAngles){
			// Lerping rotation on forward.
			Quaternion NewRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);			
			CameraTransform.rotation = Quaternion.Slerp(CameraTransform.rotation, NewRotation, dTime * 0.3F);
		}
	
	}
	
	private void GenerateShakeState(float Disp) {
		
		CameraTransform.Translate(Random.insideUnitSphere * Disp);
		CameraTransform.rotation = Quaternion.Euler(Random.Range(-1.0F,1.0F) * ShakeAngularAmplitude, Random.Range(-1.0F,1.0F) * ShakeAngularAmplitude, Random.Range(-1.0F,1.0F) * ShakeAngularAmplitude);
		
	}
		
	
}
