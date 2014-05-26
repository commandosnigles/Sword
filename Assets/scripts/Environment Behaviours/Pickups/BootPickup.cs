using UnityEngine;
using System.Collections;

public class BootPickup : MonoBehaviour {

	public ParticleSystem Particles;
	public AudioSource Audio;
	public GUIDialogue.Dialogue [] Hint;

	private GameObject player;
	private bool isReading = false;

	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update() {
		if (isReading){
			if (Input.GetButtonDown("Action")){
				isReading = GUIDialogue.instance.DialogueNextPage();
				player.GetComponent<TDS_Controller>().SetFrozen = isReading;
				if (!isReading)
					Destroy(this.gameObject);
			}

		}
	}


	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player"){
			col.gameObject.GetComponent<TDS_Motor>().MoreJumps();
			GUIDialogue.instance.DialogueStart(gameObject.name,Hint);
			isReading = true;
			player.GetComponent<TDS_Controller>().SetFrozen = true;
			//Instantiate (Particles, this.gameObject.transform.position, this.gameObject.transform.rotation);
			//Instantiate (Audio, this.gameObject.transform.position, this.gameObject.transform.rotation);

		}

	}
}
