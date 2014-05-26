using UnityEngine;
using System.Collections;

public class LifePickup : MonoBehaviour {

	public ParticleSystem Particles;
	public AudioSource Audio;
	
	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player"){
			LifeCounter.instance.IncrementLife ();
			Instantiate (Particles, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (Audio, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Destroy(this.gameObject);
		}
	}
}
