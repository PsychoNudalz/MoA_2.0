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
    [SerializeField] private float minCoverDelay = 1f;
    [SerializeField] private float maxCoverDelay = 5f;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;
    private EnemyWaypoint[] waypointstofollow;
    private NavMeshAgent shootingEnemyAgent;
    private Animator shootingEnemyAnimator;
    private float currentAttackTimer;
    private Transform lastTarget; 
    private Transform target;
    private GameObject player;
    private bool isCrouching;
    private bool isShooting;
    private bool IsStaggering;
    private bool IsDead = false;

    [Header("SHooting")]
    [SerializeField] GunDamageScript gunDamageScript;

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
        currentAttackTimer = Mathf.Clamp(Random.Range(minAttackDelay, maxAttackDelay),1.5f,maxAttackDelay);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStaggering && !IsDead && !isCrouching)
        {
            if (currentAttackTimer <= 0 && !isShooting)
            {
                RaycastHit hit;
                Vector3 playerDirection = player.transform.position - transform.position;
                Vector3 shootDirection = (new Vector3(playerDirection.x, 0f, playerDirection.z));
                //Debug.DrawRay(transform.position, shootDirection, Color.red,2f);
                if (Physics.Raycast(transform.position, shootDirection, out hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.DrawRay(transform.position, shootDirection * hit.distance, Color.red,5f);
                        StartCoroutine(Shoot(3.5f));
                    }
                }
                //Debug.DrawRay(transform.position, target * 10f, Color.red);
            }
            if (!isShooting && !isCrouching)
            {
                currentAttackTimer -= Time.deltaTime;
            }
            if (Vector3.Distance(transform.position, target.position) <= 0.2f && !isShooting)
            {
                StartCoroutine(Crouch(Random.Range(minCoverDelay,maxCoverDelay)));
            }
        }
        if (IsDead)
        {
            GameObject.Destroy(this.transform.gameObject, 5f);
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
        }
        else
        {
            lastTarget = waypointstofollow[0].transform;
        }
        EnemyWaypoint nextTarget = null;
        while (nextTarget == null || nextTarget.Equals(lastTarget))
        {
            int point = Random.Range(0, waypointstofollow.Length);
            EnemyWaypoint targetToSet = waypointstofollow[point];
            if (targetToSet.GetIsValid())
            {
                nextTarget = targetToSet;
            }
        }
        lastTarget.GetComponent<EnemyWaypoint>().SetIsValid(true);
        target = nextTarget.transform;
        target.GetComponent<EnemyWaypoint>().SetIsValid(false);
        shootingEnemyAgent.destination = target.position;
                
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
        GetNextWaypoint();
        //shootingEnemyAgent.SetDestination(target.position);
        isCrouching = false;
    }

    IEnumerator Shoot(float delay)
    {
        isShooting = true;
        //yield return new WaitForSeconds(0.5f);
        shootingEnemyAgent.enabled = false;
        transform.LookAt(player.transform);
        shootingEnemyAnimator.SetTrigger("Shoot");
        yield return new WaitForSeconds(delay);
        shootingEnemyAgent.enabled = true;
        shootingEnemyAgent.destination = target.position;
        ResetAttackTimer();
        isShooting = false;
    }

    public void DeathAnimation()
    {
        target.GetComponent<EnemyWaypoint>().SetIsValid(true);
        shootingEnemyAgent.speed = 0f;
        shootingEnemyAgent.velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
        IsDead = true;
        shootingEnemyAnimator.SetBool("IsDead", true);

    }

    

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
        gunDamageScript.Shoot();
    }
}
