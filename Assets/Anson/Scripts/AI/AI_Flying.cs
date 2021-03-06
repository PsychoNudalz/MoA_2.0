using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI_Flying : AILogic
{

    [Header("AI Flying")]
    [SerializeField]
    private float moveSpeed = 5f;

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
        UpdateOrientation();
        FlyMove();
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

    protected override void UpdateOrientation()
    {
        base.UpdateOrientation();
        Vector3 t = bodyModel.localEulerAngles;
        t.z = 0f;
        bodyModel.localEulerAngles = t;
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
        AttackSet pickedAttack = PickAttack();
        if (pickedAttack == null)
        {
            ChangeState(AIState.Move);
        }

        else
        {
            ChangeState(AIState.Attack, pickedAttack);
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
        SetNewPatrolPoint();
        base.ChangeState_Move();
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
                AttackSet pickedAttack = PickAttack();
                if (pickedAttack != null)
                {
                    ChangeState(AIState.Attack, pickedAttack);

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
            SetOrientateToTarget();
        }

        if (Vector3.Distance(movePos, transform.position) < moveStopRange)
        {
            if (MoveWaitTime_Now > 0)
            {
                MoveWaitTime_Now -= Time.deltaTime;
            }
        }
        base.AIBehaviour_Move();

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
                SetOrientateToTarget();
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
        if (lastAttack.faceTarget&& attributesStack.Contains(AIAttribute.OrientateToTarget))
        {
            SetOrientateToTarget();
        }

    }

    protected override Vector3 SetMovePointByAttribute()
    {
        return base.SetMovePointByAttribute();
    }

    protected override void SetNavAgent(Vector3 position)
    {
        movePos = position;
    }

    protected virtual void FlyMove()
    {
        Vector3 targetDir = (movePos -transform.position);
        transform.position += targetDir.normalized * Mathf.Min(moveSpeed, targetDir.magnitude) * Time.deltaTime;
    }

    protected override void EndState_Stagger()
    {
        endStaggerEvent.Invoke();
        staggerValue = 0f;
    }

    protected override void ChangeState_Stagger()
    {
        onStaggerEvent.Invoke();
        staggerTimeNow = staggerTime;
        SetNavAgent(transform.position);
    }

    protected override void AIThink_Stagger()
    {
        base.AIThink_Stagger();
    }

    protected override void AIBehaviour_Stagger()
    {
        base.AIBehaviour_Stagger();
    }
}
