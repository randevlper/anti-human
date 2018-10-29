using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGrabMotor : CharacterMotor {

	protected override void Start(){
		base.Start();
	}
	protected override void Update(){
		base.Update();
	}
	//Replace this behaviour
	protected override void FixedUpdate() {
		StepPhysics();
		UpdateTargetDirection();
		velocity += targetDirection;
		StepPhysicsEnd();
	}

	protected override void OnControllerColliderHit(ControllerColliderHit hit) {
		base.OnControllerColliderHit(hit);
	}
}
