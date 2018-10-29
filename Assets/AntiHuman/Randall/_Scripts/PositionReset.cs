using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour {

	public float minYLevel;
	public Transform tracking;
	public Transform respawnPoint;
	public Gold.Delegates.ActionValue<Transform> OnBelowYLevel;

	public bool doRespawn;

	
	// Update is called once per frame
	void Update () {
		if(tracking.position.y < minYLevel){
			OnBelowYLevel?.Invoke(respawnPoint);
			if(doRespawn){
				Respawn();
			}
		}	
	}

	void Respawn(){
		tracking.transform.position = respawnPoint.transform.position;
	}
}
