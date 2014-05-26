using UnityEngine;
using System.Collections;

public class TriggerZoneDialoguePrincess : TriggerZoneDialogue {

	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update (){
		if(Input.GetButtonDown("Action")){
			
			if (isReading){
				isReading = GUIDialogue.instance.DialogueNextPage ();
				read = !isReading;
			}
			if (read){
				Application.LoadLevel(5);
			}
			player.GetComponent<TDS_Controller>().SetFrozen = isReading;
		}
	}

}
