using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedObject : MonoBehaviour
{
    public Grabbable grabbable;
    public bool isAttachable = true;
    // Use this for initialization
    void Start()
    {
        grabbable.OnGrab = IsOnGrabbed;
        grabbable.OnEndGrab = IsOnEndGrabbed;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void IsOnGrabbed(GameObject other) //when player grabs the object
    {
        isAttachable = false;
    }

    void IsOnEndGrabbed(GameObject other) //when the player is no longer grabbling the object
    {
        isAttachable = true;
    }
}
