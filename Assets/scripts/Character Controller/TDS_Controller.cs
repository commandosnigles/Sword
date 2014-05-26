using UnityEngine;
using System.Collections;

public class TDS_Controller : MonoBehaviour {

	public float WalkMultiplier = 0.7f;

	private CharacterAnimations animator;
	private TDS_Motor motor;
	private bool frozen = false;

	public bool SetFrozen {
		set { frozen = value; }
	}
	public bool GetFrozen {get{return frozen;}}

	// Use this for initialization
	void Awake () {
		animator = gameObject.GetComponentInChildren<CharacterAnimations>();
		motor = gameObject.GetComponent<TDS_Motor>();

	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Pause")){
			if(!GUIPauseMenu.instance.GetPaused) GUIPauseMenu.instance.Pause();
			else GUIPauseMenu.instance.Unpause ();
		}

		Vector3 inputDirection = Vector3.zero;
		float speedScale = WalkMultiplier;

		if (!frozen) {
			//create vector for horizontal motion, capped at unit vector
			inputDirection = Vector3.ClampMagnitude(new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical")),1);

			if(Input.GetAxis ("Sprint") < 0 ) speedScale = 1;
		}

		motor.UpdateVector(inputDirection, speedScale);

		if (!frozen) {
			if (Input.GetButtonDown("Jump")) {
				motor.Jump();
				animator.Jump ();
			}
			else if (Input.GetButton("Jump"))
				motor.HighJump();
		}

		motor.ApplyVelocity();
		
	}
}
