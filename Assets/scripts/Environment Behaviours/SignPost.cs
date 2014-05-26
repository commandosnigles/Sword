using UnityEngine;
using System.Collections;

public class SignPost : MonoBehaviour {

	public GUIDialogue.Dialogue [] Dialogue;

	private bool inTrigger = false;
	private bool isReading = false;
	private GameObject player;

	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update (){
		if(Input.GetButtonDown("Action")){
			if(inTrigger && !isReading && Vector3.Angle (player.transform.forward, this.gameObject.transform.position - player.transform.position) < 60) { 
				GUIDialogue.instance.DialogueStart(gameObject.name, Dialogue);
				isReading = true;
			}
			else if (isReading){
				isReading = GUIDialogue.instance.DialogueNextPage ();
			}
			if (inTrigger)
				player.GetComponent<TDS_Controller>().SetFrozen = isReading;
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player")
			inTrigger = true;
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == "Player"){
			inTrigger = false;
			GUIDialogue.instance.DialogueEnd ();
			isReading = false;
		}
	}

	void OnGUI () {
		if (!isReading && inTrigger && Vector3.Angle (player.transform.forward, this.gameObject.transform.position - player.transform.position) < 60) {
			GUIMaster.instance.ControlTip ("Read", "Action");
		}
	}
}
