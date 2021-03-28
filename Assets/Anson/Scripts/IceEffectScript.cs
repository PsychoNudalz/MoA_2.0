using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffectScript : ElementDebuffScript
{

    float StartDuration;
    bool isShatter;
    
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
        targetLS = target;
        IceBehaviour();
    }

    public override bool DeactivateEffect()
    {
        targetLS.RemoveDebuff(this as IceEffectScript);
        return true;
    }

    void IceBehaviour()
    {
        Debug.Log("Player Icebehaviour");
        StartDuration = duration;

        IceEffectScript currentIce = targetLS.CheckIsStillOnIce(this);
        if (currentIce != null)
        {
            IceDamage(true);
            currentIce.IceDamage(false);
            currentIce.DeactivateEffect();
            DeactivateEffect();
        }
        else
        {
            base.ApplyEffect(targetLS);

        }

    }

    void IceDamage(bool fullDamage)
    {
        if (fullDamage)
        {
            targetLS.takeDamage(effectDamage, 1, ElementTypes.ICE);
        }
        else
        {
            targetLS.takeDamage(effectDamage * (1 - (duration / StartDuration)) * 2, 1, ElementTypes.ICE);
        }
    }
}
