using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableKiller : MonoBehaviour {

	private void OnCollisionEnter(Collision other) {
		Grabbable grabbable = other.gameObject.GetComponent<Grabbable>();
		if(grabbable != null){
			grabbable.gameObject.SetActive(false);
		}
	}

}
