using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrabbableTypes
{
	Box,
	Sphere,
	Body
}

[RequireComponent (typeof (Rigidbody))]
public class Grabbable : MonoBehaviour {
	public Rigidbody rb;
	public bool isHeavy;
	public bool isGrabable = true;
	public GrabbableTypes type = GrabbableTypes.Box;

	public Gold.Delegates.ActionValue<GameObject> OnGrab;
	public Gold.Delegates.ActionValue<GameObject> OnEndGrab;

	private void OnValidate () {
		rb = GetComponent<Rigidbody> ();
	}
	private void OnCollisionEnter(Collision other) {
		IRagdoll ragdoll = other.gameObject.GetComponent<IRagdoll>();
		if(type != GrabbableTypes.Body && ragdoll != null){
			ragdoll.GetRagdoll().GetComponent<CharacterRopeswingController>().LetGo();
		}
	}
}