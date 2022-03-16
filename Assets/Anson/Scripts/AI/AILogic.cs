using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/// <summary>
/// AI State
/// </summary>
public enum AIAttribute
{
    Stationary,
    Aggressive,
    Defensive,
    Stealthy,
    OrientateToTarget
}

/// <summary>
/// AI State
/// </summary>
public enum AIState
{
    Idle,
    Move,
    Attack,
    Stagger,
    Dead
}

/// <summary>
/// Main Enemy AI super class from pass project
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class AILogic : MonoBehaviour
{
    [Header("AI State")]
    [SerializeField]
    protected AIState currentState;

    [SerializeField]
    protected AIState previousState;


    [Space(10f)]
    [Header("AI Attributes")]
    [Tooltip("The lower the list, the higher the priority")]
    [SerializeField]
    protected List<AIAttribute> attributesStack;

    [Header("Defensive")]
    [SerializeField]
    protected float defensive_Distance = 5f;

    [SerializeField]
    protected float defensive_Space = 3f;

    [SerializeField]
    protected Vector2 defensive_TangentRange = new Vector2(2f, 4f);

    [Space(10f)]
    [Header("AI Decision Times")]
    [SerializeField]
    float lastThinkTime;

    [SerializeField]
    float thinkRate = 0.5f;

    [Header("AI Hostiles")]
    [SerializeField]
    private LayerMask hostileLayerMaks;

    [SerializeField]
    private List<string> hostileTags;

    [Header("AI Friend")]
    [SerializeField]
    private LayerMask friendlyLayerMaks;

    [SerializeField]
    private List<string> friendlyTags;

    [Header("AI Detection")]
    [SerializeField]
    protected float detectionRange = 15f;

    [SerializeField]
    protected Vector3 playerPos;

    [SerializeField]
    protected LayerMask LOSLayer;

    [Header("AI Move")]
    [SerializeField]
    protected Vector3 movePos;

    [SerializeField]
    protected float moveStopRange = 0.2f;

    [SerializeField]
    protected Vector2 MoveWaitTime = new Vector2(1f, 2f);

    [SerializeField]
    protected float MoveWaitTime_Now = 0;

    [SerializeField]
    protected float turnSpeed = 10f;

    [Header("AI Attack")]
    [SerializeField]
    public List<AttackSet> attackSets;

    [SerializeField]
    protected AttackSet lastAttack;

    [SerializeField]
    protected Transform attackTarget;

    [SerializeField]
    [Range(0f, 90f)]
    private float visionConeDegree = 45f;

    [SerializeField]
    protected float attackRange = 5f;

    [SerializeField]
    float attackDuration = 1;

    [SerializeField]
    float attackCooldown = 2;

    [Header("Debug")]
    [SerializeField]
    private bool DrawDebug;

    [Header("Other Components")]
    [SerializeField]
    GameObject playerGO;

    [SerializeField]
    private Transform bodyModel;

    [SerializeField]
    protected NavMeshAgent navMeshAgent;

    // [SerializeField]
    // PatrolManager patrolManager;

    [SerializeField]
    protected PatrolZone currentPatrolZone;


    [SerializeField]
    protected Transform head;


    private void OnDrawGizmosSelected()
    {
        if (DrawDebug)
        {
            Gizmos.DrawSphere(movePos, 0.2f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetNewPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastThinkTime + thinkRate <= Time.time)
        {
            AIThink();
            lastThinkTime = Time.time;
        }

        AIBehaviour();
    }

    public void SetActive(bool b)
    {
        this.enabled = b;
        navMeshAgent.enabled = b;
        if (!b)
        {
            ChangeState(AIState.Idle);
        }
    }


    public virtual void ChangeState(AIState newState, AttackSet attackSet = null)
    {
        previousState = currentState;
        currentState = newState;

        Debug.Log($"AI: {name}: Change State: {previousState} --> {currentState}");
        switch (previousState)
        {
            case AIState.Idle:
                EndState_Idle();
                break;
            case AIState.Move:
                EndState_Move();
                break;
            case AIState.Attack:
                EndState_Attack();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (newState)
        {
            case AIState.Idle:
                ChangeState_Idle();
                break;
            case AIState.Move:
                ChangeState_Move();
                break;
            case AIState.Attack:
                ChangeState_Attack(attackSet);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    /// <summary>
    /// Process information on the tick rate
    /// </summary>
    protected abstract void AIThink();
    //Debug.Log("I think");
    //
    // switch (currentState)
    // {
    //     case AIState.Idle:
    //         if (GetDistanceToPlayer() <= attackRange)
    //         {
    //             ChangeState(AIState.Attack);
    //         }
    //         else if (GetDistanceToPlayer() <= detectionRange && LineOfSight())
    //         {
    //             ChangeState(AIState.Move);
    //         }
    //         else
    //         {
    //             ChangeState(AIState.Move);
    //         }
    //
    //         break;
    //     case AIState.Move:
    //         if (GetDistanceToPlayer() <= attackRange)
    //         {
    //             ChangeState(AIState.Attack);
    //         }
    //         else if (GetDistanceToPlayer() <= detectionRange && LineOfSight())
    //         {
    //             ChangeState(AIState.Move);
    //         }
    //         else
    //         {
    //             AIThink_Move();
    //         }
    //
    //
    //         break;
    //     case AIState.Attack:
    //         break;
    // }

    /// <summary>
    /// process information every frame
    /// </summary>
    protected abstract void AIBehaviour();
    // switch (currentState)
    // {
    //     case AIState.Idle:
    //         break;
    //     case AIState.Move:
    //         break;
    //
    //     case AIState.Attack:
    //         break;
    // }

    protected virtual float GetDistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerMasterScript.current.transform.position);
        if (PlayerMasterScript.INVISABLE)
        {
            distanceToPlayer = 1000f;
        }

        return distanceToPlayer;
    }

    protected virtual float GetDistanceToTarget(Vector3 target = new Vector3())
    {
        if (target.magnitude == 0)
        {
            target = attackTarget.position;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target);

        return distanceToTarget;
    }protected virtual float GetDistanceFromMovePosToTarget(Vector3 target = new Vector3())
    {
        if (target.magnitude == 0)
        {
            target = attackTarget.position;
        }

        float distanceToTarget = Vector3.Distance(movePos, target);

        return distanceToTarget;
    }

    protected virtual Vector3 GetDirectionToTarget(Vector3 target = new Vector3())
    {
        if (target.magnitude == 0)
        {
            target = attackTarget.position;
        }


        return target - transform.position;
    }

    protected void OrientateToTarget()
    {
        if (attackTarget)
        {
            Vector3 temp = Quaternion.LookRotation(GetDirectionToTarget()).eulerAngles;

            temp.x = bodyModel.transform.eulerAngles.x;
            temp.z = bodyModel.transform.eulerAngles.z;
            bodyModel.transform.eulerAngles = temp;
        }
    }

    //Idle

    protected virtual void EndState_Idle()
    {
    }

    protected virtual void ChangeState_Idle()
    {
    }

    protected virtual void AIThink_Idle()
    {
    }

    protected virtual void AIBehaviour_Idle()
    {
    }

    //Move
    protected virtual void EndState_Move()
    {
    }

    protected virtual void ChangeState_Move()
    {
    }

    protected virtual void AIThink_Move()
    {
    }

    protected virtual void AIBehaviour_Move()
    {
    }

    protected virtual void SetNewPatrolPoint()
    {
        // int temp = patrolManager.GetRandomPatrolIndex();
        // while (temp.Equals(patrolIndex))
        // {
        //     //making sure it is not the same patrol point
        //     temp = patrolManager.GetRandomPatrolIndex();
        // }
        //
        // patrolIndex = temp;
        // Debug.Log($"New Patrol point {patrolIndex}: {patrolPos}");
        // ResumePatrolPoint();
    }

    protected virtual void OverridePatrolPoint(Vector3 position)
    {
        SetNavAgent(position);
    }

    protected virtual void ResumePatrolPoint()
    {
        try
        {
            // SetNavAgent(patrolManager.GetPatrol(patrolIndex).position);
        }
        catch (Exception e)
        {
            SetNewPatrolPoint();
        }
    }

    protected virtual void SetNavAgent(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
        movePos = navMeshAgent.destination;
    }


    //Attack
    protected virtual void EndState_Attack()
    {
    }

    protected virtual void ChangeState_Attack(AttackSet attackSet = null)
    {
        lastAttack = attackSet;
    }

    protected virtual void AIThink_Attack()
    {
    }


    protected virtual void AIBehaviour_Attack()
    {
    }


    /// <summary>
    /// checks line of sight, only evaluates layers specified by LOSLayer
    /// </summary>
    /// <returns></returns>
    protected virtual bool LineOfSight()
    {
        if (!IsInCone())
        {
            return false;
        }

        if (Physics.Raycast(head.position, playerGO.transform.position - head.position,
            out RaycastHit hit, detectionRange, LOSLayer))
        {
            // print($"Enemy raycast hit something {hit.collider.name}");
            if (hit.collider.CompareTag("Player"))
            {
                if (PlayerMasterScript.INVISABLE)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected virtual bool IsInCone()
    {
        // print($"{Vector3.Dot(head.forward, (playerGO.transform.position + head.position - playerOffset).normalized)}, {Math.Cos(visionConeDegree)}");
        if (Vector3.Dot(head.forward,
                (playerGO.transform.position + PlayerMasterScript.current.GetCentreOfMass().position - head.position)
                .normalized) >
            Mathf.Abs(Mathf.Cos(visionConeDegree)))
        {
            return true;
        }

        return false;
    }

    protected virtual AttackSet PickAttack()
    {
        AttackSet selectedAttack = null;
        foreach (AttackSet attackSet in attackSets)
        {
            if (attackSet.attackCondition.needsLineOfSight)
            {
                if (attackSet.IsConditionMet(GetDistanceToTarget(), LineOfSight()))
                {
                    return attackSet;
                }
            }
            else
            {
                if (attackSet.IsConditionMet(GetDistanceToTarget(), true))
                {
                    return attackSet;
                }
            }
        }

        return selectedAttack;
    }

    protected virtual Vector3 SetMovePointByAttribute()
    {
        Vector3 returnPoint = transform.position;
        foreach (AIAttribute currentAttribute in attributesStack)
        {
            switch (currentAttribute)
            {
                case AIAttribute.Stationary:
                    break;
                case AIAttribute.Aggressive:
                    if (attackTarget != null)
                    {
                        returnPoint += attackTarget.position-transform.position;
                    }

                    break;
                case AIAttribute.Defensive:
                    List<PatrolPoint> returnList = new List<PatrolPoint>();
                    int i = 0;
                    while (attackTarget && returnList.Count == 0 && i <= 360)
                    {
                        Vector3 direction = -GetDirectionToTarget().normalized;
                        Vector3 moveTangent = Quaternion.AngleAxis((90f + i) * Random.Range(-1, 1), transform.up) *
                                              direction;
                        Vector3 temp = (direction * (defensive_Distance - GetDistanceToTarget()) +
                                        moveTangent.normalized * Random.Range(defensive_TangentRange.x,
                                            defensive_TangentRange.y));
                        temp += returnPoint;

                        returnList = currentPatrolZone.GetPoints(temp, defensive_Space);
                        i += 30;
                    }

                    if (returnList.Count > 0)
                    {
                        returnPoint = returnList[Random.Range(0, returnList.Count)].Position;
                    }

                    break;
                case AIAttribute.Stealthy:
                    break;
            }
        }

        return returnPoint;
    }

    protected virtual void SetTarget()
    {
        attackTarget = PlayerMasterScript.current.transform;
    }
    
    
    
    protected virtual void EndState_Stagger()
    {
    }

    protected virtual void ChangeState_Stagger()
    {
    }

    protected virtual void AIThink_Stagger()
    {
    }

    protected virtual void AIBehaviour_Stagger()
    {
    }
    
     protected virtual void EndState_Dead()
    {
    }

    protected virtual void ChangeState_Dead()
    {
    }

    protected virtual void AIThink_Dead()
    {
    }

    protected virtual void AIBehaviour_Dead()
    {
    }
    
    

}