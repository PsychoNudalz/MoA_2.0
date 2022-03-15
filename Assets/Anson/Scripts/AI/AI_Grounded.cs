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
            if (movePos.magnitude == 0)
            {
                SetNewPatrolPoint();
            }
        }
    }

    private void SetNewPatrolPoint()
    {
        List<PatrolPoint> points = new List<PatrolPoint>();
        int i = 0;
        while (points.Count == 0 && i < 720)
        {
            points = currentPatrolZone.GetPoints(
                Quaternion.EulerAngles(0, i, 0) * transform.forward * Random.Range(3f, 5f) + transform.position,
                Random.Range(1, 2f));
            i++;
        }

        if (points.Count > 0)
        {
            SetNavAgent(points[Random.Range(0, points.Count)].Position);
            MoveWaitTime_Now = Random.Range(MoveWaitTime.x, MoveWaitTime.y);
        }
    }

    protected override void AIBehaviour_Move()
    {
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
            attackSet.Attack();
        }
    }

    protected override void AIThink_Attack()
    {
        base.AIThink_Attack();
    }

    protected override void AIBehaviour_Attack()
    {
        base.AIBehaviour_Attack();
    }
}