using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusSwitch : MonoBehaviour {
	
	[System.Serializable]
	public struct CameraFocuses
	{
		public Transform Follow;
		public Transform LookAt;
	}
	public Cinemachine.CinemachineVirtualCameraBase cameraBase;
	public CameraFocuses[] focuses;

	public void Switch (int value) {
		cameraBase.Follow = focuses[value].Follow;
		cameraBase.LookAt = focuses[value].LookAt;
	}
}