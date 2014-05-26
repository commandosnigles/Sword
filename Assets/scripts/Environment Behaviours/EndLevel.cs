using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

	private bool inTrigger = false;
	private GameObject player;
	
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update () {
		if (inTrigger && player.GetComponent <SwordControls>().getHasSword) {
			if (Input.GetButtonDown("Action")) {
				GUIMap.instance.NextLevelReached();
				GUIMaster.instance.ActivateWindow (GUIEndLevel.instance);
				player.GetComponent<TDS_Controller>().SetFrozen = true;
			}
		}
	}
	void OnGUI () {
		if (inTrigger){ 
			if (player.GetComponent <SwordControls>().getHasSword) {
				GUIMaster.instance.ControlTip ("End Level", "Action");
			}
			else{
				GUIMaster.instance.ControlTip ("Sword Required");
			}
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player")
			inTrigger = true;
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == "Player")
			inTrigger = false;
	}
}
