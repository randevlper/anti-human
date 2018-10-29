using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReset : MonoBehaviour {

	[SerializeField] PositionReset positionReset;
	[SerializeField] CharacterRagdoll ragdoll;
	[SerializeField] CharacterRopeswingController ropeswingController;
	[SerializeField] Grabbable grabbable;

	private void Start() {
		positionReset.OnBelowYLevel += OnReset;
		grabbable.OnGrab += OnGrab;
	}

	void OnGrab(GameObject other){
		ropeswingController.Zero();
	}

	void OnReset(Transform value){
		if(ragdoll.IsRagdolled){
			ragdoll.hips.position = value.position;
			ragdoll.Ragdoll(false, Vector3.zero);
			ragdoll.Ragdoll(true, Vector3.zero);
			Physics.SyncTransforms();
		}
	}
}
