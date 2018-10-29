using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPart : MonoBehaviour, IRagdoll {
	public CharacterRagdoll ragdoll;
	public void Ragdoll (bool value, Vector3 dir) {
		ragdoll.Ragdoll (value, dir);
	}

	public bool GetIsRagdolled () {
		return ragdoll.IsRagdolled;
	}

	public void ApplyVelocityToBones (Vector3 value) {
		ragdoll.ApplyVelocityToBones (value);
	}

	public CharacterRagdoll GetRagdoll(){
		return ragdoll;
	}
}