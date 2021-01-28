using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEnemyLifeSystem : LifeSystemScript
{
    [Header("Shader Effects")]
    [SerializeField] StoneEnemyMaterialHandlerScript stoneEnemyMaterialHandler;
    StoneEnemyAgent stoneEnemyAgent;

    private void Start()
    {
        stoneEnemyAgent = GetComponent<StoneEnemyAgent>();
    }

    public override int takeDamage(float dmg, int level, ElementTypes element)
    {
        StaggerAnimation();
        stoneEnemyMaterialHandler.StartDecay();
        return base.takeDamage(dmg, level, element);
        
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier)
    {
        stoneEnemyMaterialHandler.StartDecay();
        return base.takeDamageCritical(dmg, level, element, multiplier);
    }
    public override void RemoveDebuff(FireEffectScript debuff = null)
    {
        base.RemoveDebuff(debuff as DebuffScript);
        stoneEnemyMaterialHandler.SetFire(CheckIsStillOnFire());
    }
    public override void ApplyDebuff(FireEffectScript debuff)
    {
        base.ApplyDebuff(debuff as DebuffScript);
        stoneEnemyMaterialHandler.SetFire(true);

    }
    public override void ApplyDebuff(ShockEffectScript debuff)
    {
        base.ApplyDebuff(debuff as DebuffScript);

    }

    public override void ResetSystem()
    {
        base.ResetSystem();
        stoneEnemyMaterialHandler.SetFire(false);
    }

    bool CheckIsStillOnFire()
    {
        foreach (DebuffScript d in debuffList)
        {
            if (d is FireEffectScript)
            {
                return true;
            }
        }
        return false;

    }

    private void StaggerAnimation()
    {
        stoneEnemyAgent.Stagger();
    }
}
