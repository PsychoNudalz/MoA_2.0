using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class FireEffectScript : ElementDebuffScript
{
    float currentTime;
    float fireDamage = 0;
    float tickTime = 0.5f;
    public FireEffectScript(float effectDamage, float effectPotency) : base(effectDamage, effectPotency)
    {
        this.effectDamage = effectDamage;
        this.effectPotency = effectPotency;
        duration = effectPotency;
        fireDamage = effectDamage / (1/tickTime * duration);
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

    public override void ApplyEffect(LifeSystemScript target)
    {
        base.ApplyEffect(target);
    }

    public override bool DeactivateEffect()
    {
        targetLS.RemoveDebuff(this as FireEffectScript);
        return true;
    }
}
