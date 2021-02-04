using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLifeSystem : LifeSystemScript
{
    [Header("Target Handler")]
    [SerializeField] TargetHandlerScript targetHandler;
    [Header("Shader Effects")]
    [SerializeField] TargetMaterialHandlerScript targetMaterialHandler;

    public TargetMaterialHandlerScript TargetMaterialHandler { get => targetMaterialHandler;}

    private void Start()
    {
        targetHandler = GetComponent<TargetHandlerScript>();
        targetMaterialHandler = targetHandler.TargetMaterialHandler;
    }

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

    public override void RemoveDebuff(ShockEffectScript debuff)
    {
        base.RemoveDebuff(debuff);
        targetMaterialHandler.ResetShockList();
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
