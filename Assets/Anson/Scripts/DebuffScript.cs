using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DebuffScript
{
    protected float duration;
    protected LifeSystemScript targetLS;


    public virtual void ApplyEffect(LifeSystemScript target)
    {
        targetLS = target;
    }

    public virtual bool DeactivateEffect()
    {
        targetLS.RemoveDebuff(this);
        return true;
    }

    public virtual bool OverrideEffect()
    {
        return true;
    }

    public virtual bool TickEffect(float deltaTime)
    {
        duration -= deltaTime;
        if (duration < 0)
        {
            return DeactivateEffect();
        }
        return false;
    }


}
