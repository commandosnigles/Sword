using UnityEngine;
using System.Collections;

public class CameraPanTrigger : MonoBehaviour {
	public float Speed = 10f;
	public float Delay = 0.5f;
	public direction Direction = direction.Right;
	public enum direction {Left, Right};

	private float elapsed;
	private TDS_Camera camera;
	private bool active;

	void Awake () {
		camera = GameObject.FindObjectOfType<TDS_Camera>();
		elapsed = 0f;
		active = true;
	}

	void OnTriggerExit (Collider col){
		if (col.tag == "Player"){
			active = true;
			elapsed = 0f;
		}
	}
	void OnTriggerStay (Collider col) {
		if (active){
			if (col.tag == "Player"){
				elapsed += Time.deltaTime;
				if (elapsed >= Delay){
					if (Direction == direction.Left)
						active = camera.PanCamera(-Speed);
					else
						active = camera.PanCamera(Speed);
				}
			}
		}
	}
}
