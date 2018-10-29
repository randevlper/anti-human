using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//make particles activate when gameobject gets hit and ragdolls
//deactivate when the afterwards
public class RagdollBlood : MonoBehaviour
{
    public ParticleSystem bloodParticle;
    CharacterRagdoll ragdoll;
    // Use this for initialization
    void Start()
    {
        
        ragdoll = GetComponent<CharacterRagdoll>();
        ragdoll.OnRagdoll += StartRagdollPart;
    }
    void StartRagdollPart(bool isRagdolled)
    {
        if(isRagdolled)
        {
            bloodParticle.Play(true);
        }
    }
}
