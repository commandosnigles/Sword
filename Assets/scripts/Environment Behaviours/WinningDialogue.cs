using UnityEngine;
using System.Collections;

public class WinningDialogue : MonoBehaviour {

	public GUIDialogue.Dialogue [] Dialogue;
	

	private bool isReading = false;

	void Start () {
		GUIDialogue.instance.DialogueStart ("", Dialogue);
		isReading = true;
	}

	void Update (){
		if(Input.GetButtonDown("Action")){
			isReading = GUIDialogue.instance.DialogueNextPage ();
			if (!isReading){
				Application.LoadLevel (0);
				GUIMaster.instance.ActivateWindow(GUIMainMenu.instance);
			}


		}
	}

}