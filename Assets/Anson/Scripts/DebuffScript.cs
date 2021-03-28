using UnityEngine;

[System.Serializable]
public class DebuffScript
{
    protected float duration;
    protected float startTime;
    protected LifeSystemScript targetLS;
    protected bool manualTick;

    public virtual void ApplyEffect(LifeSystemScript target)
    {
        Debug.Log(this + " Apply");

        targetLS = target;
        startTime = Time.time;
        manualTick = false;
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
        if (!manualTick)
        {
            manualTick = true;
        }
        duration -= deltaTime;
        if (duration < 0)
        {
            return DeactivateEffect();
        }
        return false;
    }

    public virtual bool TickEffect()
    {
        Debug.Log(this + " Tick");
        if (duration < Time.time - startTime)
        {
            return DeactivateEffect();
        }
        return false;
    }


}
