using UnityEngine;
using System.Collections;

public class Fade_InOut : MonoBehaviour {

	public static Fade_InOut instance;

	public int drawDepth = -1000;
	public Texture2D FadeTo;

	private float fps = 60;
	private float alpha = 0;
	private bool fading = false;

	void Start () {
		instance = this;
		Application.targetFrameRate = (int)fps;
	}

	void OnGUI () {
		if (fading){
			GUI.color = new Color(0, 0, 0, alpha);
			GUI.depth = drawDepth;
			GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height), FadeTo);
		}
	}

	public IEnumerator FadeOut (float fadeOutTime){
		fading = true;
		alpha = 0;
		for (float elapsed = 0; elapsed < fadeOutTime; elapsed += Time.fixedDeltaTime) {
			alpha = elapsed/fadeOutTime;
			yield return new WaitForFixedUpdate();
		}
	}

	public IEnumerator FadeIn (float fadeInTime){
		fading = true;
		alpha = 1;
		for (float elapsed = 0; elapsed < fadeInTime; elapsed += Time.fixedDeltaTime) {
			alpha = 1-elapsed/fadeInTime;
			yield return new WaitForFixedUpdate();
		}
		fading = false;
	}


}
