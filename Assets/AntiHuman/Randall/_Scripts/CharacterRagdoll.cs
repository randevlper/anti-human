using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRagdoll {
	void Ragdoll (bool value, Vector3 dir);
	void ApplyVelocityToBones (Vector3 value);
	bool GetIsRagdolled ();
	CharacterRagdoll GetRagdoll();
}
public class CharacterRagdoll : MonoBehaviour, IRagdoll {

	public Transform hips;
	[SerializeField] List<Rigidbody> rigidbodys;
	[SerializeField] List<Collider> colliders;
	[SerializeField] List<GameObject> specialObjectsToDisable;
	[SerializeField] List<Collider> collidersToDisable;
	[SerializeField] List<GameObject> ignoreThese;
	bool isRagdolled;
	public Gold.Delegates.ActionValue<bool> OnRagdoll;
	public LayerMask groundMask;
	public float groundDetectDist;
	public bool startRagdolled;
	public bool enableProjection;
	public bool IsRagdolled {
		get { return isRagdolled; }
	}
	private void Start () {
		Ragdoll (startRagdolled, Vector3.zero);
	}

	public void Ragdoll (bool value, Vector3 velocity) {
		isRagdolled = value;

		foreach (GameObject go in specialObjectsToDisable) {
			go.SetActive (!value);
		}

		foreach(Collider co in collidersToDisable){
			co.enabled = !value;
		}

		foreach (Collider col in colliders) {
			col.enabled = value;
		}

		if (value) {
			foreach (Rigidbody rb in rigidbodys) {
				rb.MovePosition (rb.transform.position + new Vector3 (0, 0.5f, 0));
				rb.velocity = velocity + Vector3.up;
				rb.angularVelocity = Vector3.zero;
			}
		}

		OnRagdoll?.Invoke (value);
		Physics.SyncTransforms();
	}

	public bool GetIsRagdolled () {
		return isRagdolled;
	}

	public List<Rigidbody> GetRigidbodies(){
		return rigidbodys;
	}
	void GetRagdollParts (Transform t) {
		colliders = new List<Collider> ();
		rigidbodys = new List<Rigidbody> ();

		Collider col = hips.GetComponent<Collider> ();
		Rigidbody rb = hips.GetComponent<Rigidbody> ();

		if (col != null) {
			colliders.Add (col);
		}
		if (rb != null) {
			rigidbodys.Add (rb);
		}

		GetParts (t);

		foreach (var item in rigidbodys) {
			RagdollPart part = item.GetComponent<RagdollPart> ();
			if (part == null) {
				part = item.gameObject.AddComponent<RagdollPart> ();
			}
			part.ragdoll = this;
			item.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}
	}

	void GetParts (Transform t) {
		foreach (Transform child in t) {
			GetParts (child);
			if (ShouldIgnoreThis (child.gameObject)) { continue; }
			Collider col = child.GetComponent<Collider> ();
			if (col != null) {
				CharacterJoint joint = child.GetComponent<CharacterJoint> ();
				if (joint != null) {
					joint.enableProjection = enableProjection;
				}
				if (!col.isTrigger) {
					colliders.Add (col);
					Rigidbody rb = child.GetComponent<Rigidbody> ();
					if (rb != null) {
						rigidbodys.Add (rb);
					}
				}
			}
		}
	}

	bool ShouldIgnoreThis (GameObject go) {
		foreach (var item in ignoreThese) {
			if (go == item) {
				return true;
			}
		}
		return false;
	}

	private void OnValidate () {
		if (hips != null) {
			GetRagdollParts (hips);
		}
	}

	public void ApplyVelocityToBones (Vector3 value) {
		foreach (var item in rigidbodys) {
			item.velocity += value;
		}
	}

	public CharacterRagdoll GetRagdoll(){
		return this;
	}

	RaycastHit groundHit;
	public bool OnGround () {
		bool hit = Physics.Raycast (hips.transform.position, Vector3.down, out groundHit, groundDetectDist, groundMask);
		Debug.DrawRay (hips.position, Vector3.down * groundDetectDist);
		return hit;
	}
}