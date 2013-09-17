////////////////////////////////////////////
/// Класс описывает подсказку в виде 3д текста над интерактивными объектами.
/// v 1.0.0.1
/// 

using UnityEngine;
using System.Collections;

public class cToolTip : MonoBehaviour {

	public string ToolTip = "";
	
	public Vector3 Offset = Vector3.zero;
	
	[SerializeField]
	public static TextMesh ToolTipText;		
	[SerializeField]
	public static cInteractor Interactor;
	
	private float MaxSize      = 0.14f;
	private Color TipColorBlend   = Color.gray;
	
	protected Color TipColor = Color.yellow;
	
	protected bool ShowMe = false;
	protected bool HideMe = false;
	
	protected static cToolTip RollbackTip = null;
	
	protected static float BlendSpeed  = 1.5f;
	protected static float ResizeSpeed = 10.0f;
	
	
	void Awake(){
		
		ToolTipText = GameObject.Find("Tooltip").GetComponent<TextMesh>();
		Interactor  = GameObject.Find("Interactor").GetComponent<cInteractor>();
		if (ToolTipText){
			MaxSize       = ToolTipText.characterSize;			
			TipColor      = ToolTipText.renderer.material.color;
			TipColorBlend = new Color(TipColorBlend.r, TipColorBlend.g, TipColorBlend.b, 1.0f);
		}
		
	}
	
	void FixedUpdate(){
		
	}
	
	void Update(){
			
		if (Interactor == null){
			return;
		}
		
		GameObject CurrentTarget = Interactor.CurrentTarget;
		
		if (CurrentTarget != null
			&& ToolTipText != null
			&& RollbackTip == null){
			    // if this is the current interactor target
				if (CurrentTarget == this.gameObject){
			
					ToolTipText.renderer.enabled = true;
					ToolTipText.text = ToolTip;
					ToolTipText.transform.position = CurrentTarget.transform.position + Offset;
					
					if (!ShowMe){
						ShowMe = true;
						ToolTipText.characterSize = 0.0f;
						ToolTipText.renderer.material.color = TipColorBlend;
					}					
					HideMe = false;
				}
			}
			
		else{   
			
				if (RollbackTip == null){
					RollbackTip = this;
				}
			
				if (RollbackTip != this){
					return;
				}
			
				if (!HideMe){
					HideMe = true;
					
					// Hide from a current parameters.
				}
				ShowMe = false;
			}
		
	
			
	
		float dTime = Time.deltaTime;
		
		if (ShowMe && !HideMe && RollbackTip == null){
			ToolTipText.characterSize = Mathf.Lerp(ToolTipText.characterSize, MaxSize, dTime * ResizeSpeed);
			ToolTipText.renderer.material.color = Color.Lerp(ToolTipText.renderer.material.color, TipColor, dTime * BlendSpeed);
		}
		
		if (!ShowMe && HideMe && RollbackTip == this){
			ToolTipText.characterSize = Mathf.Lerp(ToolTipText.characterSize, 0.01f, dTime * ResizeSpeed * 2);
			ToolTipText.renderer.material.color = Color.Lerp(ToolTipText.renderer.material.color, TipColorBlend, dTime * BlendSpeed);
						
			if (Mathf.Abs(ToolTipText.characterSize - 0.01f) < 0.001f) {
				ToolTipText.renderer.enabled = false;
				RollbackTip = null;
			}				
			
		}		
		
	}
		
}


