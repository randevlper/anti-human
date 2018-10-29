using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserButtonGlow : LaserButton {

    [SerializeField] Material on;
    [SerializeField] Material off;
    [SerializeField] MeshRenderer mesh;
    //When the laser hits this thing
    public UnityEngine.Events.UnityEvent OnHitStart;
    public UnityEngine.Events.UnityEvent OnHitEnd;
    override public void OnLaserHit()
    {
        mesh.material = on;
        OnHitStart.Invoke();
    }

    //While the laser is hitting this thing
    override public void OnLaser()
    {
    }

    //When the laser stops hitting this thing
    override public void OnLaserStopHitting()
    {
        mesh.material = off;
        OnHitEnd.Invoke();
    }
}
