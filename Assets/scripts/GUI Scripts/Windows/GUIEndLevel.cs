using UnityEngine;
using System.Collections;

public class GUIEndLevel : MonoBehaviour {

	public static GUIEndLevel instance;
	
	public int WindowWidth = 228;
	public int WindowHeight = 500;
	private Rect windowRect;
	private float windowPercent;
	
	void Awake () {
		instance = this;
		windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
	}
	
	void OnGUI () {
		if (!Screen.fullScreen)
			windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
		GUI.Window(3,windowRect, VictoryWindow, "Victory!");
	}
	
	void VictoryWindow (int windowID) {
		if (Application.levelCount <= Application.loadedLevel + 1) GUI.enabled = false;
		if(GUI.Button (new Rect (50 * windowPercent, 50 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Next Level")){
			if(Application.levelCount > Application.loadedLevel + 1)
				Application.LoadLevel(Application.loadedLevel + 1);
			else Debug.Log ("No More Levels");
			this.enabled = false;
		}
		GUI.enabled = true;
		if(GUI.Button (new Rect (50 * windowPercent, 150 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Back to Map")){
			Application.LoadLevel(0);
			GUIMap.instance.enabled = true;
			GUIMap.instance.SetSelection();
			this.enabled = false;
		}
		
		if(GUI.Button (new Rect (50 * windowPercent, 250 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Main Menu")){
			Application.LoadLevel(0);
			GUIMainMenu.instance.enabled = true;
			this.enabled = false;
			
		}
		if(GUI.Button (new Rect (50 * windowPercent, 350 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Exit Game")){
			Application.Quit();
		}
	}	
}
