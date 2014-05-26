using UnityEngine;
using System.Collections;

public class TargetAndBridge : MonoBehaviour {

	public GameObject actingBridge;
	private bool active;
	// Use this for initialization
	void Start () {
		active = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider obj){
		if (active) {
			if(obj.collider.gameObject.tag == "Sword"){
				active = false;
				actingBridge.animation.Play();
			}
		}
	}
}
