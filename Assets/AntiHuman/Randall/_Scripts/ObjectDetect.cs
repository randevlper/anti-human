using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetect : MonoBehaviour {
	
	public GrabbableTypes typeToActivate;
	public UnityEngine.Events.UnityEvent OnEnter;
	public UnityEngine.Events.UnityEvent OnExit;
	Grabbable currentObjectOn;

	private void OnTriggerEnter(Collider other) {
		Grabbable grabbable = other.GetComponent<Grabbable>();
		if(grabbable != null && currentObjectOn == null){
			if(grabbable.type == typeToActivate){
				OnEnter.Invoke();
				currentObjectOn = grabbable;
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		Grabbable grabbable = other.GetComponent<Grabbable>();
		if(grabbable != null && currentObjectOn == grabbable){
			OnExit.Invoke();
			currentObjectOn = null;
		}
		
	}
}
