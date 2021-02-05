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
