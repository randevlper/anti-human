using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRagdollReset : MonoBehaviour {
	[SerializeField] CharacterRagdoll ragdoll;
	public Transform t1;
	public Transform t2;
	public float distance;

	bool DoReset(){
		return distance < Vector3.Distance(t1.position, t2.position);
	}

	private void Update() {
		if(DoReset()){
			ragdoll.Ragdoll(false, Vector3.zero);
			ragdoll.Ragdoll(true, Vector3.zero);
		}
	}
}
