using UnityEngine;
using System.Collections;

public class GUIDialogue : MonoBehaviour {
	public static GUIDialogue instance;

	[System.Serializable]
	public class Dialogue {
		public string Text;
		public bool NextLine = true;
		public bool GraphicElement = false;
	}

	private bool inDialogue = false;
	
	//window size variables
	public float DialogueAspectRatio = 0.25f;
	public float MaxDialogueWidth = 860;
	public int fontMaxSize = 24;
	private float dialogueWinPercent;
	
	//content variables
	public int LinesPerPage = 3;
	private int currentStartingIndex = 0;
	private int lastIndex = 0;
	private string speakerName;
	private Dialogue [] dialogue;

	void Awake () {
		instance = this;
	}

	void OnGUI () {
		DialogueWindow();
	}

	void DialogueWindow (){

		if (inDialogue){	
			
			float winWidth = Mathf.Min (MaxDialogueWidth, Screen.width * 3/4);
			float winHeight =  (winWidth * DialogueAspectRatio);
			dialogueWinPercent = winWidth / MaxDialogueWidth;
			GUISkin currentSkin = GUIMaster.instance.GetSkin;
			
			GUI.skin = currentSkin;
			GUI.skin.label.fontSize = (int)(fontMaxSize * dialogueWinPercent);
			GUI.skin.label.padding = new RectOffset((int)(6 * dialogueWinPercent), (int)(6 * dialogueWinPercent), 
			                                        (int)(16 * dialogueWinPercent), (int)(16 * dialogueWinPercent));
			GUI.Window(0, new Rect (Screen.width/2-winWidth/2, Screen.height-winHeight, winWidth, winHeight), DialogueContents, speakerName);
		}
	}

	public bool DialogueNextPage(){
		if (currentStartingIndex == lastIndex){
			DialogueEnd();
			return false;
		}
		else{
			currentStartingIndex = lastIndex;
			return true;
		}
	}
	
	public void DialogueStart(string name, Dialogue [] contents){
		dialogue = contents;
		speakerName = name;
		inDialogue = true;
	}
	
	public void DialogueEnd(){
		inDialogue = false;
		currentStartingIndex = 0;
		lastIndex = 0;
	}
	
	void DialogueContents (int windowID){
		int lines = 1;
		int iconSize = (int)(64 * dialogueWinPercent);
		GUILayout.BeginHorizontal();
		for (int i = currentStartingIndex; i < dialogue.Length; i++){
			if (!dialogue[i].GraphicElement){
				//print label in new line
				if (dialogue[i].NextLine){
					if(i != currentStartingIndex)
						lines++;
					if(lines > LinesPerPage){
						lastIndex = i;
						break;
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					GUILayout.Label (dialogue[i].Text);
				}
				//print label
				else{
					GUILayout.Label (dialogue[i].Text);
				}
			}
			else {
				//draw graphic on new line
				if (dialogue[i].NextLine) {
					if(i != currentStartingIndex)
						lines++;
					if(lines > LinesPerPage){
						lastIndex = i;
						break;
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					GUI.skin.FindStyle (dialogue[i].Text).fixedWidth = iconSize;
					GUI.skin.FindStyle (dialogue[i].Text).fixedHeight = iconSize;
					GUILayout.Box ("", GUI.skin.FindStyle (dialogue[i].Text));
				}
				//draw graphic in line
				else {
					GUI.skin.FindStyle (dialogue[i].Text).fixedWidth = iconSize;
					GUI.skin.FindStyle (dialogue[i].Text).fixedHeight = iconSize;
					GUILayout.Box ("", GUI.skin.FindStyle (dialogue[i].Text));
				}
			}
		}
		
		GUILayout.EndHorizontal();
		GUI.skin.FindStyle ("Action").fixedWidth = iconSize;
		GUI.skin.FindStyle ("Action").fixedHeight = iconSize;
		GUI.Box (new Rect(MaxDialogueWidth * dialogueWinPercent - iconSize , MaxDialogueWidth * dialogueWinPercent * DialogueAspectRatio - iconSize ,
		                  iconSize, iconSize),"", GUI.skin.FindStyle ("Action"));
	}
}
