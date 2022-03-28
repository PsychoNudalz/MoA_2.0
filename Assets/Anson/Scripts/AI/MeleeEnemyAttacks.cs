using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttacks : EnemyAttacks
{
    [Header("Melee Attack")]
    [SerializeField]
    private string meleeAttackAnimatorTrigger = "MeleeAttack";

    [SerializeField]
    private SphereCastDamageScript sphereCastDamageScript;
    [SerializeField] float meleeDamage = 100f;

    [SerializeField]
    private float meleeRange = 1f;

    [SerializeField]
    private AnimationCurve meleeRangeCurve;
    

    [Header("Light Attack")]
    [SerializeField]
    private string lightAttackAnimatorTrigger = "LightAttack";

    [Header("Heavy Attack")]
    [SerializeField]
    private string heavyAttackAnimatorTrigger = "HeavyAttack";

    
    public void MeleeAttack()
    {
        soundScript.Play_Attack();
        animator.SetTrigger(meleeAttackAnimatorTrigger);
    }
    public void MeleeAttack_SphereDamage()
    {
        sphereCastDamageScript.SphereCastDamageArea(meleeDamage, meleeRange, meleeRangeCurve, 1, ElementTypes.SHOCK);

    }
    
    public void LightAttack()
    {
        soundScript.Play_Attack();
        animator.SetTrigger(lightAttackAnimatorTrigger);
    }

    public void HeavyAttack()
    {
        soundScript.Play_Attack();
        animator.SetTrigger(heavyAttackAnimatorTrigger);
    }
}
