using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRagdoll : MonoBehaviour {
	public ParticleSystem part;
	List<ParticleCollisionEvent> collisionEvents;

	void Start () {
		part = GetComponent<ParticleSystem> ();
		collisionEvents = new List<ParticleCollisionEvent> ();
	}

	void OnParticleCollision (GameObject other) {
		int numCollisionEvents = part.GetCollisionEvents (other, collisionEvents);
		IRagdoll ragdoll = other.GetComponent<IRagdoll> ();

		if (ragdoll != null) {
			if (!ragdoll.GetIsRagdolled ()) {
				ragdoll.Ragdoll (true, Vector3.zero);
			}
		}

		Rigidbody rb = other.GetComponent<Rigidbody> ();
		for (int i = 0; i < collisionEvents.Count; i++) {
			if(ragdoll != null && rb != null){
				Vector3 force = collisionEvents[i].velocity/5;
				ragdoll.ApplyVelocityToBones(force);
			}
			else if (rb != null) {
				Vector3 pos = collisionEvents[i].intersection;
				Vector3 force = collisionEvents[i].velocity;
				rb.AddForce (force);
			}
		}
	}
}