using UnityEngine;
using System.Collections;

public class CharacterAnimations : MonoBehaviour {

	public float FlailingSpeed = -11f;
	public float MaxTurningSpeed = .5f;
	public float MinTurningSpeed = .01f;
	public float WalkAnimationSpeedCompensation = 3f;
	public Transform Spine;

	private Animation animation;
	private TDS_Controller controller;
	private TDS_Motor motor;
	private SwordControls swordController;
	private CharacterController charController;
	private Vector3 currentVelocity;
	private float maxRunSpeed;
	private float walkMultiplier;
	private string[] throwNames;
	
	private enum states {idle, run, walk, turnL, turnR, throwing, jump, fall, flail, summon, dead, ready};
	private states state;

	void Awake () {
		motor = this.gameObject.transform.parent.GetComponent<TDS_Motor>();
		controller = this.gameObject.transform.parent.GetComponent<TDS_Controller>();
		charController = this.gameObject.transform.parent.GetComponent<CharacterController>();
		swordController = this.gameObject.transform.parent.GetComponent<SwordControls>();


		maxRunSpeed = motor.MaxRunSpeed;
		walkMultiplier = controller.WalkMultiplier;
		animation = this.gameObject.animation;
		throwNames = new string[3] {"ThrowOverhand", "ThrowTwist", "ThrowTwist"};
	}

	void LateUpdate () {
		if (state != states.dead && state != states.summon) {

			currentVelocity = motor.GetCurrentVelocity;
			float horizontalSpeed = new Vector3 (currentVelocity.x, 0, currentVelocity.z).magnitude;

			//******** State Management ********\\
			if (charController.isGrounded) {

				if (horizontalSpeed > MaxTurningSpeed){

					if (horizontalSpeed <= maxRunSpeed * walkMultiplier + 0.05f){
						animation["Walking"].speed = (horizontalSpeed + WalkAnimationSpeedCompensation) / (maxRunSpeed * walkMultiplier);
						if (state != states.walk) {
							state = states.walk;
							animation.CrossFade("Walking");
						}
					}
					else {
						animation["Running"].speed = horizontalSpeed/maxRunSpeed;
						if (state != states.run){
							state = states.run;
							animation.CrossFade("Running");
						}
					}
				
				}
				else if (horizontalSpeed > MinTurningSpeed) {
					Vector3 forward = transform.parent.forward;
					Vector3 horizontalVelocity = new Vector3 (currentVelocity.x, 0,currentVelocity.z );
					float turningDirection = Vector3.Cross (forward, horizontalVelocity).y;
					turningDirection = turningDirection / Mathf.Abs(turningDirection);
					if (turningDirection > 0){
						if (state != states.turnR){
							state = states.turnR;
						}
					}
					else if (turningDirection < 0){
						if (state != states.turnL){
							state = states.turnL;
						}
					}
				}

				else if(state != states.idle && state != states.throwing) {
					state = states.idle;
					animation.CrossFade("Idle");
				}
			} 

			else if (currentVelocity.y <= FlailingSpeed) {
				if (state != states.flail){
					state = states.flail;
					animation.CrossFade("Flailing");
				}
			}

			else if (currentVelocity.y < 0) {
				if(state != states.fall){
					state = states.fall;
					animation.CrossFade("Falling");
				}
			}

		}
	}


	public void Throw (int throwMode) {
		animation[throwNames[throwMode]].speed = 2;
		if (new Vector3 (currentVelocity.x, 0, currentVelocity.z).magnitude < MaxTurningSpeed){
			state = states.throwing;
			animation[throwNames[throwMode]].RemoveMixingTransform(Spine);
		}
		else
		{
			animation[throwNames[throwMode]].layer = 1;
			animation[throwNames[throwMode]].weight = 1;
			animation[throwNames[throwMode]].AddMixingTransform(Spine,true);
		}
		animation.Stop(throwNames[throwMode]);
		animation.Play(throwNames[throwMode]);

		
	}
	public void ReleaseSword () {
		swordController.Throw();
	}

	public void Jump () {
		//play jump
		state = states.jump;
		animation.Stop ("Jumping");
		animation.CrossFade("Jumping",0.1f);

	}

	public void StartSummon () {
		animation["Falling"].layer = 1;
		animation["Falling"].weight = 1;
		animation["Falling"].AddMixingTransform(Spine,true);
		animation.CrossFade("Falling");
		controller.SetFrozen = true;
	}
	public void EndSummon () {
		animation.Stop ("Falling");
		animation["Falling"].layer = 0;
		animation["Falling"].RemoveMixingTransform(Spine);
		controller.SetFrozen = false;
	}

	public void Die (){
		animation.CrossFade ("Die");
		state = states.dead;

	}

	public void Ready (){
		state = states.ready;
	}


}
