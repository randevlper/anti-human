using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //IRagdoll ragdoll;
    public float range = 10;
    //[HideInInspector]

    [SerializeField]
    private float downForce = 0;

    public float gravity = 9.8f;
    private float vSpeed = 0;

    public float speed = 2; //Recommended speed 
    private Vector3 moveDir = Vector3.zero;
    private bool stopMovement = false;
    CharacterController cc;


    private Animator enemyAnim;
    public Transform player;
    public bool attackRangeBool = false;


    // Update is called once per frame
    private void Awake()
    {
        if (speed >= range) // Keeps from going over the range because going over makes the enemy jitter
        {
            speed = range;
        }
    }
    //IF NOT RAGDOLL
    //ANIM ACTIVE
    //CHAR CONTROL ACTIVE
    private void Start()
    {               
        cc = GetComponent<CharacterController>();   // gets the character controller component     
        enemyAnim = GetComponent<Animator>();       //get animator
    }
    void Update()
    {
        vSpeed -= gravity * Time.deltaTime;
        if (cc.isGrounded)
        {
            vSpeed = 0;
            //Debug.Log("Enemy: " + cc.isGrounded);
        }
        if (player != null)
        {
            if (Vector3.Distance(player.position, transform.position) < range && stopMovement == false) //finds the distance between the player 
            {
                enemyAnim.SetTrigger("ToWalk");
                moveDir = new Vector3(cc.transform.position.x - player.position.x, moveDir.y, cc.transform.position.z - player.position.z).normalized * speed;
            }
        }
        else
        {
            enemyAnim.SetTrigger("ToIdle"); //uses the enemy idle animation if the player is not in distance
        }
        moveDir.y = vSpeed;
        cc.Move(moveDir * Time.deltaTime); //moves the character controller by MoveDir multiplying time.deltaTime
    }
    //testing
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }    
}
