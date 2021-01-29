using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockEffectScript : ElementDebuffScript
{
    float currentTime;
    float shockDamage = 0;
    float tickTime = .2f;
    List<LifeSystemScript> lsList;
    bool ignorePlayer = true;
    public LayerMask layerMask = new LayerMask();
    public List<string> tagList = new List<string>();


    public ShockEffectScript(float effectDamage, float effectPotency, List<string> tagList, LayerMask layerMask, bool ignorePlayer = true) : base(effectDamage, effectPotency)
    {
        this.effectDamage = effectDamage;
        this.effectPotency = effectPotency;
        duration = .6f;
        shockDamage = effectDamage/2f;
        this.ignorePlayer = ignorePlayer;
        this.tagList = tagList;
        this.layerMask = layerMask;
    }

    public override bool TickEffect(float deltaTime)
    {
        currentTime += deltaTime;
        if (currentTime > tickTime)
        {
            ActiveShockOnTarget(targetLS);
            currentTime = 0;
        }
        return base.TickEffect(deltaTime);
    }

    public override void ApplyEffect(LifeSystemScript target)
    {
        base.ApplyEffect(target);
        lsList = new List<LifeSystemScript>();
        lsList.Add(target);
        ShockCainEffect(target);
    }

    public override bool DeactivateEffect()
    {
        lsList = new List<LifeSystemScript>();
        lsList.Add(targetLS);
        ShockCainEffect(targetLS);
        targetLS.RemoveDebuff(this as ShockEffectScript);
        return true;
    }

    void ShockCainEffect(LifeSystemScript currentTarget)
    {
        currentTarget.takeDamage(shockDamage, 1, ElementTypes.SHOCK);
        RaycastHit[] hits = Physics.SphereCastAll(currentTarget.transform.position, effectPotency, currentTarget.transform.forward, effectPotency, layerMask);
        foreach (RaycastHit h in hits)
        {
            Collider c = h.collider;
            if (tagList.Contains(c.tag) && c.GetComponentInParent<LifeSystemScript>() != null)
            {
                LifeSystemScript lss = c.GetComponentInParent<LifeSystemScript>();
                if (!lsList.Contains(lss))
                {
                    lsList.Add(lss);
                    ActiveShockOnTarget(currentTarget, lss.transform);
                    ; ShockCainEffect(lss);
                }
            }
        }
        ActiveShockOnTarget(currentTarget);

    }

    void ActiveShockOnTarget(LifeSystemScript currentTarget, Transform nextTarget = null)
    {
        if (currentTarget is TargetLifeSystem)
        {
            if (currentTarget.TryGetComponent(out TargetHandlerScript targetHandler))
            {
                targetHandler.TargetMaterialHandler.SetShock(true, nextTarget);
            }
        }
    }

   
}
