using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyAttacks : EnemyAttacks
{
    [Header("Light Attack")]
    [SerializeField]
    private string lightAttackAnimatorTrigger = "LightAttack";

    [Header("Heavy Attack")]
    [SerializeField]
    private string heavyAttackAnimatorTrigger = "HeavyAttack";


    public void LightAttack()
    {
        animator.SetTrigger(lightAttackAnimatorTrigger);
    }

    public void HeavyAttack()
    {
        animator.SetTrigger(heavyAttackAnimatorTrigger);
    }
}