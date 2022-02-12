using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEnemyLifeSystem : TargetLifeSystem
{
    [Header("Stone Enemy Agent")]
    StoneEnemyAgent stoneEnemyAgent;

    EnemySpawner spawner;
    bool displayDecremented = false;

    private void Start()
    {
        stoneEnemyAgent = GetComponent<StoneEnemyAgent>();
        try
        {
            spawner = transform.parent.GetComponent<EnemySpawner>();
        }
        catch (System.NullReferenceException e)
        {
            print(name + " can't find spawner");
        }
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier,
        bool displayTakeDamageEffect = true)
    {
        StaggerAnimation();
        return base.takeDamageCritical(dmg, level, element, multiplier, displayTakeDamageEffect);
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