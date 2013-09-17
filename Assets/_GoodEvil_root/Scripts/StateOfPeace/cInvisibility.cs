using UnityEngine;
using System.Collections;

public class cInvisibility : MonoBehaviour {
	public cGlobalData.StateOfTheWorld InvisibilityState  = cGlobalData.StateOfTheWorld.Good;
	public bool                        DisebledColliders  = true;
	public bool                        DisebledParticles  = false;
	public bool                        ItIsPlatform       = false;
	
	void OnEnable(){
		cMessenger<cGlobalData.StateOfTheWorld>.AddListener("Changed the state of the world", ChangedState);
	}
	
	void OnDisable(){
		cMessenger<cGlobalData.StateOfTheWorld>.RemoveListener("Changed the state of the world", ChangedState);
	}
	
	void ChangedState(cGlobalData.StateOfTheWorld State){
		bool Invisibility = (State == InvisibilityState);
		MeshRenderer[] ChildMeshObj = transform.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] ChildSkinnedMeshObj = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
		
		foreach (MeshRenderer ChildMesh in ChildMeshObj){
			ChildMesh.renderer.enabled = !Invisibility;
		}
		
		foreach (SkinnedMeshRenderer ChildMesh in ChildSkinnedMeshObj){
			ChildMesh.renderer.enabled = !Invisibility;
		}
		
		if(DisebledColliders){
			Collider[] Colliders = transform.GetComponents<Collider>();
			
			foreach (Collider ThisCollider in Colliders){
				ThisCollider.enabled = !Invisibility;
			}
			
			Collider[] ChildColliders = transform.GetComponentsInChildren<Collider>();
		
			foreach (Collider ChildCollider in ChildColliders){
				ChildCollider.enabled = !Invisibility;
			}
			
			if(ItIsPlatform){
				cPlatformController[] Scripts = transform.GetComponents<cPlatformController>();
				foreach (cPlatformController Script in Scripts){
					Script.enabled = !Invisibility;
				}
			}
		}
		
		if(DisebledParticles){
			ParticleSystem[] ChildParticles = transform.GetComponentsInChildren<ParticleSystem>();
			
			foreach (ParticleSystem ChildParticle in ChildParticles){
				if(Invisibility){
					ChildParticle.Pause();
					ChildParticle.Clear();
				}
				else{
					ChildParticle.Play();
				}
				
			}
		}
	}
}
