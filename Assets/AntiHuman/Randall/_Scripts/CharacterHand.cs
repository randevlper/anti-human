using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHand : MonoBehaviour {
//	public Joint joint;
	public Transform holdPoint;
	public bool useJoint;
	bool _canGrab;

	Rigidbody grabbedBody;

	public bool CanGrab {
		get { return _canGrab; }
		set {
			_canGrab = value;
			if (!_canGrab) {
				//joint.connectedBody = null;
				if (grabbedBody != null) {
					//grabbedBody.isKinematic = false;
					//grabbedBody.GetComponent<Collider> ().enabled = true;
					if (useJoint) {

					} else {
						grabbedBody.transform.parent = null;
						grabbedBody.isKinematic = false;
					}
					grabbedBody = null;
				}
			}
		}
	}

	private void OnTriggerEnter (Collider other) {
		//Check if the other is a collider
		if (_canGrab && (grabbedBody == null)) {
			Debug.Log ("Attempting to Grab " + other.name);
			grabbedBody = other.GetComponent<Rigidbody> ();
			if (grabbedBody != null) {
				if (useJoint) {
					//joint.connectedBody = grabbedBody;
				} else {
					grabbedBody.transform.parent = holdPoint;
					grabbedBody.transform.rotation = holdPoint.rotation;
					grabbedBody.transform.position = holdPoint.position;
					grabbedBody.isKinematic = true;
					//grabbedBody.transform.localPosition = grabbedBody.transform.position - transform.position;
				}

				//grabbedBody.isKinematic = true;
				//grabbedBody.GetComponent<Collider> ().enabled = false;
			}
		}
	}

}