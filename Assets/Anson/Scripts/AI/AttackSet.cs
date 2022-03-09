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
        public float time;
        public float range;
        public bool needsLineOfSight;
    }

    public AttackCondition attackCondition;

    public UnityEvent OnAttack;

    public int priority;
    public float lastAttack;
    public bool canMove;


    public void Attack()
    {
        lastAttack = Time.time;
        OnAttack.Invoke();
    }

    public bool IsConditionMet(float distance,bool lineOfSight)
    {
        if (attackCondition.range > distance)
        {
            if (attackCondition.time < Time.time - lastAttack)
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