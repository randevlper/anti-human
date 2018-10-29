using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour {
	[SerializeField] protected CharacterController characterController;
	protected Camera mainCamera;
	protected Animator anim;
	public float mass;
	public float jumpForce = 10;
	public float friction = 0.1f;
	public float turnSpeed = 10f;
	public float speedMult = 2.0f;
	protected float turnSpeedMultiplier = 1f;
	protected float forwardVelocity = 0f;
	public bool isSprinting = false;
	protected Vector3 targetDirection;
	protected Vector2 input;

	public Vector2 CharInput {
		set { input = value; }
		get { return input; }
	}
	protected Quaternion freeRotation;
	//private float forwardVelocity;
	[SerializeField] protected Vector3 velocity;

	public Vector3 Velocity {
		get { return velocity; }
	}

	protected virtual void Start () {
		anim = GetComponent<Animator> ();
		mainCamera = Camera.main;
	}

	protected virtual void Update () {
		anim.SetFloat ("Vertical", velocity.y);
		anim.SetBool ("Airborne", !characterController.isGrounded);
		if (isSprinting) {
			anim.SetFloat ("Sprint", speedMult);
		} else {
			anim.SetFloat ("Sprint", 1.0f);
		}
	}

	public virtual void Jump () {
		if (characterController.isGrounded) {
			anim.SetBool ("Jump", true);
			velocity.y = jumpForce;
		}
	}

	protected virtual void LateUpdate () {

		//transform.position += (anim.deltaPosition);
		//characterController.Move (anim.deltaPosition);

	}

	//Add Jumping
	//Add CharacterController
	protected virtual void StepPhysics () {
		velocity += Physics.gravity * Time.deltaTime;
		forwardVelocity = Mathf.Abs (input.x) + Mathf.Abs (input.y);
		forwardVelocity = Mathf.Clamp (forwardVelocity, 0f, 1f);
		anim.SetFloat ("Speed", forwardVelocity);
	}

	protected virtual void FixedUpdate () {
		StepPhysics ();
		// Update target direction relative to the camera view (or not if the Keep Direction option is checked)
		UpdateTargetDirection ();

		if (input != Vector2.zero && targetDirection.magnitude > 0.1f) {
			Vector3 lookDirection = targetDirection.normalized;
			freeRotation = Quaternion.LookRotation (lookDirection, transform.up);
			float diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
			float eulerY = transform.eulerAngles.y;

			if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
			Vector3 euler = new Vector3 (0, eulerY, 0);

			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
		}
		velocity += transform.forward * forwardVelocity * (isSprinting ? speedMult : 1f);

		StepPhysicsEnd ();
	}

	protected virtual void StepPhysicsEnd () {
		velocity.x -= velocity.x * friction;
		velocity.z -= velocity.z * friction;

		//Check Collision
		//Physics.OverlapCapsule(characterController.center,characterController.bounds.max, characterController.radius);
		//Debug.DrawLine(characterController.bounds.center, characterController.bounds.center + new Vector3(0, characterController.bounds.extents.y, 0));

		characterController.Move (velocity * Time.deltaTime);
		//rb.velocity = velocity;
		if (characterController.isGrounded) {
			velocity.y = 0;
		}

	}

	protected virtual void UpdateTargetDirection () {
		var forward = mainCamera.transform.TransformDirection (Vector3.forward);
		forward.y = 0;
		//get the right-facing direction of the referenceTransform
		var right = mainCamera.transform.TransformDirection (Vector3.right);
		// determine the direction the player will face based on input and the referenceTransform's right and forward directions
		targetDirection = input.x * right + input.y * forward;
	}

	public void Zero () {
		velocity = Vector3.zero;
	}

	protected virtual void OnControllerColliderHit (ControllerColliderHit hit) {
		Rigidbody otherBody = hit.gameObject.GetComponent<Rigidbody> ();
		if (otherBody != null && hit.gameObject.layer != 29) {
			//mv = mv
			// Vector3 dir = Vector3.zero;
			// float distance;
			// Physics.ComputePenetration (
			// 	characterController, transform.position, transform.rotation,
			// 	hit.collider, hit.transform.position, hit.transform.rotation,
			// 	out dir, out distance);
			// otherBody.velocity = dir * distance;

			if (transform.position.y < otherBody.transform.position.y) {
				if (!IsHeavy (otherBody.GetComponent<Grabbable> ())) {
					//Vector3 dir = (otherBody.transform.position - (characterController.bounds.center)).normalized;
					//Vector3 v = ((mass * velocity) / otherBody.mass);
					//otherBody.velocity = dir * v.magnitude;
					otherBody.velocity += (mass * velocity) / otherBody.mass;
				}
			}

		}
	}

	bool IsHeavy (Grabbable grabbable) {
		if (grabbable != null) {
			return grabbable.isHeavy;
		}
		return false;
	}
}