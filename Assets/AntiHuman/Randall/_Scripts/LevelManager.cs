using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	public string[] scenes;

	// Use this for initialization
	void Start () {
		foreach (var item in scenes) {
			SceneManager.LoadScene (item, LoadSceneMode.Additive);
		}
	}
}