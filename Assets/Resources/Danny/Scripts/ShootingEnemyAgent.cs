using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ShootingEnemyAgent : MonoBehaviour
{

    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float minAttackDelay = 1f;
    [SerializeField] private float maxAttackDelay = 5f;
    [SerializeField] private float attackDetectionRange = 50f;
    [SerializeField] private float minCoverDelay = 1f;
    [SerializeField] private float maxCoverDelay = 5f;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;
    private NavMeshAgent shootingEnemyAgent;
    private Animator shootingEnemyAnimator;
    private float currentAttackTimer;
    private GameObject player;
    private bool isCrouching;
    private bool isShooting;
    private bool IsStaggering;
    private bool IsDead = false;
    private Vector3 currentWaypoint;
    private bool waypointSet;
    private NavMeshPath path;
    private float walkPointRange = 100f;
    private AIGunDamageScript gun;

    [Header("Shooting")]
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
        shootingEnemyAgent.speed = walkSpeed;
        shootingEnemyAnimator.SetBool("IsWalking", true);
        isCrouching = false;
        gun = GetComponentInChildren<AIGunDamageScript>();
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
        if (!IsStaggering && !IsDead)
        {
            
            if (currentAttackTimer <= 0 && !isShooting)
            {
                RaycastHit hit;
                Vector3 playerDirection = player.transform.position - firePoint.transform.position;
                Debug.DrawRay(firePoint.position, playerDirection, Color.red,2f);
                if (Physics.Raycast(firePoint.transform.position, playerDirection, out hit, attackDetectionRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        StartCoroutine(Shoot(1.6f));
                    }
                }
            }
            if (!isShooting && !isCrouching)
            {
                currentAttackTimer -= Time.deltaTime;
                Patrol();
            }
            
        }
        if (IsDead)
        {
            
            GameObject.Destroy(this.transform.gameObject, 5f);
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 40f);
        gun.transform.LookAt(player.transform);
    }

    private void Patrol()
    {
        if (!waypointSet)
        {
            SearchWaypoint();
        }
        if (waypointSet)
        {
            shootingEnemyAgent.SetDestination(currentWaypoint);
        }
        Vector3 distanceToWaypoint = transform.position - currentWaypoint;

        if (distanceToWaypoint.magnitude < 1f)
        {
            waypointSet = false;
            StartCoroutine(Crouch(Random.Range(minCoverDelay, maxCoverDelay)));
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
                shootingEnemyAgent.CalculatePath(walkpoint, path);
                if (path.status.Equals(NavMeshPathStatus.PathComplete))
                {
                    currentWaypoint = walkpoint;
                    waypointSet = true;
                    pathValid = true;
                }
            }
        }
    }

    IEnumerator Crouch(float delay)
    {
        isCrouching = true;
        shootingEnemyAnimator.SetBool("IsCrouching", true);
        shootingEnemyAgent.enabled = false;
        FaceTarget();
        yield return new WaitForSeconds(delay);
        shootingEnemyAgent.enabled = true;
        shootingEnemyAnimator.SetBool("IsCrouching", false);
        isCrouching = false;
    }

    IEnumerator Shoot(float delay)
    {
        isShooting = true;
        shootingEnemyAgent.enabled = false;
        shootingEnemyAnimator.SetTrigger("Shoot");
        FaceTarget();
        yield return new WaitForSeconds(delay);
        shootingEnemyAgent.enabled = true;
        ResetAttackTimer();
        isShooting = false;
        EndFire();
        
    }

    public void DeathAnimation()
    {
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
        gunDamageScript.Fire(true);
    }

    public void EndFire()
    {
        gunDamageScript.Fire(false);

    }
}
