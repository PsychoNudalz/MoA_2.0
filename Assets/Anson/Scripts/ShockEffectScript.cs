using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockEffectScript : ElementDebuffScript
{
    float currentTime;
    float shockDamage = 0;
    float tickTime = .1f;
    List<LifeSystemScript> lsList;
    bool ignorePlayer = true;
    public LayerMask layerMask = new LayerMask();
    public List<string> tagList = new List<string>();


    public ShockEffectScript(float effectDamage, float effectPotency, List<string> tagList, LayerMask layerMask, bool ignorePlayer = true) : base(effectDamage, effectPotency)
    {
        this.effectDamage = effectDamage;
        this.effectPotency = effectPotency;
        duration = .8f;
        shockDamage = effectDamage;
        this.ignorePlayer = ignorePlayer;
        this.tagList = tagList;
        this.layerMask = layerMask;
    }

    public void SetLsList(List<LifeSystemScript> lsList)
    {
        this.lsList = lsList;
    }

    public override bool TickEffect(float deltaTime)
    {
        currentTime += deltaTime;
        if (currentTime > tickTime)
        {
            foreach(LifeSystemScript l in lsList)
            {
                UpdateShock(l);
            }
        }
        return base.TickEffect(deltaTime);
    }

    public override void ApplyEffect(LifeSystemScript target)
    {
        base.ApplyEffect(target);
        if (lsList == null)
        {
            lsList = new List<LifeSystemScript>();
        }
        lsList.Add(target);
        ShockCainEffect(target);
    }

    public override bool DeactivateEffect()
    {
        /*
        lsList = new List<LifeSystemScript>();
        lsList.Add(targetLS);
        ShockCainEffect(targetLS);
        */
        foreach (LifeSystemScript l in lsList)
        {
            UpdateShock(l);
        }
        targetLS.RemoveDebuff(this as ShockEffectScript);
        return true;
    }

    void ShockCainEffect(LifeSystemScript currentTarget)
    {
        currentTarget.takeDamage(shockDamage, 1, ElementTypes.SHOCK);
        RaycastHit[] hits = Physics.SphereCastAll(currentTarget.transform.position, effectPotency, currentTarget.transform.forward, effectPotency, layerMask);
        LifeSystemScript lss = GetClosestTarget(hits);
        if (lss != null)
        {

            lsList.Add(lss);
            ActiveShockOnTarget(currentTarget, lss.transform);
            ShockEffectScript newShock = new ShockEffectScript(effectDamage, effectPotency, tagList, layerMask, ignorePlayer);
            newShock.SetLsList(this.lsList);
            lss.ApplyDebuff(newShock);
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

    void UpdateShock(LifeSystemScript currentTarget)
    {
        if (currentTarget is TargetLifeSystem)
        {
            if (currentTarget.TryGetComponent(out TargetHandlerScript targetHandler))
            {
                targetHandler.TargetMaterialHandler.UpdateShock();
            }
        }
    }

    LifeSystemScript GetClosestTarget(RaycastHit[] hits)
    {
        LifeSystemScript currentHit = null;
        float minDist = Mathf.Infinity;
        LifeSystemScript lss;

        foreach (RaycastHit h in hits)
        {
            Collider c = h.collider;
            if (tagList.Contains(c.tag) && c.GetComponentInParent<LifeSystemScript>() != null)
            {
                lss = c.GetComponentInParent<LifeSystemScript>();
                if (!lsList.Contains(lss) && minDist > (targetLS.transform.position - h.transform.position).magnitude)
                {
                    currentHit = lss;
                    minDist = (targetLS.transform.position - h.transform.position).magnitude;

                }
            }
        }
        return currentHit;

    }


}
