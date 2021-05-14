﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifeSystem : TargetLifeSystem
{
    [Header("Boss Enemy Agent")]
    [SerializeField] Boss_AI bossAgent;
    EnemySpawner spawner;
    bool displayDecremented = false;

    private void Start()
    {
        //bossAgent = GetComponent<BossAgent>();
        try
        {

            spawner = transform.parent.GetComponent<EnemySpawner>();
        }
        catch (System.NullReferenceException e)
        {
            print(name + " can't find spawner");
        }
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
        //bossAgent.Stagger();
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
