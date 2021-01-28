using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneEnemyAgent : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 1f;
    [SerializeField]
    private float attackPlayerDistance = 5;
    private NavMeshAgent enemyAgent;
    private Animator animator;
    private Transform target;
    public Vector3 initialPosition;

    private void Awake()
    {
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        initialPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
        enemyAgent = GetComponent<NavMeshAgent>();
        animator.SetBool("IsWalking", true);
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
    void Update()
    {
        
       
        if(Vector3.Distance(transform.position, target.position) < attackPlayerDistance)
        {
            Attack();
        }
        else
        {
            WalkTowardsPlayer();
        }

    }

    private void WalkTowardsPlayer()
    {
        GetComponent<Rigidbody>().freezeRotation = false;
        enemyAgent.speed = walkSpeed;
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsAttacking", false);
        enemyAgent.SetDestination(target.position);
    }

    private void Attack()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        enemyAgent.speed = 0;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", true);
        enemyAgent.SetDestination(target.position);
        //transform.LookAt(target);
        print("attack");
        
    }
}
