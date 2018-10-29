using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private CharacterRagdoll ragdoll;
    private Animator enemyAnim;
    private CharacterController cc;
    private EnemyMovement movement;
    public float attackRange;
    // Use this for initialization
    void Start()
    {
        ragdoll = GetComponent<CharacterRagdoll>(); //gets the ragdoll script
        cc = GetComponent<CharacterController>(); // gets the character controller component
        enemyAnim = GetComponent<Animator>(); //gets the animator       
        movement = GetComponent<EnemyMovement>(); //gets the enemy movement script
        //enemyControl = GetComponent<CharacterController>(); // gets the ....
        ragdoll.OnRagdoll += OnRagdoll; //connects to a delegate
    } 
    //private void Update()
    //{
    //    if(cc.isGrounded)
    //    {
    //        ragdoll.Ragdoll(true, Vector3.zero);
    //    }
    //}
    void OnRagdoll(bool isRagdolled) //enables and disables the ragdoll
    {
        enemyAnim.enabled = !isRagdolled;
        cc.enabled = !isRagdolled;
        movement.enabled = !isRagdolled;
        enabled = !enabled;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((cc.collisionFlags & CollisionFlags.Sides) != 0)//checks collision with anything that is not the player
        {
            if(!ragdoll.IsRagdolled)
            {
                ragdoll.Ragdoll(true, Vector3.up);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterController character = other.GetComponent<CharacterController>(); //looks to see if the collision has a character controller
        if (character != null)//checks to see if the character controller does not = null
        {
            ragdoll.Ragdoll(true, Vector3.zero);//if it does not = null then ragdoll
             //sets the enemy movement script false
        }
    }
    //testing
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}