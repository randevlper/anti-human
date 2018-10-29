using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gold;

public class SoundManager : MonoBehaviour {

	ObjectPool soundPool;
	public GameObject soundPrefab;

	static public SoundManager instance;

	private void Awake () {
		if (instance == null) {
			instance = this;
			soundPool = new ObjectPool (soundPrefab, 50, true);
		} else {
			Destroy (this);
		}
	}

	public void Play (AudioClip clip, GameObject other, float volume) {
		GameObject sound = soundPool.Get ();
		sound.SetActive (true);

		AudioSource audio = sound.GetComponent<AudioSource> ();
		if (audio != null) {
			audio.clip = clip;
			audio.volume = volume;
			sound.transform.position = other.transform.position;
			audio.Play ();
		}
	}

}
