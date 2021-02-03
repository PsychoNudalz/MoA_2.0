using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ShootingEnemyAgent : MonoBehaviour
{

    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private Transform waypointsToFollow;
    [SerializeField] private float minAttackDelay = 1f;
    [SerializeField] private float maxAttackDelay = 5f;
    private Transform[] waypoints;
    private NavMeshAgent shootingEnemyAgent;
    private Animator shootingEnemyAnimator;
    private float attackDelay;
    private float currentAttackTimer;
    private Transform target;
    private int currentWaypoint;
    private GameObject player;
    private bool isCrouching;
    private bool isShooting;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shootingEnemyAnimator = GetComponent<Animator>();
        shootingEnemyAgent = GetComponent<NavMeshAgent>();
        SetWaypoints();
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
        if(currentAttackTimer <= 0)
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

    public void SetWaypoints()
    {
        try
        {
            waypoints = new Transform[waypointsToFollow.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = waypointsToFollow.GetChild(i);
            }
            target = waypoints[0];
        }
        catch (System.Exception e)
        {

        }
        if (target != null)
            shootingEnemyAgent.destination = target.position;
    }

    //Get next waypoint and reset to first if last waypoint reached.
    private void GetNextWaypoint()
    {
        if (currentWaypoint >= waypoints.Length - 1)
        {
            currentWaypoint = 0;
        }
        else
        {
            currentWaypoint++;
        }
        target = waypoints[currentWaypoint];
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
        print("fired");
    }
}
