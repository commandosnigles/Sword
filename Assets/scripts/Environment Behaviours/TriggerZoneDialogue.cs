using UnityEngine;
using System.Collections;

public class TriggerZoneDialogue : MonoBehaviour {

	public GUIDialogue.Dialogue [] Dialogue;
	public bool FreezeOnEnter;
	
	protected bool read = false;
	protected bool isReading = false;
	protected GameObject player;
	
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update (){
		if(Input.GetButtonDown("Action")){

			if (isReading){
				isReading = GUIDialogue.instance.DialogueNextPage ();
				read = !isReading;
			}

			player.GetComponent<TDS_Controller>().SetFrozen = isReading;
		}
	}
	
	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player" && !read) {
			GUIDialogue.instance.DialogueStart (gameObject.name, Dialogue);
			isReading = true;
			player.GetComponent<TDS_Controller>().SetFrozen = FreezeOnEnter;
		}
	}
	
	void OnTriggerExit (Collider col) {
		if (col.tag == "Player"){
			GUIDialogue.instance.DialogueEnd ();
			isReading = false;
		}
	}
}