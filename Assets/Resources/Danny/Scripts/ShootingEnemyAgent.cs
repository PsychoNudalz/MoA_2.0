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
    [SerializeField] private Transform firePoint;
    private EnemyWaypoint[] waypointstofollow;
    private NavMeshAgent shootingEnemyAgent;
    private Animator shootingEnemyAnimator;
    private float attackDelay;
    private float currentAttackTimer;
    private Transform lastTarget; 
    private Transform target;
    private int currentWaypoint;
    private GameObject player;
    private bool isCrouching;
    private bool isShooting;


    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        shootingEnemyAnimator = GetComponent<Animator>();
        shootingEnemyAgent = GetComponent<NavMeshAgent>();
        GetWaypoints();
        target = waypointstofollow[currentWaypoint].transform;
        GetNextWaypoint();
        shootingEnemyAgent.speed = walkSpeed;
        shootingEnemyAnimator.SetBool("IsWalking", true);
        isCrouching = false;
        ResetAttackTimer();
    }

    private void ResetAttackTimer()
    {
        attackDelay = Random.Range(minAttackDelay, maxAttackDelay);
        currentAttackTimer = attackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAttackTimer <= 0 && !(Vector3.Distance(transform.position, target.position) <= 3f) && !(Vector3.Distance(transform.position, lastTarget.position) <= 3f))
        {
            StartCoroutine(Shoot(3f));
        }
        if (!isShooting && !isCrouching)
        {
            currentAttackTimer -= Time.deltaTime;
        }
        if(Vector3.Distance(transform.position, target.position) <= 0.5f && !isCrouching && !isShooting)
        {
            StartCoroutine(Crouch(3.5f));
            GetNextWaypoint();
        }

        
    }

    public void GetWaypoints()
    {
        waypointstofollow = transform.parent.GetComponentInChildren<EnemyWaypoints>().GetWaypointsToFollow();
    }

    //Get next waypoint and reset to first if last waypoint reached.
    private void GetNextWaypoint()
    {
        lastTarget = target;
        EnemyWaypoint nextTarget = null;
        while (nextTarget.Equals(null))
        {
            EnemyWaypoint targetToSet = waypointstofollow[Random.Range(0, waypointstofollow.Length)];
            if (targetToSet.IsValid())
            {
                nextTarget = targetToSet;
            }
        }
        lastTarget.GetComponent<EnemyWaypoint>().SetIsValid(true);
        target = nextTarget.transform;
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
        shootingEnemyAnimator.SetTrigger("Shoot");
        shootingEnemyAgent.enabled = false;
        transform.LookAt(player.transform);
        yield return new WaitForSeconds(delay);
        shootingEnemyAgent.enabled = true;
        shootingEnemyAgent.destination = target.position;
        isShooting = false;
    }

    public void Fire()
    {
        Instantiate((GameObject)Resources.Load("Danny/Prefabs/Fireball"),firePoint.position, Quaternion.identity ,this.transform);
    }
}
