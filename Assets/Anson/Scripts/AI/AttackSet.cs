using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

[CanBeNull]
[Serializable]
public class AttackSet
{
    [Serializable]
    public struct AttackCondition 
    {
        public float cooldown;
        public float duration;
        public float range;
        public bool needsLineOfSight;
    }

    public AttackCondition attackCondition;

    public UnityEvent OnAttack;

    public float lastAttackTime;
    public bool canMove;
    public bool faceTarget = true;


    public void Attack()
    {
        lastAttackTime = Time.time;
        OnAttack.Invoke();
    }

    public bool IsConditionMet(float distance,bool lineOfSight)
    {
        if (attackCondition.range > distance)
        {
            if (attackCondition.cooldown < Time.time - lastAttackTime)
            {

                if (attackCondition.needsLineOfSight)
                {
                    return lineOfSight;
                }
                return true;
            }
        }

        return false;
    }


}