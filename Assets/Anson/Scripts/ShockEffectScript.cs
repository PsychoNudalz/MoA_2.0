using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockEffectScript : ElementDebuffScript
{
    float currentTime = 0;
    float shockDamage = 0;
    float tickTime = .5f;
    int maxShocks = 10;
    List<LifeSystemScript> lsList;
    Queue<LifeSystemScript> toShockQueue;
    int lsListPointer = 0;
    bool ignorePlayer = true;
    public LayerMask layerMask = new LayerMask();
    public List<string> tagList = new List<string>();

    public Queue<LifeSystemScript> ToShockQueue { get => toShockQueue; set => toShockQueue = value; }

    public ShockEffectScript()
    {

    }

    public void init(float effectDamage, float effectPotency, List<string> tagList, LayerMask layerMask, bool ignorePlayer = true)
    {
        this.effectDamage = effectDamage;
        this.effectPotency = effectPotency;
        duration = .25f;
        shockDamage = effectDamage;
        this.ignorePlayer = ignorePlayer;
        this.tagList = tagList;
        this.layerMask = layerMask;
    }

    public void SetLsList(List<LifeSystemScript> lsList, int ptr)
    {
        this.lsList = lsList;
        lsListPointer = ptr;
    }

    public override bool TickEffect(float deltaTime)
    {
        currentTime += deltaTime;
        float currentCount = lsList.Count;
        return base.TickEffect(deltaTime);
    }


    public override void ApplyEffect(LifeSystemScript target)
    {
        if (target == null)
        {
            return;
        }

        base.ApplyEffect(target);
        if (lsList == null)
        {
            lsList = new List<LifeSystemScript>();
        }
        lsList.Add(target);
        Debug.Log(target.name + " apply Shock");
        if (effectPotency > 0)
        {
            ShockCainEffect(target);
        }

    }

    public override bool DeactivateEffect()
    {
        /*
        lsList = new List<LifeSystemScript>();
        lsList.Add(targetLS);
        ShockCainEffect(targetLS);
        for (int i = 0; i <lsList.Count; i++)
        {
            UpdateShock(lsList[i]);
        }
        */
        while (toShockQueue != null&& toShockQueue.Count != 0)
        {
            //ShockEffectScript newShock = new ShockEffectScript(effectDamage, effectPotency, tagList, layerMask, ignorePlayer);
            ShockEffectScript newShock = new ShockEffectScript();
            newShock.init(effectDamage, effectPotency, tagList, layerMask);

            newShock.SetLsList(lsList, lsListPointer);
            toShockQueue.Dequeue().ApplyDebuff(newShock);
            //Debug.Log("Current Queue size: " + toShockQueue.Count);
        }
        targetLS.RemoveDebuff(this as ShockEffectScript);
        return true;
    }

    void ShockCainEffect(LifeSystemScript currentTarget)
    {
        currentTarget.takeDamage(shockDamage, 1, ElementTypes.SHOCK);

        toShockQueue = SphereCastShock(currentTarget);

        ActiveShockOnTarget(currentTarget);

        //Debug.Log("initial Queue size: " + toShockQueue.Count);

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
    void ActiveShockOnTarget(LifeSystemScript currentTarget, LifeSystemScript nextTarget)
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

    LifeSystemScript GetClosestTarget(Transform currentTarget)
    {
        LifeSystemScript currentHit = null;
        float minDist = Mathf.Infinity;
        LifeSystemScript lss;
        RaycastHit[] hits = Physics.SphereCastAll(currentTarget.position, effectPotency, currentTarget.forward, effectPotency, layerMask);

        foreach (RaycastHit h in hits)
        {
            Debug.Log(lsList.Count);
            Collider c = h.collider;
            if (tagList.Contains(c.tag) && c.GetComponentInParent<LifeSystemScript>() != null && (c.GetComponentInParent<PlayerLifeSystemScript>() == null)==ignorePlayer)
            {
                lss = c.GetComponentInParent<LifeSystemScript>();
                if (!lsList.Contains(lss))
                {
                    lsList.Add(lss);
                    if (minDist > (targetLS.transform.position - h.transform.position).magnitude)
                    {
                        currentHit = lss;
                        minDist = (targetLS.transform.position - h.transform.position).magnitude;

                    }
                }
            }
        }
        return currentHit;

    }

    LifeSystemScript GetClosestTarget(RaycastHit hit)
    {
        LifeSystemScript currentHit = null;
        float minDist = Mathf.Infinity;
        LifeSystemScript lss;

        //foreach (RaycastHit h in hits)
        //{
        Collider c = hit.collider;
        if (tagList.Contains(c.tag) && c.GetComponentInParent<LifeSystemScript>() != null)
        {
            lss = c.GetComponentInParent<LifeSystemScript>();
            if (!lsList.Contains(lss) && minDist > (targetLS.transform.position - hit.transform.position).magnitude)
            {
                currentHit = lss;
                minDist = (targetLS.transform.position - hit.transform.position).magnitude;

            }
        }
        //}
        return currentHit;


    }

    Queue<LifeSystemScript> SphereCastShock(LifeSystemScript currentTarget)
    {

        Queue<LifeSystemScript> lsQueue = new Queue<LifeSystemScript>();
        LifeSystemScript lss;
        RaycastHit[] hits = Physics.SphereCastAll(currentTarget.transform.position, effectPotency - 1, currentTarget.transform.forward, effectPotency, layerMask);
        foreach (RaycastHit h in hits)
        {
            Collider c = h.collider;
            if (tagList.Contains(c.tag) && c.GetComponentInParent<LifeSystemScript>() != null)
            {
                lss = c.GetComponentInParent<LifeSystemScript>();
                if (!lsList.Contains(lss))
                {
                    lsList.Add(lss);
                    lsQueue.Enqueue(lss);
                    ActiveShockOnTarget(currentTarget, lss as LifeSystemScript);

                }
            }
        }
        return lsQueue;
    }

}
