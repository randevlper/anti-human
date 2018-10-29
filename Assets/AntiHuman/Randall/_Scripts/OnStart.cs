using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStart : MonoBehaviour {
	public UnityEngine.Events.UnityEvent EventOnStart;
	void Start () { EventOnStart.Invoke(); }
}
