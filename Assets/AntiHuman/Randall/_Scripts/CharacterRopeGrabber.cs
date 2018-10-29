using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRopeGrabber : MonoBehaviour {
	[SerializeField] CharacterRopeswingController ropeswingController; 
	private void OnTriggerEnter(Collider other) {
		RopeSegment segment = other.GetComponent<RopeSegment> ();
		if(segment!=null){
			ropeswingController.Grab(segment);
		}

	}
}
