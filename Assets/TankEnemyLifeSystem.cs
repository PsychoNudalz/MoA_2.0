using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemyLifeSystem : TargetLifeSystem
{
    [Header("Tank Enemy Agent")]
    TankEnemyAgent TankEnemyAgent;

    private void Start()
    {
        TankEnemyAgent = GetComponent<TankEnemyAgent>();
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier, bool displayTakeDamageEffect = true)
    {
        StaggerAnimation();
        return base.takeDamageCritical(dmg, level, element, multiplier, displayTakeDamageEffect);
    }

    public override bool CheckDead()
    {
        bool retValue = base.CheckDead();
        if (retValue)
        {
            TankEnemyAgent.DeathAnimation();
        }
        return retValue;
    }

    private void StaggerAnimation()
    {
        TankEnemyAgent.Stagger();
    }
}
