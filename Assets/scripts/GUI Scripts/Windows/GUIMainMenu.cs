using UnityEngine;
using System.Collections;

public class GUIMainMenu : MonoBehaviour {

	public static GUIMainMenu instance;

	public int WindowWidth = 228;
	public int WindowHeight = 500;
	private Rect windowRect;
	private float windowPercent;

	void Awake () {
		instance = this;
		windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
	}

	void OnGUI () {
		if (Application.loadedLevel == 0){
			if(!Screen.fullScreen)
				windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
			GUI.Window(2,windowRect, MainWindow,"");
		}
	}

	void MainWindow (int windowID) {
		if(GUI.Button (new Rect (50 * windowPercent, 50 * windowPercent, 128 * windowPercent, 64 * windowPercent), "New Game!")){
			LifeCounter.instance.ResetLives ();
			GUIMap.instance.ResetLevelReached ();
			GUIMap.instance.SetSelection();
			GUIMaster.instance.ActivateWindow(GUIMap.instance);
		}
		GUI.enabled = false;
		if(GUI.Button (new Rect (50 * windowPercent, 150 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Continue!")){
			GUIMaster.instance.ActivateWindow(GUIMap.instance);
		}
		GUI.enabled = true;
		if(GUI.Button (new Rect (50 * windowPercent, 250 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Credits")){
			GUIMaster.instance.ActivateWindow (GUIAbout.instance);
		}

		if(GUI.Button (new Rect (50 * windowPercent, 350 * windowPercent, 128 * windowPercent, 64 * windowPercent), "Exit")){
			Application.Quit();
		}
	}
}
