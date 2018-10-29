using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserButton : MonoBehaviour {

    //When the laser hits this thing
    virtual public void OnLaserHit()
    {
        Debug.Log("Hit start");
    }

    //While the laser is hitting this thing
    virtual public void OnLaser()
    {
        Debug.Log("Hit stay");
    }

    //When the laser stops hitting this thing
    virtual public void OnLaserStopHitting()
    {
        Debug.Log("Hit End");
    }
}
