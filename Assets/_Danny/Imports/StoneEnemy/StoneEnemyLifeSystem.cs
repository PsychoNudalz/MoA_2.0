﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEnemyLifeSystem : LifeSystemScript
{
    [Header("Shader Effects")]
    [SerializeField] TargetMaterialHandlerScript targetMaterialHandler;

    public override int takeDamage(float dmg, int level, ElementTypes element)
    {
        targetMaterialHandler.StartDecay();
        return base.takeDamage(dmg, level, element);
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier)
    {
        targetMaterialHandler.StartDecay();
        return base.takeDamageCritical(dmg, level, element, multiplier);
    }
    public override void RemoveDebuff(FireEffectScript debuff = null)
    {
        base.RemoveDebuff(debuff as DebuffScript);
        targetMaterialHandler.SetFire(CheckIsStillOnFire());
    }
    public override void ApplyDebuff(FireEffectScript debuff)
    {
        base.ApplyDebuff(debuff as DebuffScript);
        targetMaterialHandler.SetFire(true);

    }
    public override void ApplyDebuff(ShockEffectScript debuff)
    {
        base.ApplyDebuff(debuff as DebuffScript);

    }

    public override void ResetSystem()
    {
        base.ResetSystem();
        targetMaterialHandler.SetFire(false);
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
}
