using UnityEngine;
using System.Collections;

public class ParticleEvent : MonoBehaviour {

	private ParticleSystem pSystem;

	void Start () {
		pSystem = gameObject.GetComponent<ParticleSystem>();
		pSystem.Play();
		StartCoroutine(EmitDestroy());
	}

	IEnumerator EmitDestroy(){
		yield return new WaitForSeconds (pSystem.startLifetime + pSystem.duration);
		Destroy(this.gameObject);
	}
}
