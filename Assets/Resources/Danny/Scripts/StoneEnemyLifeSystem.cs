using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEnemyLifeSystem : TargetLifeSystem
{
    [Header("Stone Enemy Agent")]
    StoneEnemyAgent stoneEnemyAgent;

    private void Start()
    {
        stoneEnemyAgent = GetComponent<StoneEnemyAgent>();
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element,float multiplier, bool displayTakeDamageEffect = true)
    {
        StaggerAnimation();
        return base.takeDamageCritical(dmg, level, element, multiplier,displayTakeDamageEffect);
    }

    public override bool CheckDead()
    {
        bool retValue = base.CheckDead();
        if (retValue)
        {
            stoneEnemyAgent.DeathAnimation();
        }
        return retValue;
    }

    private void StaggerAnimation()
    {
        stoneEnemyAgent.Stagger();
    }
}
