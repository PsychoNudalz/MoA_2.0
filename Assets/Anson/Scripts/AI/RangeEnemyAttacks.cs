using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyAttacks : EnemyAttacks
{

    
    [Header("Light Attack")]
    [SerializeField]
    private string lightAttackAnimatorTrigger = "LightAttack";

    [SerializeField]
    private Transform[] lightAttackTransforms;

    [Header("Heavy Attack")]
    [SerializeField]
    private string heavyAttackAnimatorTrigger = "HeavyAttack";


    public void LightAttack()
    {
        soundScript.Play_Attack();
        animator.SetTrigger(lightAttackAnimatorTrigger);
    }

    public void LightAttack_Aimed()
    {
        // Vector3 aimDir = aiLogic.GetDirectionToTarget();
        foreach (Transform lightAttackTransform in lightAttackTransforms)
        {
            lightAttackTransform.LookAt(aiLogic.AttackTarget.position);
        }
        LightAttack();
    }

    public void HeavyAttack()
    {
        soundScript.Play_Attack();
        animator.SetTrigger(heavyAttackAnimatorTrigger);
    }
}