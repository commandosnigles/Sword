using UnityEngine;
using System.Collections;

public class SoundEvent : MonoBehaviour {

	private AudioSource Source;
	private AudioClip Clip;

	void Start () {
		Source = gameObject.GetComponent<AudioSource>();
		Clip = Source.clip;
		StartCoroutine(PlayDestroy());
	}

	IEnumerator PlayDestroy() {
		Source.audio.PlayOneShot(Clip);
		yield return new WaitForSeconds(Clip.length);
		Destroy (this.gameObject);
	}

}
