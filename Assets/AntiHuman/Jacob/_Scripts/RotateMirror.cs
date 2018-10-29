using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMirror : MonoBehaviour {

    public GameObject mirror;
    public float speed;

    private void OnTriggerStay(Collider other)
    {
        //mirror.transform.Rotate(new Vector3(0, mirror.transform.rotation.y + (speed * Time.deltaTime), 0), Space.World);
        mirror.transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
