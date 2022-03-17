using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Grounded : AILogic
{
    [Header("AI_Ground")]
    private float temp;


    public override void ChangeState(AIState newState, AttackSet attackSet = null)
    {
        base.ChangeState(newState, attackSet);
    }

    protected override void AIThink()
    {
        switch (currentState)
        {
            case AIState.Idle:
                AIThink_Idle();

                break;
            case AIState.Move:
                AIThink_Move();
                break;
            case AIState.Attack:
                AIThink_Attack();
                break;
            case AIState.Stagger:
                AIThink_Stagger();;
                break;
            case AIState.Dead:
                break;
        }
    }

    protected override void AIBehaviour()
    {
        switch (currentState)
        {
            case AIState.Idle:
                break;
            case AIState.Move:
                AIBehaviour_Move();
                break;

            case AIState.Attack:
                break;
            case AIState.Stagger:
                AIBehaviour_Stagger();
                break;
            case AIState.Dead:
                break;
        }
    }

    protected override void EndState_Idle()
    {
        base.EndState_Idle();
    }

    protected override void ChangeState_Idle()
    {
        base.ChangeState_Idle();
    }

    protected override void AIThink_Idle()
    {
        base.AIThink_Idle();
        if (!attackTarget)
        {
            ChangeState(AIState.Move);

        }
        AttackSet temp = PickAttack();
        if (temp == null)
        {
            ChangeState(AIState.Move);
        }

        else
        {
            ChangeState(AIState.Attack, temp);
        }
    }

    protected override void AIBehaviour_Idle()
    {
        base.AIBehaviour_Idle();
    }

    protected override void EndState_Move()
    {
        base.EndState_Move();
    }

    protected override void ChangeState_Move()
    {
        base.ChangeState_Move();
        SetNewPatrolPoint();
    }

    protected override void AIThink_Move()
    {

        if (Vector3.Distance(movePos, transform.position) < moveStopRange)
        {
            if (MoveWaitTime_Now <= 0)
            {
                ChangeState(AIState.Idle);
            }
        }
        else
        {
            
            if (attackTarget)
            {
                AttackSet temp = PickAttack();
                if (temp != null)
                {
                    ChangeState(AIState.Attack, temp);

                }
            }

            if (movePos.magnitude == 0||GetDistanceFromMovePosToTarget()<defensive_Distance*0.7f||GetDistanceToTarget()<defensive_Distance*0.7f)
            {
                SetNewPatrolPoint();
            }
        }
    }

    private void SetNewPatrolPoint()
    {
        if (!attackTarget)
        {
            SetTarget();
        }

        SetNavAgent(SetMovePointByAttribute());
        MoveWaitTime_Now = Random.Range(MoveWaitTime.x, MoveWaitTime.y);
    }

    protected override void AIBehaviour_Move()
    {
        if (attributesStack.Contains(AIAttribute.OrientateToTarget))
        {
            OrientateToTarget();
        }

        if (Vector3.Distance(movePos, transform.position) < moveStopRange)
        {
            if (MoveWaitTime_Now > 0)
            {
                MoveWaitTime_Now -= Time.deltaTime;
            }
        }
    }

    protected override void EndState_Attack()
    {
        base.EndState_Attack();
    }

    protected override void ChangeState_Attack(AttackSet attackSet = null)
    {
        base.ChangeState_Attack(attackSet);
        if (attackSet != null)
        {
            if (attackSet.faceTarget)
            {
                OrientateToTarget();
            }

            if (!attackSet.canMove)
            {
                SetNavAgent(transform.position);
            }
            attackSet.Attack();
        }
    }

    protected override void AIThink_Attack()
    {
        if (lastAttack != null)
        {
            if (lastAttack.canMove)
            {
                ChangeState(AIState.Move);
            }
            else if (Time.time - lastAttack.lastAttackTime > lastAttack.attackCondition.duration)
            {
                ChangeState(AIState.Idle);
            }
        }

        else
        {
            ChangeState(AIState.Idle);
        }
        base.AIThink_Attack();
    }

    protected override void AIBehaviour_Attack()
    {
        base.AIBehaviour_Attack();
        if (lastAttack.canMove&& attributesStack.Contains(AIAttribute.OrientateToTarget))
        {
            OrientateToTarget();
        }

    }

    protected override Vector3 SetMovePointByAttribute()
    {
        return base.SetMovePointByAttribute();
    }
}