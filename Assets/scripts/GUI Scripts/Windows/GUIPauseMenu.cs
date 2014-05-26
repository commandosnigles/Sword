using UnityEngine;
using System.Collections;

public class GUIPauseMenu : MonoBehaviour {

	public static GUIPauseMenu instance;

	public int WindowWidth = 228;
	public int WindowHeight = 500;
	private Rect windowRect;
	private float windowPercent;
	public bool GetPaused{
		get {return paused;}
	}
	private bool paused = false;
	
	void Awake () {
		instance = this;
		windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
	}

	void OnGUI () { 
		GUI.skin = GUIMaster.instance.GetSkin;
		if(!Screen.fullScreen)
			windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
		GUI.Window(2, windowRect, PauseWindow, "Pause");
	}
	
	void PauseWindow (int windowID) {
		if(GUI.Button (new Rect (50 * windowPercent, 50 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Continue")){
			Unpause ();
		}
		if(GUI.Button (new Rect (50 * windowPercent, 150 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Back To Map")){
			Unpause ();
			Application.LoadLevel (0);
			//GUIMap.instance.SetSelection();
			GUIMaster.instance.ActivateWindow(GUIMap.instance);
		}
		if(GUI.Button (new Rect (50 * windowPercent, 250 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Main Menu")){
			Unpause ();
			Application.LoadLevel (0);
			GUIMaster.instance.ActivateWindow(GUIMainMenu.instance);
		}
		if(GUI.Button (new Rect (50 * windowPercent, 350 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Exit Game")){
			Unpause ();
			Application.Quit();
		}
	}

	public void Pause() {
		paused = true;
		GUIMaster.instance.ActivateWindow(GUIPauseMenu.instance);
		Time.timeScale = 0;
	}

	public void Unpause() {
		paused = false;
		GUIMaster.instance.ActivateWindow();
		Time.timeScale = 1;
	}
}
