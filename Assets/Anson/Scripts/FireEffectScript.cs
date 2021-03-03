using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class FireEffectScript : ElementDebuffScript
{
    float currentTime;
    float fireDamage = 0;
    float tickTime = 0.5f;
    public FireEffectScript()
    {

    }


    public override void init(float effectDamage, float effectPotency, List<string> tagList, LayerMask layerMask)
    {
        this.effectDamage = effectDamage;
        this.effectPotency = effectPotency;
        duration = effectPotency;
        fireDamage = effectDamage / (1 / tickTime * duration);
        this.tagList = tagList;
        this.layerMask = layerMask;
    }

    public override bool TickEffect(float deltaTime)
    {
        currentTime += deltaTime;
        if (currentTime >= tickTime)
        {
            targetLS.takeDamage(fireDamage * (currentTime / tickTime), 1, ElementTypes.FIRE);
            currentTime = 0;
        }

        return base.TickEffect(deltaTime);
    }

    public override bool TickEffect()
    {
        
        if (Time.time- currentTime >= tickTime)
        {
            targetLS.takeDamage(fireDamage * (currentTime / tickTime), 1, ElementTypes.FIRE);
            currentTime = Time.time;
        }

        return base.TickEffect();
    }



    public override void ApplyEffect(LifeSystemScript target)
    {
        base.ApplyEffect(target);
        currentTime = 0;
    }

    public override bool DeactivateEffect()
    {
        targetLS.RemoveDebuff(this as FireEffectScript);
        return true;
    }
}
