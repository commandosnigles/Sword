using UnityEngine;
using System.Collections;

public class GUIMaster : MonoBehaviour {

	public static GUIMaster instance;
	
	public GUISkin[] SkinsList;
	public GUISkin GetSkin {
		get{return SkinsList[skinIndex];}
	}

	private int skinIndex = 0;
	private MonoBehaviour[] windows;

	private int width;
	private int height;
	private float ratio;

	void Awake () {
		DontDestroyOnLoad(this.gameObject);
		instance = this;
		GetScreenDimensions ();
	}

	void Start(){
		windows = new MonoBehaviour[] {GUIMainMenu.instance, GUIMap.instance, GUIPauseMenu.instance, GUIEndLevel.instance, GUIGameOver.instance, GUIDeath.instance, GUIAbout.instance};
		if (Application.loadedLevel == 0)
			ActivateWindow(GUIMainMenu.instance);
		else
			ActivateWindow ();
	}

	void OnGUI () {
		string[] controllers = Input.GetJoystickNames();
		bool xbox = false;
		for (int i = 0; i < controllers.Length; i++){
			xbox = Input.GetJoystickNames()[i].Contains("XBOX");
			if (xbox) break;
		}
		if (xbox) 
			skinIndex = 1;
		else 
			skinIndex = 0;

		if (!Screen.fullScreen)
			GetScreenDimensions();
	}

	public Rect FitWindow (int winWidth, int winHeight, out float winPercent) {
		if (ratio > (float)winHeight / (float)winWidth){
			winPercent = Mathf.Clamp01 ((float)width/(float)winWidth);
		}
		else{
			winPercent = Mathf.Clamp01 ((float)height / (float)winHeight);
		}
		Rect winRect = new Rect (width / 2 - winWidth * winPercent / 2, height / 2 - winHeight * winPercent / 2, 
		                    winWidth * winPercent, winHeight * winPercent);
		return winRect;
	}

	public void ActivateWindow (MonoBehaviour active) {
		foreach (MonoBehaviour window in windows){
			if (window == active)
				window.enabled = true;
			else
				window.enabled = false;
		}
	}
	public void ActivateWindow () {
		foreach (MonoBehaviour window in windows)
			window.enabled = false;
	}

	public void ControlTip (string command, string iconName) {
		GUI.skin = SkinsList[skinIndex];
		GUILayout.BeginArea (new Rect (Screen.width/2-32, Screen.height*.9f-32, 512, 128));
		GUILayout.BeginHorizontal();
		GUILayout.Box ("", GUI.skin.FindStyle(iconName));
		GUILayout.Label (command);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
	public void ControlTip (string command) {
		GUI.skin = SkinsList[skinIndex];
		GUILayout.BeginArea (new Rect (Screen.width/2-32, Screen.height*.9f-32, 512, 128));
		GUILayout.Label (command);
		GUILayout.EndArea();
	}

	public static void GUIMasterCheckCreate(GUIMaster master){
		if (GameObject.FindObjectOfType<GUIMaster>() != null)
			return;
		GUIMaster temp = (GUIMaster)Instantiate(master);
		temp.name = "GUIMaster";

	}
	
	void GetScreenDimensions() {
		width = Screen.width;
		height = Screen.height;
		ratio = (float)height / (float)width;
	}
}
