using UnityEngine;
using System.Collections;

public class CIndicationControl : MonoBehaviour {
	
	public GUISkin m_Skin;
	public bool anime = false;
	
	private float TimeAnime = 0.3f;
	private int deltaRecBox = 0;
	private int deltaRecSpace = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (anime)
		{
			if (TimeAnime <= 0)
			{
				anime = false;
				deltaRecBox = 0;
				deltaRecSpace = 0;
				TimeAnime = 0.3f;
			}
			else
			{
				deltaRecBox = 6;
				deltaRecSpace = 3;
				TimeAnime -= Time.deltaTime;
			}
		}

	}
	
	void OnGUI()
	{
		
		GUI.skin = m_Skin;
		GUIStyle style;
		
		style = GUI.skin.GetStyle("IndicationPuzzle");
		
		Rect RectBox = new Rect((10+deltaRecSpace),(10+deltaRecSpace),(50 - deltaRecBox),(50 - deltaRecBox));
		Rect RectLable = new Rect(65,15,50,50);
		
		GUI.Box(RectBox,"", style);
		GUI.Label(RectLable,CGlobalStatic.CountCurrentPuzzle + "/3",GUI.skin.GetStyle("IndicationPuzzleLable"));
		
	}
}
