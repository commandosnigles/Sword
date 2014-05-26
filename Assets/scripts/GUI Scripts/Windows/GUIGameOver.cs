using UnityEngine;
using System.Collections;

public class GUIGameOver : MonoBehaviour {

	public static GUIGameOver instance;

	public int WindowWidth = 200;
	public int WindowHeight = 100;
	private Rect windowRect;
	private float windowPercent;

	void Awake () {
		instance = this;
		windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
	}

	void OnGUI () {
		if(!Screen.fullScreen)
			windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
		GUI.Window(6,windowRect,GameOverWindow, "Game Over");
	}
	
	void GameOverWindow (int windowID) {
		if(GUI.Button (new Rect (35 * windowPercent, 18 * windowPercent, 130 * windowPercent, 64 * windowPercent), "Main Menu")){
			GUIPauseMenu.instance.Unpause ();
			Application.LoadLevel (0);
			StartCoroutine (Fade_InOut.instance.FadeIn(.5f));
			GUIMaster.instance.ActivateWindow(GUIMainMenu.instance);
		}
	}

}
