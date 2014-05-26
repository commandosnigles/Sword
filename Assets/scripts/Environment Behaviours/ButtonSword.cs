using UnityEngine;
using System.Collections;

public class ButtonSword : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if (col.tag == "Sword"){
			renderer.material.SetColor("_Color", Color.yellow);
			BroadcastMessage("ButtonActivated");
		}
	}
}
