//////////////////////////////////////////////////////////////////////
// Скрипт содержит классы управления диалогами и подсказками в игре
// Версия: 1.0.0.1
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

//////////////////////////////////////////////////////////////////////
// Класс CTalksControls предназначен для вывода подсказок и диалогов между 
// персанажами.
//
public class CTalksControls : MonoBehaviour {
	
	public GUISkin m_Skin;
	
	private bool ShowTakls = false;
	private CTalks CurrentTalks;
	private float Timer = 3.0f;
	
	private Dictionary<int,CTalks> TalksList = new Dictionary<int,CTalks>();
	
	private int BoxX  = 0;
	private int BoxY  = 0;
	
	private int BoxWidth  = 300;
	private int BoxHeight = 150;	
	private int BoxHelpHeight = 60;	
		

	// Use this for initialization
	void Awake () {
		
		GetTalksLevel(Application.loadedLevelName);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		// По таймеру получим следующую реплику, если их больше нет, прекратим диалог.
		if (ShowTakls)
			if (Timer <= 0)
				if (!CurrentTalks.GetMessage())
				{
					Timer = 3.0f;
					ShowTakls = false;
				}
				else
					Timer = 3.0f;
			else
				Timer -= Time.deltaTime;
	}
	
	void OnGUI()
	{
		if (ShowTakls)
		{
			GUI.skin = m_Skin;
			GUIStyle style;
			
			int PointLeft = 100;
			int PointTop = 50;
			
			if (CurrentTalks.m_IsHelp)
			{
				// Получим размер рамки для подсказки
				BoxX = Screen.width - (PointLeft*2);
				BoxY = BoxHelpHeight;
				
				style = GUI.skin.GetStyle("Box");
			}
			else
			{
				Vector3 pos_person = CurrentTalks.m_PersonMessag.transform.position;
				Vector3 pos_personwith = CurrentTalks.m_PersonMessagWith.transform.position;
				
				// Определим положение диалоговой рамки
				bool left = true;
				if (pos_person.x <= pos_personwith.x)
					left = true;
				else
					left = false;
				
				// Рассчитаем верхнюю точку персонажа
				Vector3 pos = CurrentTalks.m_PersonMessag.transform.position;
				
				pos.y -= CurrentTalks.m_PersonMessag.transform.localScale.y/2;
				pos.x = left ? pos.x - CurrentTalks.m_PersonMessag.transform.localScale.x/2 : pos.x + CurrentTalks.m_PersonMessag.transform.localScale.x;
				
				// Получим координаты относительно экрана
				Vector3 posW = Camera.main.WorldToScreenPoint(pos);
				
				int x = (int)posW.x;
				int y = (int)posW.y;
				
				// Рассчитаем верхнюю левую точку рамки диалога
				PointLeft = left ? x - BoxWidth : x;
				PointTop = y-BoxHeight;
				
				// Получим размер рамки для диалога
				BoxX = BoxWidth;
				BoxY = BoxHeight;
				
				style = left ? GUI.skin.GetStyle("FrameLeft") : GUI.skin.GetStyle("FrameRight");
			
			}
			
			// Выводим диалог
			
			Rect RectBox = new Rect(PointLeft,PointTop,BoxX,BoxY);
			Rect RectLable = new Rect(RectBox.x + 10, RectBox.y + 10, RectBox.width - 20, RectBox.height - 20);
		
			GUI.Box(RectBox,"", style);
			GUI.Label(RectLable,CurrentTalks.m_CurrentMessage);
				
		}
	}
	
	//////////////////////////////////////////////////////////////////////
	// Процедура получает диалог по номеру и инициализирует отображение 
	// диалога. Вызывается из класса CTalksTrigger.
	//
	// Параметры:
	//  NumberTalks - число, номер диалога  
	//
	public void ShowTalks(int NumberTalks)
	{
		CurrentTalks = TalksList[NumberTalks];
		
		if (CurrentTalks.GetMessage())
			ShowTakls = true;
	}
	
	//////////////////////////////////////////////////////////////////////
	// Процедура загружает структуру дилогов уровня из xml. 
	//
	// Параметры:
	//  NameXmlFile - строка, имя xml файла в каталоге Resources/GUI/  
	//
	void GetTalksLevel(string NameXmlFile)
	{
		// Получим схему диалогов для текущего уровня
		string filePath = "GUI/"+NameXmlFile;
		TextAsset xml = (TextAsset)Resources.Load(filePath);
		
		// Нет схемы диалогов
		if (!xml) return;
				
		XmlDocument xmlDoc = new XmlDocument(); 
		xmlDoc.LoadXml(xml.text);

		XmlNodeList xmltalkslist = xmlDoc.GetElementsByTagName("talk"); // массив секций talks
		
		foreach (XmlNode talksInfo in xmltalkslist)
		{
			CTalks talks = new CTalks();
			
			int id = Convert.ToInt32(talksInfo.Attributes["id"].Value);
			
			talks.m_IsHelp = Convert.ToBoolean(talksInfo.Attributes["ishelp"].Value);
			
			XmlNodeList talkcontent = talksInfo.ChildNodes;
			foreach (XmlNode talkItems in talkcontent) // levels itens nodes.
			{
				talks.AddMessage(talkItems.Attributes["person"].Value,talkItems.Attributes["personwith"].Value, talkItems.InnerText);
			}
			
			TalksList.Add(id,talks);
			
		}
		
	}
}


//////////////////////////////////////////////////////////////////////
// Класс CTalks предназначен для хранения реплик диалога между 
// персанажами. Используется в классе CTalksControls.
//
public class CTalks {


	public bool m_IsHelp = false;
	public string m_CurrentMessage = "";
	public GameObject m_PersonMessag, m_PersonMessagWith;
	
	
	private struct MessStruct
	{
		public string person;
		public string personwith;
		public string message;
	}	
	
	private int CountMessage = 0;
	
	private List<MessStruct> MessagesList = new List<MessStruct>();
	
	//////////////////////////////////////////////////////////////////////
	// Процедура добавления диалоговых реплик персонажей.
	//
	// Параметры:
	//  person     - строка, персонаж от которого ведется диалог  
	//  personwith - строка, персонаж c которым ведется диалог
	//  message    - строка, реплика
	//
	public void AddMessage(string person,string personwith, string message)
	{
		MessStruct mess;
		
		mess.person     = person;
		mess.personwith = personwith;
		mess.message    = message;
		
		MessagesList.Add(mess);
	}
	
	//////////////////////////////////////////////////////////////////////
	// Процедура получения диалоговых реплик персонажей.
	//
	// Параметры:
	//  Нет  
	//  
	// Выходные параметры:
	//  Истина, получен текущая реплика, лож - диалог окончен
	//
	public bool GetMessage()
	{
	
		if (CountMessage >= MessagesList.Count)
			return false;
		
		if (!m_IsHelp)
		{
			m_PersonMessag     = GameObject.Find(MessagesList[CountMessage].person);
			m_PersonMessagWith = GameObject.Find(MessagesList[CountMessage].personwith);
		}
		
		m_CurrentMessage = MessagesList[CountMessage].message;
		
		CountMessage++;
		
		return true;
	}
	
}
