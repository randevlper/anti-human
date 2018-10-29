using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandController : MonoBehaviour {

	[SerializeField] Animator animController;
	[SerializeField] CharacterRopeswingController ropeswingController;
	bool ikActive;
	//public float distanceToActivate;
	//public Transform rightHandObj;
	public Transform holdPoint;
	public FixedJoint joint;
	public float grabRadius;
	public LayerMask grabMask;
	public Transform rightHandHold;
	public Transform leftHandHold;
	public float throwSpeed = 10f;
	Grabbable grabbedObject;
	Collider[] overlapColliders;
	public Gold.Delegates.ActionValue<Grabbable> OnObjectGrab;
	public Gold.Delegates.ActionValue<Grabbable> OnObjectThrow;

	public bool CanGrab {
		get { return canGrab; }
	}

	bool canGrab = true;
	public float grabResetTime = 0.5f;
	//Raycast from the center of unitychan to hold out her hands
	//If the hand colliders touch a ridgid body link a pivot to them
	//Be able to grab and let go with a button
	public void ReachOut (bool value) {

		if (!canGrab) { return; }

		ikActive = value;
		if (grabbedObject == null && value == true) {
			//Sphere detect for things to grab
			overlapColliders = Physics.OverlapSphere (holdPoint.position, grabRadius, grabMask);
			foreach (Collider col in overlapColliders) {
				Grabbable grabbable = col.GetComponent<Grabbable> ();
				RopeSegment segment = col.GetComponent<RopeSegment> ();
				//Rules for all ridgid bodies
				if (segment != null) {
					//Rules for when a rope is grabbed
					ropeswingController.Grab (segment);
					break;
				} else if (grabbable != null) {

					if (grabbable.isGrabable) {
						grabbedObject = grabbable;
						//body.isKinematic = true;

						IRagdoll ragdoll = grabbedObject.GetComponent<IRagdoll> ();
						if (ragdoll != null) {
							ragdoll.GetRagdoll ().transform.parent = holdPoint;
						} else {
							grabbable.transform.parent = holdPoint;
						}

						if (!grabbedObject.isHeavy) {
							grabbable.transform.position = holdPoint.position;
							grabbable.transform.rotation = holdPoint.rotation;
						}
						if (joint == null) {
							joint = holdPoint.gameObject.AddComponent<FixedJoint> ();
						}
						joint.connectedBody = grabbable.rb;

						Physics.SyncTransforms ();
						OnObjectGrab?.Invoke (grabbedObject);
						grabbedObject?.OnGrab?.Invoke (gameObject);
						break;
					}
				}
			}
		}

		if (grabbedObject != null && value == false) {
			Ungrab ();
			OnObjectGrab?.Invoke (null);
		}
	}

	public void Ungrab () {
		if (joint != null) {
			joint.connectedBody = null;
		}

		if (grabbedObject != null) {
			grabbedObject.OnEndGrab?.Invoke (gameObject);
			grabbedObject.rb.isKinematic = false;

			IRagdoll ragdoll = grabbedObject.GetComponent<IRagdoll> ();
			if (ragdoll != null) {
				ragdoll.GetRagdoll ().transform.parent = null;
			} else {
				grabbedObject.transform.parent = null;
			}

			grabbedObject = null;
		}
	}

	public void Throw (Vector3 dir) {
		if (grabbedObject != null) {
			Grabbable toThrow = grabbedObject;
			ReachOut (false);
			IRagdoll ragdoll = toThrow.GetComponent<IRagdoll> ();
			if (ragdoll != null) {
				ragdoll.ApplyVelocityToBones (dir * throwSpeed);
			} else {
				toThrow.rb.velocity = dir * throwSpeed;
			}
			canGrab = false;
			Invoke ("EnableGrab", grabResetTime);
		}
	}

	void EnableGrab () {
		canGrab = true;
	}

	private void OnAnimatorIK (int layerIndex) {
		if (animController) {

			if (ikActive) {
				//float distToRHObj = Vector3.Distance (animController.GetBoneTransform (HumanBodyBones.Head).position, rightHandObj.position);

				if (rightHandHold != null) {
					animController.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1);
					animController.SetIKRotationWeight (AvatarIKGoal.LeftHand, 1);
					animController.SetIKPosition (AvatarIKGoal.LeftHand, leftHandHold.position);
					animController.SetIKRotation (AvatarIKGoal.LeftHand, leftHandHold.rotation);
				}

				if (leftHandHold != null) {
					animController.SetIKPositionWeight (AvatarIKGoal.RightHand, 1);
					animController.SetIKRotationWeight (AvatarIKGoal.RightHand, 1);
					animController.SetIKPosition (AvatarIKGoal.RightHand, rightHandHold.position);
					animController.SetIKRotation (AvatarIKGoal.RightHand, rightHandHold.rotation);
				}

				// if (lookObj != null) {
				// 	animController.SetLookAtWeight (1);
				// 	animController.SetLookAtPosition (lookObj.position);
				// }
			} else {
				animController.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0);
				animController.SetIKRotationWeight (AvatarIKGoal.LeftHand, 0);
				animController.SetIKPositionWeight (AvatarIKGoal.RightHand, 0);
				animController.SetIKRotationWeight (AvatarIKGoal.RightHand, 0);
				animController.SetLookAtWeight (0);
			}
		}
	}

	private void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (holdPoint.transform.position, grabRadius);
	}
	//if the IK is not active, set the position and rotation of the hand and head back to the original position
}