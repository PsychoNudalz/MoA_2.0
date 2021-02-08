using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneEnemyAgent : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float attackPlayerDistance = 2f;
    [SerializeField] private AnimationCurve attackDropOff;
    [SerializeField] private float attackTimeInitial;
    private NavMeshAgent stoneEnemyAgent;
    private Animator animator;
    private Transform target;
    private SphereCastDamageScript sphereDamageScript;
    private bool IsStaggering;
    private bool IsDead = false;
    private float attackTimeNow;


    private void Awake()
    {
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        attackTimeNow = attackTimeInitial;
        animator = GetComponentInChildren<Animator>();
        stoneEnemyAgent = GetComponent<NavMeshAgent>();
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
        stoneEnemyAgent.SetDestination(target.position);
        stoneEnemyAgent.speed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsStaggering && !IsDead)
        {
            if (attackTimeNow > 0)
            {
                attackTimeNow -= Time.deltaTime;
            }

            /*
             * If player is in range then attack
             * If not then follow navmesh toward players location
             */
            if (Vector3.Distance(transform.position, target.position) < attackPlayerDistance)
            {
                if (attackTimeNow <= 0)
                {
                    Attack();
                }

            }
            else
            {
                WalkTowardsPlayer();
            }
        }
        if (IsDead)
        {
            GameObject.Destroy(this.gameObject, 10f);
        }

    }

    private void WalkTowardsPlayer()
    {
        /*
         * Set walking animation, nav agent speed and target.
         */
        GetComponent<Rigidbody>().freezeRotation = false;
        stoneEnemyAgent.speed = walkSpeed;
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsAttacking", false);
        stoneEnemyAgent.SetDestination(target.position);
    }

    public void Stagger()
    {
        StartCoroutine(StaggerDelay());
    }

    public void DeathAnimation()
    {
        IsDead = true;
        stoneEnemyAgent.speed = 0f;
        stoneEnemyAgent.velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
        animator.SetBool("IsDead", true);
    }

    
    IEnumerator StaggerDelay()
    {
        /*
         * Stop enemy, trigger stagger animation, wait 0.5 seconds then restart enemy
         */
        IsStaggering = true;
        stoneEnemyAgent.speed = 0f;
        stoneEnemyAgent.velocity = Vector3.zero;
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        IsStaggering = false;

    }

    private void Attack()
    {
        /*
         * Stop nav agent moving and start attack animation.
         */
        GetComponent<Rigidbody>().freezeRotation = true;
        stoneEnemyAgent.speed = 0;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", true);
        stoneEnemyAgent.SetDestination(target.position);
        transform.LookAt(target.position);
        attackTimeNow = attackTimeInitial;
    }

    public void DamagePlayer()
    {
        /*
         * Damage player if in range (triggered from attack animation
         */
        sphereDamageScript.SphereCastDamageArea(1, 1f, attackDropOff , 1, ElementTypes.PHYSICAL);
    }
}
