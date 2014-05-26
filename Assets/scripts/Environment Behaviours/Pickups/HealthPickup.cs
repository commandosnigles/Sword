using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour {
	public int Healing;
	public ParticleSystem Particles;
	public AudioSource Audio;

	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player"){
			col.gameObject.GetComponent<HealthCounter>().Heal(Healing);
			Instantiate (Particles, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (Audio, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Destroy(this.gameObject);
		}
	}
}
