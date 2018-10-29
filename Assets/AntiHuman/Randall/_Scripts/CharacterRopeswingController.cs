using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRopeswingController : MonoBehaviour {
	[SerializeField] CharacterRagdoll ragdoll;
	[SerializeField] Transform hips;
	Joint hipsJoint;

	public Gold.Delegates.ActionValue<RopeSegment> OnRopegrab;

	bool _isOnRope = false;

	RopeRenderer grabbedRope;
	RopeSegment grabbedSegment;

	public bool IsOnRope {
		get { return _isOnRope; }
	}

	public void Grab (RopeSegment rope) {
		if (rope.ropeRenderer != grabbedRope) {
			grabbedRope = rope.ropeRenderer;
			//Ragdoll the character
			//Move hips to the rope position
			//Attach the joint to the hips
			_isOnRope = true;
			if (!ragdoll.IsRagdolled) {
				ragdoll.Ragdoll (true, Vector3.zero);
			}
			Attach (rope);
			OnRopegrab?.Invoke (rope);
			Physics.SyncTransforms ();
		}

	}

	void Attach (RopeSegment rope) {
		hips.transform.position = rope.transform.position;
		hipsJoint = hips.gameObject.AddComponent<CharacterJoint> ();
		hipsJoint.connectedBody = rope.rb;
		grabbedSegment = rope;
	}

	public void LetGo () {
		_isOnRope = false;
		grabbedSegment = null;
		Destroy (hipsJoint);
	}

	public void Zero () {
		grabbedRope = null;
	}

	//MoveUp
	public void Move (int value) {
		List<Transform> transforms = grabbedRope.GetTransforms ();
		int newsegment = Mathf.Clamp (grabbedSegment.value + value, 1, transforms.Count - 1);
		Destroy (hipsJoint);
		Attach (transforms[newsegment].GetComponent<RopeSegment> ());
	}

	//MoveDown
}