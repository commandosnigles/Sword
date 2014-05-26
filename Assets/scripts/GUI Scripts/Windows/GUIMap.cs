using UnityEngine;
using System.Collections;

public class GUIMap : MonoBehaviour {
	public static GUIMap instance;

	//***Map screen variables***\\
	[System.Serializable]
	public class LevelButton{
		public Rect Rectangle;
		public int Level;
		public string LevelName;
		public Texture2D Active;
		public Texture2D Unlocked;
		public Texture2D Locked;
	}
	
	public LevelButton[] MapButtons;
	public int MaxMapWidth = 1920;
	public int MaxMapHeight = 1080;

	private enum navigation {
		increasing, decreasing, stationary
	}

	private navigation navDirection;
	private int selectedButton = 1;
	private float mapWinPercent;
	private int levelReached = 1;
	private Rect mapRect;
	
	void Awake () {
		instance = this;
		navDirection = navigation.stationary;
		SetSelection(1);
		mapRect = GUIMaster.instance.FitWindow (MaxMapWidth, MaxMapHeight, out mapWinPercent);
	}

	void Update() {
		bool newState = false;

		if(Input.GetAxis ("Horizontal") > 0.5 || Input.GetAxis ("Vertical") > 0.5) {
			if (navDirection != navigation.increasing)
				newState = true;
			navDirection = navigation.increasing;
		}
		else if(Input.GetAxis ("Horizontal") < -0.5 || Input.GetAxis ("Vertical") < -0.5) {
			if (navDirection != navigation.decreasing)
				newState = true;
			navDirection = navigation.decreasing;
		}
		else {
			navDirection = navigation.stationary;
		}

		if (newState) {
			if (navDirection == navigation.stationary)
				return;
			else if (navDirection == navigation.increasing && selectedButton < levelReached)
				selectedButton++;
			else if (navDirection == navigation.decreasing && selectedButton > 1)
				selectedButton--;
		}

		if (Input.GetButtonDown("Action"))
			LoadLevel();
	}

	void OnGUI () {
		if (Application.loadedLevel == 0){
			GUI.skin = GUIMaster.instance.GetSkin;
			if(!Screen.fullScreen)
				mapRect = GUIMaster.instance.FitWindow (MaxMapWidth, MaxMapHeight, out mapWinPercent);

			GUI.Window (1, mapRect,MapContents,"",GUI.skin.FindStyle("MapBackground"));
		}
	}
	
	void MapContents(int windowID){
		foreach (LevelButton button in MapButtons){
			Rect buttonRect = new Rect (button.Rectangle.x * mapWinPercent, button.Rectangle.y * mapWinPercent, 
			                            button.Rectangle.width * mapWinPercent, button.Rectangle.height * mapWinPercent);
			if (button.Level > levelReached){
				GUI.enabled = false; 
				GUI.skin.FindStyle("MapButton").normal.background = button.Locked;
			}
			else if (selectedButton == button.Level) {
				GUI.skin.FindStyle("MapButton").normal.background = button.Active;
			}
			else {
				GUI.skin.FindStyle("MapButton").normal.background = button.Unlocked;
			}

			if (GUI.Button (buttonRect, "", GUI.skin.FindStyle("MapButton"))){
				GUIMaster.instance.ActivateWindow();
				LoadLevel(button.Level);
			}
			
			GUI.enabled = true;
		}
		if (GUI.Button (new Rect((MaxMapWidth - 240) * mapWinPercent, (MaxMapHeight - 100) * mapWinPercent, 200 * mapWinPercent, 60 * mapWinPercent),"Back to Menu")) {
			GUIMaster.instance.ActivateWindow (GUIMainMenu.instance);		
		}
	}

	public void SetSelection (int selectionID) {
		selectedButton = selectionID;
	}

	public void SetSelection () {
		selectedButton = levelReached;
	}

	void LoadLevel (int levelID) {
		Application.LoadLevel (levelID);
		this.enabled = false;
	}

	void LoadLevel () {
		Application.LoadLevel (selectedButton);
		this.enabled = false;
	}

	public void NextLevelReached () {
		if (Application.levelCount > levelReached + 1 && Application.loadedLevel >= levelReached) levelReached = Application.loadedLevel + 1;
	}
	public void ResetLevelReached () {
		levelReached = 1;
	}
}
