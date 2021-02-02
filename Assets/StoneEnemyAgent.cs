using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneEnemyAgent : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float attackPlayerDistance = 2f;
    [SerializeField]
    private AnimationCurve attackDropOff;
    [SerializeField]
    private float attackTimeInitial;
    private NavMeshAgent enemyAgent;
    private Animator animator;
    private Transform target;
    private Vector3 initialPosition;
    private SphereCastDamageScript sphereDamageScript;
    
    private float attackTimeNow;


    private void Awake()
    {
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        attackTimeNow = attackTimeInitial;
        initialPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
        enemyAgent = GetComponent<NavMeshAgent>();
        animator.SetBool("IsWalking", true);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        sphereDamageScript = GetComponent<SphereCastDamageScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
         * Set navagent destination to player and set speed
         */
        enemyAgent.SetDestination(target.position);
        enemyAgent.speed = walkSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (attackTimeNow > 0)
        {
            attackTimeNow -= Time.deltaTime;
        }
        
        /*
         * If player is in range then attack
         * If not then follow navmesh toward players location
         */
        if(Vector3.Distance(transform.position, target.position) < attackPlayerDistance)
        {
            if(attackTimeNow <= 0)
            {
                Attack();
            }
            
        }
        else
        {
            WalkTowardsPlayer();
        }

    }

    private void WalkTowardsPlayer()
    {
        /*
         * Set walking animation, nav agent speed and target.
         */
        GetComponent<Rigidbody>().freezeRotation = false;
        enemyAgent.speed = walkSpeed;
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsAttacking", false);
        enemyAgent.SetDestination(target.position);
    }

    internal void Stagger()
    {
        animator.SetTrigger("Hit");
    }

    private void Attack()
    {
        /*
         * Stop nav agent moving and start attack animation
         * then damage player if in radius.
         */
        GetComponent<Rigidbody>().freezeRotation = true;
        enemyAgent.speed = 0;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", true);
        enemyAgent.SetDestination(target.position);
        sphereDamageScript.SphereCastDamageArea(1, 1f, attackDropOff , 1, ElementTypes.PHYSICAL);
        attackTimeNow = attackTimeInitial;
    }
}
