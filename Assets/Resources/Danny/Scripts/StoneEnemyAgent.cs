using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class StoneEnemyAgent : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float attackPlayerDistance = 2f;
    [SerializeField] private float chasePlayerDistance = 15f;
    [SerializeField] private AnimationCurve attackDropOff;
    [SerializeField] private float attackTimeInitial;
    [SerializeField] private LayerMask playerMask;
    private NavMeshAgent stoneEnemyAgent;
    private Animator animator;
    private Transform player;
    private SphereCastDamageScript sphereDamageScript;
    private bool IsStaggering;
    private bool IsDead = false;
    private float attackTimeNow;
    private bool inChaseRange;
    private bool inAttackRange;
    private Vector3 currentWaypoint;
    private bool waypointSet;
    private NavMeshPath path;
    private float walkPointRange = 100f;


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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sphereDamageScript = GetComponent<SphereCastDamageScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStaggering && !IsDead)
        {
            inChaseRange = Physics.CheckSphere(transform.position, chasePlayerDistance, playerMask);
            inAttackRange = Physics.CheckSphere(transform.position, attackPlayerDistance, playerMask);

            if (!inAttackRange && !inChaseRange)
            {
                Patrol();
            }
            if (inChaseRange && !inAttackRange)
            {
                try
                {

                    Vector3 walkpoint = player.transform.position;
                    stoneEnemyAgent.CalculatePath(walkpoint, path);
                    if (path.status.Equals(NavMeshPathStatus.PathComplete))
                    {
                        ChasePlayer();
                    }
                    else
                    {
                        waypointSet = false;
                    }
                }catch(NullReferenceException e){

                }
            }
            if (inAttackRange && inChaseRange)
            {
                FaceTarget();
                AttackPlayer();
            }

        }

        if (IsDead)
        {
            GameObject.Destroy(this.gameObject, 5f);
        }

    }

    private void Patrol()
    {
        if (!waypointSet)
        {
            SearchWaypoint();
        }
        if (waypointSet)
        {
            stoneEnemyAgent.SetDestination(currentWaypoint);
        }
        Vector3 distanceToWaypoint = transform.position - currentWaypoint;

        if (distanceToWaypoint.magnitude < 1f)
        {
            waypointSet = false;
        }
    }

    private void SearchWaypoint()
    {
        bool pathValid = false;
        path = new NavMeshPath();
        while (!pathValid)
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            Vector3 castPoint = new Vector3(transform.position.x + randomX, transform.position.y + 10f, transform.position.z + randomZ);
            RaycastHit raycastHit;
            Physics.Raycast(castPoint, -transform.up, out raycastHit);
            if (raycastHit.point != null)
            {
                Vector3 walkpoint = raycastHit.point;
                stoneEnemyAgent.CalculatePath(walkpoint, path);
                if (path.status.Equals(NavMeshPathStatus.PathComplete))
                {
                    currentWaypoint = walkpoint;
                    waypointSet = true;
                    pathValid = true;
                }
            }
        }

    }

    private void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }



    private void ChasePlayer()
    {
        /*
         * Set walking animation, nav agent speed and target.
         */
        GetComponent<Rigidbody>().freezeRotation = false;
        stoneEnemyAgent.speed = walkSpeed;
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsAttacking", false);
        stoneEnemyAgent.SetDestination(player.position);
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

    private void AttackPlayer()
    {
        /*
         * Stop nav agent moving and start attack animation.
         */
        GetComponent<Rigidbody>().freezeRotation = true;
        stoneEnemyAgent.speed = 0;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", true);
        stoneEnemyAgent.SetDestination(player.position);
        //transform.LookAt(player.position);
        attackTimeNow = attackTimeInitial;
    }

    public void DamagePlayer()
    {
        /*
         * Damage player if in range (triggered from attack animation
         */
        sphereDamageScript.SphereCastDamageArea(1, 1f, attackDropOff, 1, ElementTypes.PHYSICAL);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chasePlayerDistance);
    }
}
