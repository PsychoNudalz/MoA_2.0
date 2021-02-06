using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyLifeSystem : TargetLifeSystem
{
    [Header("Shooting Enemy Agent")]
    ShootingEnemyAgent shootingEnemyAgent;

    private void Start()
    {
        shootingEnemyAgent = GetComponent<ShootingEnemyAgent>();
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier)
    {
        StaggerAnimation();
        return base.takeDamageCritical(dmg, level, element, multiplier);
    }

    public override bool CheckDead()
    {
        bool retValue = base.CheckDead();
        if (retValue)
        {
            shootingEnemyAgent.DeathAnimation();
        }
        return retValue;
    }

    private void StaggerAnimation()
    {
        shootingEnemyAgent.Stagger();
    }
}
