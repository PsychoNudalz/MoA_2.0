using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAgent : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float attackPlayerDistance = 6f;
    [SerializeField] private AnimationCurve attackDropOff;
    [SerializeField] private float attackTimeInitial;
    private NavMeshAgent bossAgent;
    private Animator animator;
    private Transform target;
    private SphereCastDamageScript sphereDamageScript;
    private bool IsStaggering;
    private bool IsDead = false;
    private float attackTimeNow;
    [SerializeField]AIGunDamageScript shootingScript;
    public bool isFiring = false;


    private void Awake()
    {
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        
        attackTimeNow = attackTimeInitial;
        animator = GetComponentInChildren<Animator>();
        bossAgent = GetComponent<NavMeshAgent>();
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
        bossAgent.SetDestination(target.position);
        bossAgent.speed = walkSpeed;
    }

    // Update is called once per frame
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
            if (Vector3.Distance(transform.position, target.position) < attackPlayerDistance)
            {
                bossAgent.enabled = true;
                animator.SetBool("IsFiring", false);
                shootingScript.Fire(false);
                if (attackTimeNow <= 0)
                {
                    Attack();
                }

            }
            else if (Vector3.Distance(transform.position, target.position) > 40 && Vector3.Distance(transform.position, target.position) < 50)
            {
                bossFire();
            }
            else if (Vector3.Distance(transform.position, target.position) > attackPlayerDistance)
            {
                bossAgent.enabled = true;
                animator.SetBool("IsFiring", false);
                animator.SetBool("IsWalking", true);
                shootingScript.Fire(false);
                WalkTowardsPlayer();
            }
            else {
                bossAgent.enabled = true;
                animator.SetBool("IsFiring", false);
                animator.SetBool("IsWalking", true);
            }

        }
        if (IsDead)
        {
            GameObject.Destroy(this.gameObject, 10f);
        }

    }

    void bossFire() {
        bossAgent.enabled = false;
        animator.SetBool("IsFiring", true);
        animator.SetBool("IsWalking", false);
        shootingScript.Fire(true);
    }
    private void WalkTowardsPlayer()
    {
        /*
         * Set walking animation, nav agent speed and target.
         */
        
            GetComponent<Rigidbody>().freezeRotation = false;
            bossAgent.speed = walkSpeed;
            animator.SetBool("IsAttacking", false);
            bossAgent.SetDestination(target.position);
        
    }

    public void Stagger()
    {
        StartCoroutine(StaggerDelay());
    }

    public void DeathAnimation()
    {
        IsDead = true;
        bossAgent.speed = 0f;
        bossAgent.velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
        animator.SetBool("IsDead", true);
    }


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
        transform.LookAt(target.position);
        attackTimeNow = attackTimeInitial;
    }

    public void DamagePlayer()
    {
        /*
         * Damage player if in range (triggered from attack animation
         */
        sphereDamageScript.SphereCastDamageArea(1, 1f, attackDropOff, 1, ElementTypes.PHYSICAL);
    }
}
