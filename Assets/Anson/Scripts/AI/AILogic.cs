using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
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
    OrientateToTarget,
    AttackBehindCover,
    RandomMovement
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

    [Header("Orientate To Target")]
    [SerializeField]
    private bool freezeY = true;

    [Header("Aggressive")]
    [SerializeField]
    protected float aggressive_distanceToTarget = 3f;
    
    [Header("Defensive")]
    [SerializeField]
    protected float defensive_Distance = 5f;

    [SerializeField]
    protected float defensive_Space = 3f;

    [SerializeField]
    protected Vector2 defensive_TangentRange = new Vector2(2f, 4f);


    [Header("Take Cover")]
    [SerializeField]
    protected float takeCover_CoverSpace = 3f;

    [SerializeField]
    private float takeCover_CoverDot = 0.7f;

    [SerializeField]
    private float takeCover_DefenceDistanceMultiplier = .5f;

    [Header("Random Movement")]
    [SerializeField]
    private bool overrideMovementIfOutOfRange;


    [Space(10f)]
    [Header("AI Decision Times")]
    [SerializeField]
    float lastThinkTime;

    [SerializeField]
    float thinkRate = 0.5f;

    [SerializeField]
    private float offsetThinkTime = 0;

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
    protected Quaternion targetRotation;

    [SerializeField]
    protected float rotateSpeed = 10f;

    [Header("AI Attack")]
    [SerializeField]
    public List<AttackSet> attackSets;

    [SerializeField]
    protected AttackSet lastAttack;

    [SerializeField]
    protected bool autoSetPlayerToTarget;
    
    [SerializeField]
    protected Transform attackTarget;

    [SerializeField]
    [Range(0f, 90f)]
    private float visionConeDegree = 45f;


    [Header("AI Stagger")]
    [SerializeField]
    [Range(0f, 1f)]
    protected float staggerValue = 0f;

    [SerializeField]
    private float staggerDamageMultiplier = 0.002f;

    [SerializeField]
    protected float staggerTime;

    [SerializeField]
    protected float staggerTimeNow;

    [SerializeField]
    protected UnityEvent onStaggerEvent;

    [SerializeField]
    protected UnityEvent endStaggerEvent;


    [Header("Debug")]
    [SerializeField]
    private bool DrawDebug;

    [Header("Other Components")]
    [SerializeField]
    private EnemyHandler enemyHandler;

    [SerializeField]
    protected Transform bodyModel;

    [SerializeField]
    protected NavMeshAgent navMeshAgent;

    // [SerializeField]
    // PatrolManager patrolManager;

    [SerializeField]
    protected PatrolZone currentPatrolZone;


    [SerializeField]
    protected Transform head;

    private static float SnapRotationThresshold;


    public Transform AttackTarget => attackTarget;

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
        offsetThinkTime = Random.Range(0f, -thinkRate);
        if (!enemyHandler)
        {
            enemyHandler = GetComponent<EnemyHandler>();
        }

        RandomiseAttackTime();
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        SetNewPatrolPoint();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lastThinkTime + thinkRate+offsetThinkTime <= Time.time)
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

    public void SetPatrolZone(PatrolZone patrolZone)
    {
        currentPatrolZone = patrolZone;
    }


    public virtual void ChangeState(AIState newState, AttackSet attackSet = null)
    {
        previousState = currentState;
        currentState = newState;

        // Debug.Log($"AI: {name}: Change State: {previousState} --> {currentState}");
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
            case AIState.Stagger:
                EndState_Stagger();
                break;
            case AIState.Dead:
                EndState_Dead();
                break;
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
            case AIState.Stagger:
                ChangeState_Stagger();
                break;
            case AIState.Dead:
                ChangeState_Dead();
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

    public virtual float GetDistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerMasterScript.current.transform.position);
        if (PlayerMasterScript.INVISABLE)
        {
            distanceToPlayer = 1000f;
        }

        return distanceToPlayer;
    }

    public virtual float GetDistanceToTarget(Vector3 target = new Vector3())
    {
        if (target.magnitude == 0)
        {
            if (attackTarget)
            {
                target = attackTarget.position;
            }
            else
            {
                return Mathf.Infinity;
            }
        }

        float distanceToTarget = Vector3.Distance(transform.position, target);

        return distanceToTarget;
    }

    public virtual float GetDistanceFromMovePosToTarget(Vector3 target = new Vector3())
    {
        if (target.magnitude == 0)
        {
            if (attackTarget)
            {
                target = attackTarget.position;
            }
            else
            {
                return Mathf.Infinity;
            }
        }

        float distanceToTarget = Vector3.Distance(movePos, target);

        return distanceToTarget;
    }

    public virtual Vector3 GetDirectionToTarget(Vector3 target = new Vector3())
    {
        if (target.magnitude == 0)
        {
            if (attackTarget)
            {
                target = attackTarget.position;
            }
        }


        return target - transform.position;
    }

    protected virtual void SetOrientateToTarget()
    {
        if (targetRotation == Quaternion.identity)
        {
            targetRotation = transform.rotation;
        }

        if (attackTarget)
        {
            targetRotation = Quaternion.LookRotation(GetDirectionToTarget());
            Vector3 temp = targetRotation.eulerAngles;
            if (freezeY)
            {
                temp.x = bodyModel.transform.eulerAngles.x;
                temp.z = bodyModel.transform.eulerAngles.z;
            }
            else
            {
                temp.z = bodyModel.transform.eulerAngles.z;
            }

            targetRotation.eulerAngles = temp;
        }
    }

    protected virtual void UpdateOrientation()
    {
        if (targetRotation == Quaternion.identity)
        {
            targetRotation = transform.rotation;
        }

        if (bodyModel.rotation != targetRotation)
        {
            SnapRotationThresshold = 0.01f;
            if (bodyModel.eulerAngles.magnitude == 0 || Quaternion.Angle(bodyModel.transform.rotation, targetRotation) >
                SnapRotationThresshold)
            {
                if (
                    targetRotation.eulerAngles.magnitude != 0)
                {
                    bodyModel.rotation =
                        Quaternion.Lerp(bodyModel.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                }
            }
            else
            {
                bodyModel.transform.rotation = targetRotation;
            }
        }
    }

    //Idle

    protected virtual void EndState_Idle()
    {
    }

    protected virtual void ChangeState_Idle()
    {
        if (!attackTarget && autoSetPlayerToTarget)
        {
            attackTarget = PlayerMasterScript.current.transform;
        }
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
        MoveAnimation();
    }

    protected virtual void ChangeState_Move()
    {
        MoveAnimation();
    }

    protected virtual void AIThink_Move()
    {
    }

    protected virtual void AIBehaviour_Move()
    {
        MoveAnimation();
    }

    protected virtual void MoveAnimation()
    {
        if (enemyHandler)
        {
            enemyHandler.OnMove(navMeshAgent.velocity);
        }
        else
        {
            SendMessage("OnMove");
        }
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
        if (navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(position);
            movePos = navMeshAgent.destination;
        }
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
        if (!attackTarget || !IsInCone())
        {
            return false;
        }

        Vector3 target = attackTarget.transform.position + new Vector3(0, .5f, 0);
        if (attackTarget.tag.Equals("Player"))
        {
            target = PlayerMasterScript.current.GetCentreOfMass().position;
        }

        Vector3 dir = target - head.position;

        if (Physics.Raycast(head.position, dir.normalized,
                out RaycastHit hit, detectionRange, LOSLayer))
        {
            // print($"Enemy raycast hit something {hit.collider.name}");
            if (hostileTags.Contains(hit.collider.tag))
            {
                if (DrawDebug)
                {
                    Debug.DrawLine(head.position, head.position + dir, Color.green, thinkRate);
                }

                if (PlayerMasterScript.INVISABLE)
                {
                    return false;
                }

                return true;
            }
        }

        if (DrawDebug)
        {
            Debug.DrawLine(head.position, head.position + dir, Color.red, thinkRate);
        }

        return false;
    }

    protected virtual bool IsInCone()
    {
        // print($"{Vector3.Dot(head.forward, (playerGO.transform.position + head.position - playerOffset).normalized)}, {Math.Cos(visionConeDegree)}");
        Vector3 target = attackTarget.transform.position + new Vector3(0, .5f, 0);


        if (attackTarget.tag.Equals("Player"))
        {
            target = PlayerMasterScript.current.GetCentreOfMass().position;
        }

        float dotValue = Vector3.Dot(head.forward,
            (target - head.position)
            .normalized);
        if (dotValue >
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

    protected virtual void RandomiseAttackTime()
    {
        foreach (AttackSet attackSet in attackSets)
        {
            attackSet.lastAttackTime = Random.Range(0f, attackSet.attackCondition.cooldown);
        }
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
                        returnPoint += attackTarget.position - transform.position;
                    }

                    break;
                case AIAttribute.Defensive:


                    List<PatrolPoint> returnList = new List<PatrolPoint>();
                    int i = 0;
                    int startingSide = 0;
                    while (startingSide == 0)
                    {
                        startingSide = Random.Range(-1, 2);
                    }

                    // Debug.Log(startingSide);
                    while (attackTarget && returnList.Count == 0 && Mathf.Abs(i) <= 360)
                    {
                        Vector3 direction = -GetDirectionToTarget().normalized;
                        Vector3 moveTangent = Quaternion.AngleAxis((90f * startingSide + i), transform.up) *
                                              direction;
                        Vector3 temp = (direction * (defensive_Distance - GetDistanceToTarget()) +
                                        moveTangent.normalized * Random.Range(defensive_TangentRange.x,
                                            defensive_TangentRange.y));
                        temp += returnPoint;

                        returnList = currentPatrolZone.GetPoints(temp, defensive_Space);
                        i -= 180 * startingSide;
                    }

                    if (returnList.Count > 0)
                    {
                        returnPoint = returnList[Random.Range(0, returnList.Count)].Position;
                    }

                    break;
                case AIAttribute.Stealthy:
                    break;
                case AIAttribute.AttackBehindCover:
                    if (attackTarget)
                    {
                        bool foundPoint = false;
                        //Get points near enemy
                        List<PatrolPoint> coverReturnList = currentPatrolZone.GetCover(transform.position,
                            takeCover_CoverSpace,
                            CoverType.Half, GetDirectionToTarget(), takeCover_CoverDot);
                        if (coverReturnList.Count > 0)
                        {
                            Vector3 proposedPoint = coverReturnList[Random.Range(0, coverReturnList.Count)].Position;
                            if (Vector3.Distance(proposedPoint, attackTarget.position) >
                                takeCover_DefenceDistanceMultiplier * defensive_Distance)
                            {
                                returnPoint = proposedPoint;
                                foundPoint = true;
                            }
                        }

                        //Get points near return point
                        if (!foundPoint)
                        {
                            coverReturnList = currentPatrolZone.GetCover(returnPoint, takeCover_CoverSpace,
                                CoverType.Half,
                                GetDirectionToTarget(), takeCover_CoverDot);
                            if (coverReturnList.Count > 0)
                            {
                                returnPoint = coverReturnList[Random.Range(0, coverReturnList.Count)].Position;
                            }
                        }
                    }

                    break;
                case AIAttribute.RandomMovement:
                    if (overrideMovementIfOutOfRange)
                    {
                        if (attributesStack.Contains(AIAttribute.Aggressive) && !attackTarget)
                        {
                            returnPoint = currentPatrolZone.GetRandomPoint();

                        }
                        else if (attributesStack.Contains(AIAttribute.Defensive))
                        {
                            
                            if (!attackTarget || Vector3.Distance(attackTarget.position, transform.position) >
                                defensive_Distance)
                            {
                                returnPoint = currentPatrolZone.GetRandomPoint();
                            }
                        }
                        
                    }
                    else
                    {
                        returnPoint = currentPatrolZone.GetRandomPoint();
                    }

                    break;
            }
        }

        return returnPoint;
    }

    protected virtual void SetTarget()
    {
        if (detectionRange > GetDistanceToPlayer())
        {
            attackTarget = PlayerMasterScript.current.transform;
        }
    }


    protected virtual void EndState_Stagger()
    {
        endStaggerEvent.Invoke();
        staggerValue = 0f;
        navMeshAgent.enabled = true;
    }

    protected virtual void ChangeState_Stagger()
    {
        onStaggerEvent.Invoke();
        staggerTimeNow = staggerTime;
        SetNavAgent(transform.position);
        navMeshAgent.enabled = false;
    }

    protected virtual void AIThink_Stagger()
    {
    }

    protected virtual void AIBehaviour_Stagger()
    {
        staggerTimeNow -= Time.deltaTime;
        if (staggerTimeNow <= 0f)
        {
            ChangeState(AIState.Idle);
        }
    }

    public virtual void AddStagger(float dmg)
    {
        if (staggerValue < 1f)
        {
            staggerValue += dmg * staggerDamageMultiplier;
            if (staggerValue >= 1f)
            {
                ChangeState(AIState.Stagger);
            }
        }
    }

    protected virtual void EndState_Dead()
    {
    }

    protected virtual void ChangeState_Dead()
    {
        SetNavAgent(transform.position);
        navMeshAgent.enabled = false;
    }

    protected virtual void AIThink_Dead()
    {
    }

    protected virtual void AIBehaviour_Dead()
    {
    }
}