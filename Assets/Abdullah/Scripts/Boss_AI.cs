﻿using UnityEngine;
using UnityEngine.AI;

public enum AIMode
{
    Idle,
    Hunt,
    Waiting,
    Walking,
    Melee,
    Shoot
}

public class Boss_AI : MonoBehaviour
{
    [Header("Other Component")]
    public GameObject player;
    [Header("Self Component")]
    public NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Transform lineOfSightTransform;
    [SerializeField] SphereCastDamageScript meleeDamageScript;
    [SerializeField] AIGunDamageScript shootingDamageScript;
    [SerializeField] AIGunDamageScript missileDamageScript;
    [Header("Variables")]
    [SerializeField] float maxDetection = 60f;
    [SerializeField] LayerMask layerMask;
    [Header("Shoot")]
    [SerializeField] float shootAttackRange = 15f;
    [SerializeField] float shootAttackCoolDown = 3f;
    [Header("Melee")]
    [SerializeField] float meleeAttackRange = 3f;
    [SerializeField] float meleeAttackCoolDown = 1f;
    [SerializeField] float meleeDamage;
    [SerializeField] AnimationCurve meleeCurve;

    [Header("Status")]
    [SerializeField] AIMode aIMode = AIMode.Idle;
    [SerializeField] float nextDecisionTime;
    [SerializeField] float decisionTime = 1f;

    public bool IsDead { get; private set; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(transform.position);
        agent.enabled = false;
    }
    private void FixedUpdate()
    {
        //MoveTowordsPlayer();
        if (Time.time > nextDecisionTime)
        {
            AIBehaviour();
            nextDecisionTime = nextDecisionTime + decisionTime;
        }
    }

    void AIBehaviour()
    {
        switch (aIMode)
        {
            case AIMode.Idle:
                IdleBehaviour();
                break;
            case AIMode.Waiting:
                AnimatorNext();
                break;
            case AIMode.Walking:
                if (CheckLineOfSight(shootAttackRange))
                {
                    StopMoving();
                }
                else
                {
                MoveTowordsPlayer();

                }
                break;
            case AIMode.Melee:
                break;
            case AIMode.Shoot:
                break;
            case AIMode.Hunt:
                break;
            default:
                break;
        }
    }

    void SetAIMode(AIMode mode)
    {
        Debug.Log(name + " change from: " + aIMode + " --> " + mode);
        aIMode = mode;
    }

    void SetNextDecisionTime(float t)
    {
        nextDecisionTime = Time.time + t;
    }

    public void AnimatorNext()
    {
        animator.SetTrigger("Next");
    }



    void MeleeAttack()
    {
        SetAIMode(AIMode.Melee);
        animator.SetTrigger("Attack_Melee");
        SetNextDecisionTime(meleeAttackCoolDown);
        SetAIMode(AIMode.Waiting);

    }

    void RangedAttack()
    {
        SetAIMode(AIMode.Shoot);
        animator.SetTrigger("Attack_Shooting");
        SetNextDecisionTime(shootAttackCoolDown);
        SetAIMode(AIMode.Waiting);

    }



    void MissileAttack()
    {

    }

    public void LookForPlayer()
    {
        SetAIMode(AIMode.Hunt);
        StopAttack();
        if (CheckLineOfSight(shootAttackRange))
        {
            animator.SetTrigger("Next");

        }
        else
        {
            SetAIMode(AIMode.Idle);
            animator.SetTrigger("ToIdle");
        }
    }

    public void PickAttack()
    {
        agent.enabled = false;
        FaceTarget();
        
        if (CheckLineOfSight(meleeAttackRange))
        {
            MeleeAttack();

        }
        else if (CheckLineOfSight(shootAttackRange))
        {
            RangedAttack();
        }
        else
        {
            RangedAttack();
        }
    }

    bool CheckLineOfSight(float range)
    {
        RaycastHit hit;
        Vector3 dir = player.transform.position + new Vector3(0f, 0.3f, 0f) - lineOfSightTransform.position;
        Debug.DrawRay(lineOfSightTransform.position, dir * range, Color.red);
        if (Physics.Raycast(lineOfSightTransform.position, dir, out hit, range,layerMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    void LookAtPlayer()
    {

    }

    void AttackCooldown()
    {

    }

    public void IdleBehaviour()
    {
        SetAIMode(AIMode.Idle);
        agent.enabled = false;

        if (CheckLineOfSight(maxDetection))
        {
            MoveTowordsPlayer();

        }
        else
        {
        }
    }

    public void MoveTowordsPlayer()
    {
        SetAIMode(AIMode.Walking);
        Vector3 playerLocation = player.transform.position;
        agent.enabled = true;
        agent.SetDestination(playerLocation);

        animator.SetTrigger("Next");
        animator.SetBool("IsWalking", true);

    }

    public void StopMoving()
    {
        animator.SetBool("IsWalking", false);
    }

    void MoveAwayFromPlayer()
    {

    }

    void CheckLineOfSight()
    {

    }

    void Death()
    {

    }

    public void DealMeleeAttack()
    {
        meleeDamageScript.SphereCastDamageArea(meleeDamage, meleeAttackRange, meleeCurve, 1, ElementTypes.PHYSICAL, true);
    }

    public void DealRangeAttack()
    {
        shootingDamageScript.Fire(true);
    }

    public void DealMissleAttack()
    {
        missileDamageScript.Fire(true);

    }

    public void StopAttack()
    {
        shootingDamageScript.Fire(false);
        missileDamageScript.Fire(false);

    }

    private void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.95f);
    }

    public void DeathAnimation()
    {
        IsDead = true;
        agent.speed = 0f;
        agent.velocity = Vector3.zero;
        animator.SetTrigger("Death");
    }

}