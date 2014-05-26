using UnityEngine;
using System.Collections;

public class SwordControls : MonoBehaviour {

	public bool IsPiercing = true;
	public int MaxMode = 3;
	public float ThrowSpeed = 10f;
	public float Arc = 2f;
	public Sword[] SwordPrefab;
	public Vector3 Offset;
	public GUITexture ModeIndicator;
	public Transform ThrowingHand;

	private CharacterAnimations animator;
	private bool hasSword;
	private int throwMode = 0;
	private CharacterController controller;
	private ThrowModeGUI modeGUI;
	private TDS_Controller tds_control;
	private Sword sword;

	//used to read the private variable hasSword
	//hasSword can only be manipulated in this class and is read-only everywhere else
	public bool getHasSword{
		get {return hasSword;}
	}
	public int getThrowMode{
		get {return throwMode;}
	}
	public int SetThrowMode{
		set {throwMode = value;}
	}
	
	void Start () {

		tds_control = gameObject.GetComponent<TDS_Controller>();
		animator = gameObject.GetComponentInChildren<CharacterAnimations>();
		controller = (CharacterController) GetComponent<CharacterController>();
		if (GameObject.FindObjectOfType <ThrowModeGUI> () == null)
			Instantiate(ModeIndicator);
		modeGUI = GameObject.FindObjectOfType <ThrowModeGUI> ();
		modeGUI.ChangeIcon(throwMode);
	}

	void Update () {
		if (sword == null)
			hasSword = false;
		if (hasSword && !tds_control.GetFrozen){

			//switch through throw modes
			if (Input.GetButtonDown ("ThrowMode_Next")) {
				throwMode++;
				throwMode = throwMode % MaxMode;
				DestroySwords();
				sword = (Sword) Instantiate(SwordPrefab[throwMode]);
				sword.Pickup(ThrowingHand);
				modeGUI.ChangeIcon(throwMode);
			}
			if (Input.GetButtonDown ("ThrowMode_Prev")) {
				throwMode--;
				if (throwMode < 0) throwMode = MaxMode-1;
				DestroySwords();
				sword = (Sword) Instantiate(SwordPrefab[throwMode]);
				sword.Pickup(ThrowingHand);
				modeGUI.ChangeIcon(throwMode);
			}

			//throw the sword
			if (Input.GetButtonDown ("Throw")){
//				hasSword = false;
//				DestroySwords ();
				//spawn the sword three units in front of the character, pointing the right way
//				Sword sword = (Sword)Instantiate (SwordPrefab[throwMode], transform.position + Offset + controller.transform.forward * 3f, 
//				                                          this.transform.rotation);

//				sword.rigidbody.velocity = controller.transform.forward * ThrowSpeed + controller.velocity + Vector3.up * Arc;

//				sword.animation.Play();
				animator.Throw (throwMode);
//				sword.GetComponent<DamageOnCollision>().DamagesEnemy = true;
			}
		}

	}

	public void Throw () {
		if (sword != null){
			hasSword = false;
			sword.Held = false;
			sword.transform.parent = null;
			sword.transform.rotation = this.transform.rotation;
			sword.rigidbody.isKinematic = false;
			Vector3 swordPos = sword.transform.position;
			swordPos.y = this.transform.position.y;
			sword.transform.position = swordPos;
			sword.rigidbody.velocity = controller.transform.forward * ThrowSpeed + controller.velocity + Vector3.up * Arc;
			sword.animation.Play();
			if (sword.GetComponent<Boomerang>() != null)
				sword.IgnorePlayer (false);
			sword.SetIsFlying = true;
			sword.GetComponent<DamageOnCollision>().DamagesEnemy = true;
			if (sword.gameObject.GetComponent<Boomerang>()!= null){
				sword.gameObject.GetComponent<Boomerang>().Throw();
			}
		}
	}

	//Pick up the sword
	void OnTriggerEnter(Collider col){
		if (col.GetComponent<Sword>() != null) {
			sword = col.GetComponent<Sword>();
			sword.Pickup(ThrowingHand);
			hasSword = true;
			throwMode = sword.SwordVersion;
			modeGUI.ChangeIcon(throwMode);
			Debug.Log ("sword got");

		}
	}

	public void LearnThrow() {
		MaxMode = Mathf.Clamp (MaxMode + 1, 1, 3);
	}

	public void DestroySwords(){
		Sword[] swords = FindObjectsOfType<Sword>();
		if (swords != null) {
			if (swords.Length > 0){
				foreach (Sword sword in swords) {
					Destroy (sword.gameObject);		
				}
			}
		}
	}

}
