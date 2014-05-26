using UnityEngine;
using System.Collections;

public class MainMenuCam : MonoBehaviour {

	public void FadeIn(){
		StartCoroutine (Fade_InOut.instance.FadeIn(1));
	}
	public void FadeOut(){
		StartCoroutine (Fade_InOut.instance.FadeOut(1));
	}
}
