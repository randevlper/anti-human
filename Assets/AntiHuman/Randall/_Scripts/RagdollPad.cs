using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPad : MonoBehaviour {
	public float velocity;
	private void OnTriggerEnter (Collider other) {
		IRagdoll ragdoll = other.GetComponent<IRagdoll> ();
		Rigidbody rb = other.GetComponent<Rigidbody>();
		Vector3 newDir = transform.up;
		if(ragdoll != null){
			ragdoll?.Ragdoll (true, newDir * velocity);
			ragdoll.ApplyVelocityToBones(newDir*velocity);
		} else if (rb != null){
			rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.y) + (newDir * velocity);
		}
	}
}