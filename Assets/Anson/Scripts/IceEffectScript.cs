using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffectScript : ElementDebuffScript
{

    float currentTime;
    
    public IceEffectScript()
    {

    }
    public override void init(float effectDamage, float effectPotency, List<string> tagList, LayerMask layerMask)
    {
        this.effectDamage = effectDamage;
        this.effectPotency = effectPotency;
        duration = effectPotency;
        this.tagList = tagList;
        this.layerMask = layerMask;
    }

    public override void ApplyEffect(LifeSystemScript target)
    {
        base.ApplyEffect(target);
        currentTime = 0;
    }

    public override bool DeactivateEffect()
    {
        targetLS.RemoveDebuff(this as IceEffectScript);
        return true;
    }
}
