using UnityEngine;
using System.Collections;

public class TDS_Camera : MonoBehaviour {

	public Vector3 Deadzone = new Vector3(8f,6.7f,16f);
	public float ySpeed = 4.25f;

	private GameObject player;
	private Vector3 cameraDistance;
	private Vector3 camPos;
	private Vector3 charPos;


	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
		cameraDistance = transform.position - player.transform.position;
	}

	void LateUpdate () {
		if(player.GetComponent<HealthCounter>().GetCurrentHealth > 0){
			//record world position of camera and the center point of the camera deadzone
			camPos = transform.position;
			charPos = player.transform.position + cameraDistance;

			//Move camera when exiting deadzone\\
			//set camera speed to absolute value of character velocity
			Vector3 currentVelocity = player.GetComponent <TDS_Motor>().GetCurrentVelocity;
			Vector3 camSpeed = new Vector3 (Mathf.Abs (currentVelocity.x), 0, Mathf.Abs (currentVelocity.z));
			Vector3 camDisplace = Vector3.zero;

			if( camPos.x > charPos.x + Deadzone.x || camPos.x < charPos.x - Deadzone.x) {
				//set camera displacement to the distance it will travel toward the character in x this update
				camDisplace.x = Mathf.MoveTowards(camPos.x, charPos.x, camSpeed.x * Time.deltaTime) - camPos.x;
			}

			float targetY = cameraDistance.y + Mathf.Floor( player.transform.position.y / Deadzone.y) * Deadzone.y + 1.08f;
			
			camDisplace.y = Mathf.MoveTowards(camPos.y, targetY, Time.deltaTime * ySpeed) - camPos.y;

			if(camDisplace != Vector3.zero){
				Camera.main.transform.Translate(camDisplace,Space.World);
			}
		}
	}

	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position - cameraDistance, 2*Deadzone );
	}

	public bool PanCamera(float speed){
		if (speed > 0) {
			if (camPos.x < charPos.x + Deadzone.x) { 
				transform.Translate (new Vector3(speed,0,0) * Time.deltaTime,Space.World);
				return true;
			}	
			else
				return false;
		}
		else {
			if (camPos.x > charPos.x - Deadzone.x) { 
				transform.Translate (new Vector3(speed,0,0) * Time.deltaTime,Space.World);
				return true;
			}
			else
				return false;
		}
	}

	public void Respawn(){
		charPos = player.transform.position + cameraDistance;
		float targetY = cameraDistance.y + Mathf.Floor( player.transform.position.y / Deadzone.y) * Deadzone.y + 1.08f;
		Vector3 respawnDisplace = Vector3.Scale (charPos - transform.position, Vector3.right);
		respawnDisplace.y = targetY - transform.position.y;
		Camera.main.transform.Translate (respawnDisplace,Space.World);

	}

	public static void CameraCheckSetup(){
		if(Camera.main == null){
			GameObject temp = new GameObject("Main Camera");
			temp.AddComponent ("Camera");
			temp.tag = "MainCamera";
		}
		if (Camera.main.GetComponent <TDS_Camera> () == null)
			Camera.main.gameObject.AddComponent("TDS_Camera");
	}
}
