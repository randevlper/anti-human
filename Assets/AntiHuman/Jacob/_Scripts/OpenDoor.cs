using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Split")
        {
            Debug.Log("hit");
        }
    }
}
// check if obj has script
// call method in script x2 or x3