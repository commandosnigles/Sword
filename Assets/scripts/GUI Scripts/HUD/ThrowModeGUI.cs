using UnityEngine;
using System.Collections;

public class ThrowModeGUI : MonoBehaviour {

	public Texture [] ModeIcons;

	private GUITexture activeIcon;
	private SwordControls controller;

	// Use this for initialization
	void Awake () {
		controller = FindObjectOfType <SwordControls> ();
		activeIcon = this.gameObject.GetComponent<GUITexture> ();
	}

	// Update is called once per frame
	void Update () {
		if(controller.getHasSword){
			activeIcon.enabled = true;
		}
		else activeIcon.enabled = false;
	}
	public void ChangeIcon (int throwMode) {
		activeIcon.texture = ModeIcons[throwMode];
	}
}
