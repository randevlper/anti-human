using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	private void OnTriggerEnter(Collider other) {
		PlayerInput playerInput = other.GetComponent<PlayerInput>();
		if(playerInput != null){
			other.GetComponent<PositionReset>().respawnPoint = transform;
		}
	}
}
