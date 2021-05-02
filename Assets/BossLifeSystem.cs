using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifeSystem : TargetLifeSystem
{
    [Header("Stone Enemy Agent")]
    BossAgent bossAgent;

    private void Start()
    {
        bossAgent = GetComponent<BossAgent>();
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier, bool displayTakeDamageEffect = true)
    {
       // StaggerAnimation();
        return base.takeDamageCritical(dmg, level, element, multiplier, displayTakeDamageEffect);
    }

    public override bool CheckDead()
    {
        bool retValue = base.CheckDead();
        if (retValue)
        {
            bossAgent.DeathAnimation();
        }
        return retValue;
    }

    private void StaggerAnimation()
    {
        bossAgent.Stagger();
    }
}
