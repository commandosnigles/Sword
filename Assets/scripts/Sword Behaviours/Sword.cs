using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	public Vector3 Position;
	public Vector3 Rotation;
	public int SwordVersion;
	public bool GetIsFlying{
		get {return isFlying;}
	}
	public bool SetIsFlying{
		set{isFlying = value;}
	}
	[HideInInspector]
	public bool Held;
	private bool isFlying = true;
	private SwordControls controller;
	private Rigidbody rigidBody;
	private Animation flyingAnimation;
	private Collider[] colliderList;

	void Awake(){
		if (GetComponent<Animation>() == null){
			gameObject.AddComponent("Animation");
		}
		flyingAnimation = GetComponent<Animation>();
		flyingAnimation.playAutomatically = false;
		if (GetComponent<Rigidbody> () == null)
			gameObject.AddComponent ("Rigidbody");
		rigidBody = GetComponent<Rigidbody> ();
		colliderList = GetComponents<Collider>();
		
		controller = GameObject.FindObjectOfType <SwordControls> ();
	}

	void Start () {

	}

	void Update () {
		//if the sword falls from the level, automatically respawn at the active pedestal
		if (transform.position.y < -10f){
			foreach (SwordSpawnPedestal pedestal in SwordSpawnPedestal.Pedestals){
				pedestal.RespawnSword();
			}
		}
	}

	void OnCollisionEnter(Collision col){
		if (!Held)
			IgnorePlayer(false);
		if (col.gameObject.tag != "Enemy"){
			isFlying = false;
			animation.Stop();
		}
		else if (!controller.IsPiercing){
			isFlying = false;
			animation.Stop();
		}
	} 
	void OnTriggerExit(Collider col){
		if (col.tag == "Player") {
			Held = false;
			Debug.Log ("Sword Exited Character Trigger");
		}
	}

	public void Pickup (Transform throwingHand){
		Held = true;
		IgnorePlayer(true);
		gameObject.GetComponent<DamageOnCollision> ().DamagesEnemy = false;
		animation.Stop ();
		rigidBody.isKinematic = true;
		transform.parent = throwingHand;
		transform.localPosition = Position;
		transform.localRotation = Quaternion.Euler(Rotation);
		if (this.gameObject.GetComponent<Boomerang>()!= null){
			this.gameObject.GetComponent<Boomerang>().Pickup();
		}

	}
	public void IgnorePlayer (bool ignore) {
		foreach (Collider col in colliderList) {
			Physics.IgnoreCollision(col, controller.gameObject.collider, ignore);		
		}
		
	}

}
