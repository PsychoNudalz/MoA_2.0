using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyLifeSystem : TargetLifeSystem
{
    [Header("Shooting Enemy Agent")]
    ShootingEnemyAgent shootingEnemyAgent;
    EnemySpawner spawner;
    bool displayDecremented = false;

    private void Start()
    {
        shootingEnemyAgent = GetComponent<ShootingEnemyAgent>();
        if (transform.parent)
        {
            spawner = transform.parent.GetComponent<EnemySpawner>();
        }
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
            shootingEnemyAgent.DeathAnimation();
        }
        return retValue;
    }

    private void StaggerAnimation()
    {
        shootingEnemyAgent.Stagger();
    }

    public override void DeathBehaviour()
    {
        base.DeathBehaviour();
        if (!displayDecremented)
        {
            spawner.DecrementEnemies();
            displayDecremented = true;
        }
    }
}
