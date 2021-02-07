using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ShootingEnemyAgent : MonoBehaviour
{

    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float minAttackDelay = 1f;
    [SerializeField] private float maxAttackDelay = 5f;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;

    private NavMeshAgent shootingEnemyAgent;
    private Animator shootingEnemyAnimator;
    private float attackDelay;
    private float currentAttackTimer;
    private Transform lastTarget; 
    private Transform target;
    private GameObject player;
    private bool isCrouching;
    private bool isShooting;
    private bool IsStaggering;
    private bool IsDead = false;
    Vector3 aimTarget;

    // Start is called before the first frame update
    void Start()
    {
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shootingEnemyAnimator = GetComponent<Animator>();
        shootingEnemyAgent = GetComponent<NavMeshAgent>();
        GetWaypoints();
        //target = waypointstofollow[0].transform;
        GetNextWaypoint();
        shootingEnemyAgent.speed = walkSpeed;
        shootingEnemyAnimator.SetBool("IsWalking", true);
        isCrouching = false;
        ResetAttackTimer();
    }

    /*
     * Set next attack timer to random value between min and max values set
     */
    private void ResetAttackTimer()
    {
        currentAttackTimer = Random.Range(minAttackDelay, maxAttackDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStaggering && !IsDead)
        {
            if (currentAttackTimer <= 0)
            {
                RaycastHit hit;
                Vector3 playerDirection = player.transform.position - transform.position;
                Vector3 shootDirection = (new Vector3(playerDirection.x, 0f, playerDirection.z));
                //Debug.DrawRay(transform.position, shootDirection, Color.red,2f);
                aimTarget = transform.position + shootDirection * 5f;
                if (Physics.Raycast(transform.position, shootDirection, out hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        //Debug.DrawRay(transform.position, shootDirection * hit.distance, Color.red);
                        StartCoroutine(Shoot(3f));
                    }
                }
                //Debug.DrawRay(transform.position, target * 10f, Color.red);
            }
            if (!isShooting && !isCrouching)
            {
                currentAttackTimer -= Time.deltaTime;
            }
            if (Vector3.Distance(transform.position, target.position) <= 0.5f && !isCrouching && !isShooting)
            {
                GetNextWaypoint();
                StartCoroutine(Crouch(3.5f));
            }
        }
        if (IsDead)
        {
            GameObject.Destroy(this.transform.gameObject, 10f);
        }
    }

    public void GetWaypoints()
    {
        waypointstofollow = transform.parent.parent.GetComponentInChildren<EnemyWaypoints>().GetWaypointsToFollow();
    }

    //Get next waypoint and reset to first if last waypoint reached.
    private void GetNextWaypoint()
    {
        if(target != null)
        {
            lastTarget = target;
            lastTarget.GetComponent<EnemyWaypoint>().SetIsValid(true);
        }
        EnemyWaypoint nextTarget = null;
        while (nextTarget == null)
        {
            int point = Random.Range(0, waypointstofollow.Length);
            EnemyWaypoint targetToSet = waypointstofollow[point];
            if (targetToSet.GetIsValid())
            {
                nextTarget = targetToSet;
            }
        }
        target = nextTarget.transform;
        target.GetComponent<EnemyWaypoint>().SetIsValid(false);
        if (shootingEnemyAgent.isActiveAndEnabled)
        {
            shootingEnemyAgent.destination = target.position;
        }
     }

    IEnumerator Crouch(float delay)
    {
        isCrouching = true;
        shootingEnemyAnimator.SetBool("IsCrouching", true);
        shootingEnemyAgent.enabled = false;
        transform.LookAt(player.transform);
        yield return new WaitForSeconds(delay);
        shootingEnemyAnimator.SetBool("IsCrouching", false);
        shootingEnemyAgent.enabled = true;
        shootingEnemyAgent.destination = target.position;
        isCrouching = false;
    }

    IEnumerator Shoot(float delay)
    {
        isShooting = true;
        ResetAttackTimer();
        yield return new WaitForSeconds(0.3f);
        shootingEnemyAnimator.SetTrigger("Shoot");
        shootingEnemyAgent.enabled = false;
        transform.LookAt(player.transform);
        yield return new WaitForSeconds(delay);
        shootingEnemyAgent.enabled = true;
        shootingEnemyAgent.destination = target.position;
        isShooting = false;
    }

    public void DeathAnimation()
    {
        shootingEnemyAgent.speed = 0f;
        shootingEnemyAgent.velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
        IsDead = true;
        shootingEnemyAnimator.SetBool("IsDead", true);

    }

    private EnemyWaypoint[] waypointstofollow;

    public void Stagger()
    {
        StartCoroutine(StaggerDelay());
    }

    IEnumerator StaggerDelay()
    {
        IsStaggering = true;
        shootingEnemyAgent.enabled = false;
        shootingEnemyAnimator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        shootingEnemyAgent.enabled = true;
        IsStaggering = false;

    }

    public void Fire()
    {
        GameObject newFireBall = Instantiate(fireballPrefab,firePoint.position, Quaternion.identity ,this.transform);
        newFireBall.GetComponent<Fireball>().SetTarget(aimTarget);
    }
}
