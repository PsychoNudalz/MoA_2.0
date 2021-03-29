using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffectScript : ElementDebuffScript
{

    float StartDuration;
    float currentTime = 0;
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
            currentIce.ShatterIceOnTarget();
            currentIce.DeactivateEffect();
            DeactivateEffect();
        }
        else
        {
            base.ApplyEffect(targetLS);
            effectDamage = effectDamage * 2;
            ActiveIceOnTarget();

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
            targetLS.takeDamage(effectDamage * (StartDuration - duration )*2/StartDuration, 1, ElementTypes.ICE);
        }
    }

    void ActiveIceOnTarget()
    {
        if (targetLS is TargetLifeSystem)
        {
            if (targetLS.TryGetComponent(out TargetHandlerScript targetHandler))
            {
                targetHandler.TargetMaterialHandler.SetIceShard(effectPotency);
            }
        }

    }
    void ShatterIceOnTarget()
    {
        if (targetLS is TargetLifeSystem)
        {
            if (targetLS.TryGetComponent(out TargetHandlerScript targetHandler))
            {
                targetHandler.TargetMaterialHandler.ShatterIceShards(StartDuration- duration);
            }
        }
    }
}
