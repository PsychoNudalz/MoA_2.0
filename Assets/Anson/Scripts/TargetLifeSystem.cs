using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLifeSystem : LifeSystemScript
{
    [Header("Shader Effects")]
    [SerializeField] TargetMaterialHandlerScript targetMaterialHandler;

    public override int takeDamage(float dmg, int level, ElementTypes element)
    {
        targetMaterialHandler.StartDecay();
        switch (element)
        {
            case (ElementTypes.FIRE):
                break;
        }
        return base.takeDamage(dmg, level, element);
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier)
    {
        targetMaterialHandler.StartDecay();
        return base.takeDamageCritical(dmg, level, element, multiplier);
    }
    public override void RemoveDebuff(FireEffectScript debuff = null)
    {
        targetMaterialHandler.SetFire(false);
        base.RemoveDebuff(debuff as DebuffScript);
    }
    public override void ApplyDebuff(FireEffectScript debuff)
    {
        base.ApplyDebuff(debuff as DebuffScript);
        targetMaterialHandler.SetFire(true);

    }

    public override void ResetSystem()
    {
        base.ResetSystem();
        targetMaterialHandler.SetFire(false);
    }
}
