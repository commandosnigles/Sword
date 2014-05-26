using UnityEngine;
using System.Collections;

public class GUIDeath : MonoBehaviour {

	public static GUIDeath instance;

	public int WindowWidth = 500;
	public int WindowHeight = 160;
	private Rect windowRect;
	private float windowPercent;

	void Awake () {
		instance = this;
		windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
	}

	void OnGUI () {
		if(!Screen.fullScreen)
			windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
		GUI.Window(5,windowRect,DeathWindow, "You Died!");
	}

	void DeathWindow (int windowID) {

		if(GUI.Button (new Rect (70 * windowPercent, 50 * windowPercent, 130 * windowPercent, 64 * windowPercent), "Continue")){
			GameObject.FindObjectOfType<TDS_Respawn>().Respawn();
		}
		if(GUI.Button (new Rect (300 * windowPercent, 50 * windowPercent, 130 * windowPercent, 64 * windowPercent), "Back To Map")){
			GUIPauseMenu.instance.Unpause ();
			Application.LoadLevel (0);
			StartCoroutine (Fade_InOut.instance.FadeIn(.5f));
			//GUIMap.instance.SetSelection();
			GUIMaster.instance.ActivateWindow(GUIMap.instance);
		}
	}
}
