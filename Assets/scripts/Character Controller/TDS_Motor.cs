using UnityEngine;
using System.Collections;

public class TDS_Motor : MonoBehaviour {
	//character variables

	public float TractionFraction = .25f;
	public float AerialManeuvering = .03f;
	public float RotationSpeed = 45f;
	public float MaxRunSpeed = 12f;
	public float JumpSpeed = 12.0f;
	public float HoldJump = 16.0f;
	public float Gravity = 38.0f;
	public int AirJumps = 1;

	private Vector3 targetDirection;
	private CharacterController controller;
	private Vector3 currentVelocity;
	private float verticalVelocity;
	private int jumpCount;
	private bool wasGrounded;

	public Vector3 GetCurrentVelocity{
		get {return currentVelocity;}
	}

	void Awake () {
		controller = (CharacterController) GetComponent<CharacterController>();
		targetDirection = Vector3.right;
	}	

	//**** STEP ONE ****\\
	//give me a direction to go and a 0-1 scale factor for my speed, a value of 1 resulting in max run speed
	public void UpdateVector (Vector3 inputVector, float speedScale) {

		//character motion math\\
		Vector3 inputDirection = inputVector;
		//fraction of max run speed based on the extent to which controls are pushed squared for more precise control in the lower ranges
		float targetSpeed = MaxRunSpeed * Mathf.Clamp01 (speedScale) * inputDirection.magnitude * inputDirection.magnitude;

		//create a vector for applying a velocity
		currentVelocity = Vector3.zero;

		if(controller.isGrounded){
			wasGrounded = true;
			//apply input to character velocity using ground mechanics
			currentVelocity = Vector3.Lerp (controller.velocity, inputDirection.normalized * targetSpeed, TractionFraction);
			verticalVelocity = -Gravity * Time.deltaTime * Mathf.Max (1, targetSpeed);
			//reset jumping parameters
			jumpCount = 0;

		}
		else {
			if ((wasGrounded && controller.velocity.y < 0) || controller.velocity.y == 0f)
				verticalVelocity = 0f;
			wasGrounded = false;
			//apply input to character velocity using aerial mechanics
			currentVelocity = Vector3.Lerp (controller.velocity, inputDirection.normalized * targetSpeed, AerialManeuvering);
			//incase you hit your head
//			if (controller.velocity.y == 0f){
//				verticalVelocity = 0f;
//			}
			verticalVelocity -= Gravity * Time.deltaTime;
		}

		//set the target facing direction to the horizontal velocity vector whenever there is directional input
		if (inputDirection != Vector3.zero){
				targetDirection = Vector3.Scale(currentVelocity, new Vector3(1,0,1));
		}

		//rotate character to face in direction of velocity
		Quaternion qRotate = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection,Vector3.up), RotationSpeed * RotationSpeed * Time.deltaTime * inputDirection.magnitude);
		transform.rotation = qRotate;
	}
	//**** STEP TWO A****\\(optional)
	public void Jump(){
		if(controller.isGrounded){
			//jump from ground
			verticalVelocity = JumpSpeed;
		}
		else{
			//air jump aka double jump
			if (jumpCount < AirJumps) {
				verticalVelocity = JumpSpeed;
				jumpCount ++;
			}
		}
	}
	//**** STEP TWO B ****\\(optional)
	public void HighJump(){
		//hold jump to jump higher and for longer
		if (jumpCount < Mathf.Max (1, AirJumps) && verticalVelocity > 0)
			verticalVelocity += HoldJump * Time.deltaTime;
	}
	//**** STEP THREE ****\\
	public void ApplyVelocity(){
		//apply velocity
		currentVelocity.y = verticalVelocity;
		controller.Move(currentVelocity * Time.deltaTime);
	}

	public void MoreJumps () {
		AirJumps = Mathf.Clamp (AirJumps+1, 0, 2);
	}
}
