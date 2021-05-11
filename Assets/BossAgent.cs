using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAgent : MonoBehaviour
{
    [SerializeField] int meleeDamage = 100;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float attackPlayerDistance = 6f;
    [SerializeField] float rangeAttackPlayerDistance = 30f;
    [SerializeField] private AnimationCurve attackDropOff;
    [SerializeField] private float attackTimeInitial;
    [SerializeField] AIGunDamageScript shootingScript;
    [SerializeField]Transform firePoint;
    [SerializeField] Transform gun;
    private NavMeshAgent bossAgent;
    private Animator animator;
    private Transform target;
    private SphereCastDamageScript sphereDamageScript;
    private bool IsStaggering;
    private bool IsDead = false;
    private float attackTimeNow;
    public bool isFiring = false;
    private GameObject player;
    private float attackDetectionRange = 50f;
    private bool isShooting;
    private bool deathHandled = false;

    private void Awake()
    {
        ResetEnemy();
    }

    /// <summary>
    /// reset paramaters
    /// </summary>
    private void ResetEnemy()
    {
        gun = shootingScript.transform;
        player = GameObject.FindGameObjectWithTag("Player");
        attackTimeNow = attackTimeInitial;
        animator = GetComponentInChildren<Animator>();
        bossAgent = GetComponent<NavMeshAgent>();
        animator.SetBool("IsWalking", true);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        sphereDamageScript = GetComponent<SphereCastDamageScript>();
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        /*
         * Set navagent destination to player and set speed
         */
        bossAgent.SetDestination(target.position);
        bossAgent.speed = walkSpeed;
    }

    /// <summary>
    /// check when boss can attack
    /// decide when the boss chases, hits or shoots
    /// check if boss is dead
    /// </summary>
    void Update()
    {
        if (!IsStaggering && !IsDead)
        {
            if (attackTimeNow > 0)
            {
                attackTimeNow -= Time.deltaTime;
            }

            /*
             * If player is in range then attack
             * If not then follow navmesh toward players location
             */
            if (!isShooting && Vector3.Distance(transform.position, target.position) < attackPlayerDistance)
            {
                bossAgent.enabled = true;
                //animator.SetBool("IsFiring", false);
                shootingScript.Fire(false);
                if (attackTimeNow <= 0)
                {
                    Attack();
                }

            }
            else if (Vector3.Distance(transform.position, target.position) > attackPlayerDistance && Vector3.Distance(transform.position, target.position) < rangeAttackPlayerDistance)
            {
                if (!isShooting)
                {
                    RaycastHit hit;
                    Vector3 playerDirection = player.transform.position - firePoint.transform.position;
                    Debug.DrawRay(firePoint.position, playerDirection, Color.red, 3f);
                    if (Physics.Raycast(firePoint.transform.position, playerDirection, out hit, attackDetectionRange))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            bossAgent.enabled = false;
                            animator.SetTrigger("Shoot");
                        }
                    }
                }
            }
            else if (!isShooting && Vector3.Distance(transform.position, target.position) > rangeAttackPlayerDistance)
            {
                bossAgent.enabled = true;
                //animator.SetBool("IsFiring", false);
                animator.SetBool("IsWalking", true);
                shootingScript.Fire(false);
                WalkTowardsPlayer();
            }
            /*
            else {
                bossAgent.enabled = true;
                //animator.SetBool("IsFiring", false);
                animator.SetBool("IsWalking", true);
            }*/

        }
        if (IsDead && !deathHandled)
        {
            transform.parent.GetComponent<EnemySpawner>().RemoveFromSpawnedEnemies(this.gameObject);
            GameObject.Destroy(this.gameObject, 10f);
            deathHandled = true;
        }

    }

    /// <summary>
    /// stop the boss character, start the animation, call the fire method in the shooting script
    /// </summary>
    void bossFire() {

        StartCoroutine(Shoot(0.1f));
        //bossAgent.enabled = false;
        //FaceTarget();
        //animator.SetBool("IsFiring", true);
        //animator.SetBool("IsWalking", false);
        //shootingScript.transform.LookAt(player.transform);
        //shootingScript.Fire(true);
    }
     
    /// <summary>
    /// Walk twords player object
    /// </summary>
    private void WalkTowardsPlayer()
    {
        /*
         * Set walking animation, nav agent speed and target.
         */
            GetComponent<Rigidbody>().freezeRotation = true;
            bossAgent.speed = walkSpeed;
            animator.SetBool("IsAttacking", false);
            bossAgent.SetDestination(target.position);
    }

    /// <summary>
    /// start stagger
    /// </summary>
    public void Stagger()
    {
        StartCoroutine(StaggerDelay());
    }

    /// <summary>
    /// 
    /// </summary>
    public void DeathAnimation()
    {
        IsDead = true;
        bossAgent.speed = 0f;
        bossAgent.velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
        animator.SetBool("IsDead", true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator StaggerDelay()
    {
        /*
         * Stop enemy, trigger stagger animation, wait 0.5 seconds then restart enemy
         */
        IsStaggering = true;
        bossAgent.speed = 0f;
        bossAgent.velocity = Vector3.zero;
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        IsStaggering = false;

    }

    /// <summary>
    /// Melee attack, set animation and change attack time
    /// </summary>
    private void Attack()
    {
        /*
         * Stop nav agent moving and start attack animation.
         */
        GetComponent<Rigidbody>().freezeRotation = true;
        bossAgent.speed = 0;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", true);
        bossAgent.SetDestination(target.position);
        //transform.LookAt(target.position);
        attackTimeNow = attackTimeInitial;
    }

    /// <summary>
    /// Deal damage to player health
    /// </summary>
    public void DamagePlayer()
    {
        /*
         * Damage player if in range (triggered from attack animation
         */
        sphereDamageScript.SphereCastDamageArea(meleeDamage, 3f, attackDropOff, 1, ElementTypes.PHYSICAL);
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 40f);
        shootingScript.transform.LookAt(player.transform);
    }

    IEnumerator Shoot(float delay)
    {
        isShooting = true;
        animator.SetBool("IsWalking", false);
        FaceTarget();
        animator.SetTrigger("Shoot");
        shootingScript.Fire(true);
        yield return new WaitForSeconds(delay);
        //bossAgent.enabled = true;
        //animator.SetBool("IsWalking", true);
        isShooting = false;
        FaceTarget();
    }
}
