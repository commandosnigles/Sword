using UnityEngine;
using System.Collections;

public class TDS_Respawn : MonoBehaviour {

	private Vector3 respawnLocation;
	private TDS_Camera mainCamera;

	void Awake () {
		respawnLocation = transform.position;
		TDS_Camera.CameraCheckSetup();
		mainCamera = Camera.main.GetComponent<TDS_Camera>();
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "RespawnPoint") respawnLocation = col.transform.position;
	}

	public void Respawn(){
		GUIPauseMenu.instance.Unpause();
		gameObject.transform.Translate(respawnLocation - gameObject.transform.position, Space.World);
		gameObject.GetComponent<TDS_Controller>().SetFrozen = false;
		gameObject.GetComponentInChildren<CharacterAnimations>().Ready();
		mainCamera.Respawn();
		StartCoroutine(Fade_InOut.instance.FadeIn(.5f));
	}


		


}
