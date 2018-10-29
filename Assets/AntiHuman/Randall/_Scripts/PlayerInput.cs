using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	[SerializeField] CharacterController characterController;
	[SerializeField] Animator anim;
	[SerializeField] CharacterMotor motor;
	[SerializeField] CharacterGrabMotor grabMotor;
	[SerializeField] CharacterHandController handController;
	[SerializeField] CharacterRagdoll ragdoll;
	[SerializeField] CameraFocusSwitch cameraFocus;
	[SerializeField] CharacterRopeswingController ropeswingController;
	[SerializeField] PositionReset positionReset;
	Camera mainCamera;

	public KeyCode ragdollKeyCode;
	public KeyCode ragdollDebugKeycode;
	public KeyCode screenUnlockKeycode;
	public KeyCode sprintKeyCode;
	public KeyCode jumpKeycode;	

	Gold.FiniteStateMachine stateMachine;

	// Use this for initialization
	void Start () {
		ragdoll.OnRagdoll += OnRagdoll;
		ropeswingController.OnRopegrab += OnRopegrab;
		handController.OnObjectGrab += OnObjectGrab;
		handController.OnObjectThrow += OnObjectThrow;
		positionReset.OnBelowYLevel += OnPositionReset;


		Cursor.lockState = CursorLockMode.Locked;
		mainCamera = Camera.main;
		grabMotor.enabled = false;

		stateMachine = new Gold.FiniteStateMachine ();
		stateMachine.Add (new Gold.FiniteStateMachineNode (Standard, StandardEnter, StandardExit), "Standard");
		stateMachine.Add (new Gold.FiniteStateMachineNode (Ragdolled, RagdollEnter), "Ragdolled");
		stateMachine.Add (new Gold.FiniteStateMachineNode (Ropeswing, RopeswingEnter, RopeswingExit), "Ropeswing");
		stateMachine.Add (new Gold.FiniteStateMachineNode (MovingObject, MovingObjectEnter, MovingObjectExit), "MovingObject");
		stateMachine.Start ("Standard");
	}

	void OnPositionReset(Transform respawn){
		if(respawn != null){
			ragdoll.hips.transform.position = respawn.position;
		} else {
			Debug.Log("Respawn Position not set");
			ragdoll.hips.transform.position = Vector3.zero;
		}
		Physics.SyncTransforms();
		ragdoll.Ragdoll(true, Vector3.zero);
	}

	void OnRagdoll (bool value) {
		if (value) {
			stateMachine.ChangeState ("Ragdolled");
		} else {
			cameraFocus.Switch (0);
			anim.enabled = true;
			anim.Play ("Getup");
		}
	}

	//Animation event call
	public void EnableCharacter () {
		stateMachine.ChangeState ("Standard");
	}

	void OnRopegrab (RopeSegment rope) {
		if (rope != null) {
			stateMachine.ChangeState ("Ropeswing");
			ragdoll.ApplyVelocityToBones(motor.Velocity);
			motor.Zero();
		}
	}

	void OnObjectGrab (Grabbable grabbable) {
		//Switch to grab state
		if (grabbable != null) {
			if (grabbable.isHeavy) {
				stateMachine.ChangeState ("MovingObject");
			}
		} else {
			stateMachine.ChangeState ("Standard");
		}
	}

	void OnObjectThrow(Grabbable grabbable){
		
	}

	// Update is called once per frame
	void Update () {
		stateMachine.Tick ();
		if (Input.GetKeyDown (ragdollDebugKeycode)) {
			stateMachine.ChangeState ("Standard");
			ragdoll.Ragdoll (!ragdoll.IsRagdolled, Vector3.zero);
		}

		if (IsUnlockButtonPressed ()) {
			Cursor.lockState = CursorLockMode.None;
			return;
		} else {
			Cursor.lockState = CursorLockMode.Locked;
		}

		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}

		if(Input.GetKeyDown(KeyCode.R)){
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
	}

	void MovingObjectEnter () {
		anim.enabled = true;
		grabMotor.enabled = true;
		characterController.enabled = true;
	}

	void MovingObject () {
		grabMotor.CharInput = GetInput ();

		Grab ();

		if (Input.GetButtonDown ("Jump")) { grabMotor.Jump (); }

		if (Input.GetKeyDown (ragdollKeyCode)) {
			//Switch to standard state
			stateMachine.ChangeState ("Standard");
			//Ragdoll
		}
	}

	void MovingObjectExit () {
		grabMotor.enabled = false;
		characterController.enabled = false;
	}

	Vector2 GetInput () {
		return new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
	}

	void StandardEnter () {
		anim.enabled = true;
		characterController.enabled = true;
		motor.enabled = true;
		handController.Ungrab();
		ropeswingController.Zero();
	}

	void Standard () {
		motor.CharInput = GetInput ();

		if (Input.GetButtonDown ("Jump")) { motor.Jump (); }
		Throw();
		//handController.ReachOut (true);
		Grab ();

		if (Input.GetKeyDown (sprintKeyCode)) {
			motor.isSprinting = true;
		} else if (Input.GetKeyUp (sprintKeyCode)) {
			motor.isSprinting = false;
		}

		if (Input.GetKeyDown (ragdollKeyCode)) {
			ragdoll.Ragdoll (!ragdoll.IsRagdolled, Vector3.zero);
		}
	}

	void Throw(){
		if(Input.GetButtonDown("Fire2")){
			handController.Throw(mainCamera.ViewportPointToRay(new Vector2(0.5f,0.5f)).direction);
		}
	}

	void StandardExit () {
		motor.isSprinting = false;
		characterController.enabled = false;
		motor.enabled = false;
		anim.enabled = false;
		handController.Ungrab();
	}

	void Grab () {
		if (Input.GetButton ("Fire1")) {
			handController.ReachOut (true);
		} else if (Input.GetButtonUp ("Fire1")) {
			handController.ReachOut (false);
		}
	}

	void RagdollEnter(){
		handController.ReachOut (false);
		cameraFocus.Switch (1);

		if (!ropeswingController.IsOnRope) {
			ragdoll.ApplyVelocityToBones (motor.Velocity);
		}
	}

	void RagdollExit(){

	}

	void Ragdolled () {
		RagdollMovement ();
		RagdollJumping ();
		RagdollGetup ();
	}

	void RopeswingEnter(){
		//Debug.Log("Entering Ropeswing");
	}

	public KeyCode keycodeRopeUp;
	public KeyCode keycodeRopeDown;

	void Ropeswing () {
		RagdollMovement ();

		if(Input.GetKeyDown(keycodeRopeUp)){
			ropeswingController.Move(-1);
		}

		if(Input.GetKeyDown(keycodeRopeDown)){
			ropeswingController.Move(1);
		}

		if (Input.GetKeyDown (jumpKeycode) || Input.GetKeyDown(ragdollKeyCode)) {
			//Debug.Log("Letting go!");
			stateMachine.ChangeState ("Ragdolled");
		}
	}

	void RopeswingExit () {
		//Debug.Log("Exiting ropeswing");
		motor.Zero();
		ropeswingController.LetGo ();
	}

	void RagdollMovement () {
		Vector2 input = GetInput ();
		var forward = mainCamera.transform.TransformDirection (Vector3.forward);
		var right = mainCamera.transform.TransformDirection (Vector3.right);
		forward *= input.y;
		right *= input.x;
		ragdoll.ApplyVelocityToBones ((forward + right).normalized * Time.deltaTime * 5f);
	}

	void RagdollJumping () {
		if (Input.GetKeyDown (jumpKeycode) && ragdoll.OnGround ()) {
			ragdoll.ApplyVelocityToBones (Vector3.up * 3f);
		}
	}

	void RagdollGetup () {
		if (Input.GetKeyDown (ragdollKeyCode)) {
			if (ragdoll.OnGround ()) {
				transform.position = ragdoll.hips.position;
				ragdoll.Ragdoll (!ragdoll.IsRagdolled, Vector3.zero);
			}
		}
	}

	public bool IsUnlockButtonPressed () {
		if (Input.GetKey (screenUnlockKeycode)) {
			Cursor.lockState = CursorLockMode.None;
			return true;
		}
		return false;
	}
}