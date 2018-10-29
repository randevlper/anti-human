using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grinder : MonoBehaviour
{
    private FixedJoint joint;
   
    // Use this for initialization
    void Start()
    {
        joint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
   
    private void OnCollisionEnter(Collision collision)
    {
        //Get the CharacterRagdoll
        //Get the GrabbableObject and check if its grabbable


        /*.gameObject.GetComponent<CharacterRagdoll>();*/

        IRagdoll ragdoll = collision.gameObject.GetComponent<IRagdoll>();

        if (ragdoll != null)
        {
            GrabbedObject grabbedObject = ragdoll.GetRagdoll().GetComponent<GrabbedObject>();

            if (grabbedObject != null)
            {
                if (grabbedObject.isAttachable) //not entering
                {

                    //Debug.Log("Attachable " + grabbedObject.isAttachable.ToString());
                    if (collision.gameObject.layer != 27)
                    {

                        //look for closest contact points
                        //  loop through all of the contacts points of the enemy
                        // find the point closest to the enemies position

                        //ContactPoint closestPoint;
                        //float closestPointDistance = float.MaxValue;
                        //foreach ( ContactPoint item in collision.contacts)
                        //{
                        //    float pointDistance = Vector3.Distance(item.point, ragdoll.GetRagdoll().hips.position);
                        //    if(pointDistance < closestPointDistance)
                        //    {
                        //        closestPoint = item;
                                
                        //    }
                        //}
                        //AttachObject(ragdoll.GetRagdoll(), closestPoint.point, closestPoint.normal);
                        AttachObject(ragdoll.GetRagdoll(), ragdoll.GetRagdoll().hips.position, collision.contacts[0].normal);
                        grabbedObject.grabbable.isGrabable = false;
                    }
                }
            }
        }
    }
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Debug.Log(hit.controller.gameObject.name + " hit");
    //    CharacterRagdoll ragdoll = hit.controller.gameObject.GetComponent<CharacterRagdoll>();
    //    objects = GetComponent<GrabbedObject>();
    //    // if we collide with a ragdoll
    //    if (ragdoll != null)
    //    {
    //        if (objects.isAttachable) //not entering
    //        {
    //            if (hit.gameObject.layer != 27)
    //            {
    //                AttachObject(ragdoll, hit.point, hit.normal);
    //            }
    //        }

    //    }
    // add a joint to ourselves and connect it to the ragdoll's hips


    private void AttachObject(CharacterRagdoll ragdoll, Vector3 point, Vector3 normal)
    {
        Debug.Log("KILL " + ragdoll.gameObject.name);
        // add a fixed joint
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        //joint.autoConfigureConnectedAnchor = false;
        Rigidbody rb = ragdoll.hips.GetComponent<Rigidbody>();
        //Debug.Log("HIT: X: " + point.x + " Y: " + point.y + " Z: " + point.z);
        BoxCollider col = ragdoll.hips.GetComponent<BoxCollider>();

        ragdoll.hips.position = point;
        //Move hips to the point you want them attached to
        joint.connectedBody = rb;
        //joint.connectedAnchor = new Vector3(ragdoll.hips.position.x, 1, ragdoll.hips.position.z);
        List<Rigidbody> rbs = ragdoll.GetRigidbodies();
        foreach (Rigidbody rigiB in rbs)
        {
            rigiB.isKinematic = true;
        }

        ragdoll.transform.parent = transform;
    }

}