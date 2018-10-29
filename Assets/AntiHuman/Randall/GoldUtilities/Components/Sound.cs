using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {
	public AudioSource audioSource;
	void Update () {
		if (!audioSource.isPlaying) {
			gameObject.SetActive (false);
		}
	}
}