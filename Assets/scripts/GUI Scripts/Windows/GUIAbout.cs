using UnityEngine;
using System.Collections;

public class GUIAbout : MonoBehaviour {

	public static GUIAbout instance;
	
	public int WindowWidth = 1920;
	public int WindowHeight = 1080;
	private Rect windowRect;
	private float windowPercent;

	void Awake () {
		instance = this;
		windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
	}
	
	void OnGUI () {
		if (Application.loadedLevel == 0){
			GUI.skin = GUIMaster.instance.GetSkin;
			if(!Screen.fullScreen)
				windowRect = GUIMaster.instance.FitWindow (WindowWidth, WindowHeight, out windowPercent);
			GUI.Box(windowRect,"",GUI.skin.FindStyle("About"));
		}
		if (GUI.Button (new Rect((WindowWidth - 240) * windowPercent, (WindowHeight - 100) * windowPercent, 200 * windowPercent, 60 * windowPercent),"Back to Menu")) {
			GUIMaster.instance.ActivateWindow (GUIMainMenu.instance);		
		}
	}
}
