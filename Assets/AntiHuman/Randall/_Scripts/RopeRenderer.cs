using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour {
	[SerializeField] LineRenderer lineRenderer;
	[SerializeField] List<Transform> transforms;
	[SerializeField] List<Rigidbody> rigidbodies;

	[Range (2, 10)]
	public int segmentsNumberOf = 2;

	[Range (0.1f, 1f)]
	public float segmentsDistanceBetween = 0.1f;

	[Range (0.1f, 1f)]
	public float colliderRadius = 0.1f;

	public float mass;

	public bool build;
	[Range (0, 1f)]
	public float slowTime;

	//public float width;
	// List<BoxCollider> boxColliders;

	private void Awake () {
		// boxColliders = new List<BoxCollider>();
		BuildRope ();
		SetupLineRenderer ();
		Render ();
	}

	private void Update () {
		Render ();
		foreach (var item in rigidbodies) {
			item.velocity = Vector3.Lerp (item.velocity, Vector3.zero, slowTime);
		}
	}
	
	void SetupLineRenderer () {
		if (transforms == null) {
			enabled = false;
			return;
		}
		lineRenderer.positionCount = transforms.Count;
	}

	void Render () {
		for (int i = 0; i < transforms.Count; i++) {
			lineRenderer.SetPosition (i, transforms[i].position);
		}
		lineRenderer.startWidth = colliderRadius * 2;
		lineRenderer.endWidth = colliderRadius * 2;
	}

	public List<Transform> GetTransforms(){
		return transforms;
	}

	System.Type[] ropeSegmentComp = {
		typeof (Rigidbody),
		typeof (CharacterJoint),
		typeof (SphereCollider),
		typeof (RopeSegment)
	};
	void BuildRope () {
		if (transforms.Count != segmentsNumberOf + 1) {

			foreach (Transform t in transforms) {
				if(t != transform){
					DestroyImmediate (t.gameObject, true);
				}
			}

			transforms = new List<Transform> ();

			Vector3 segmentDist = new Vector3 (0, -segmentsDistanceBetween, 0);

			GameObject actingOn = null;
			Transform lastTransform = transform;
			transforms.Add (lastTransform);

			Rigidbody lastRigidbody = GetComponent<Rigidbody> ();
			CharacterJoint joint;
			SphereCollider col;
			RopeSegment segment;

			rigidbodies = new List<Rigidbody> ();

			for (int i = 0; i < segmentsNumberOf; i++) {
				actingOn = new GameObject ("segment " + i, ropeSegmentComp);
				//Setup Parenting
				actingOn.transform.parent = lastTransform;
				lastTransform = actingOn.transform;
				transforms.Add (lastTransform);
				//Move
				lastTransform.localPosition = segmentDist;
				//Attach
				joint = actingOn.GetComponent<CharacterJoint> ();
				joint.connectedBody = lastRigidbody;
				joint.enableProjection = true;
				//Rigidbody
				lastRigidbody = actingOn.GetComponent<Rigidbody> ();
				lastRigidbody.mass = mass;
				rigidbodies.Add (lastRigidbody);
				//SetupCollider
				col = actingOn.GetComponent<SphereCollider> ();
				col.isTrigger = true;
				col.radius = colliderRadius * 2;
				//Setup Ropesegment
				segment = actingOn.GetComponent<RopeSegment> ();
				segment.rb = lastRigidbody;
				segment.ropeRenderer = this;
				segment.value = i + 1;

				actingOn.layer = LayerMask.NameToLayer ("Grabbable");
			}
		}
	}

	private void OnValidate () {
		if (build) {
			//ApplyRopesegment ();
			BuildRope ();
			SetupLineRenderer ();
			Render ();
			build = false;
		}

	}
}