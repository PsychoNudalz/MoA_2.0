using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Anson:
/// super class for handling dealing damage to life system
/// </summary>
public class DamageScript : MonoBehaviour
{
    [Header("Target")]
    [SerializeField]
    protected LayerMask layerMask;

    [SerializeField]
    protected List<string> tagList;

    [Header("Debug")]
    [SerializeField]
    protected List<LifeSystemScript> attackedTargets = new List<LifeSystemScript>();


    /// <summary>
    /// deals damage to a single target that has a LifeSystemScript
    /// </summary>
    /// <param name="ls"></param>
    public virtual bool dealDamageToTarget(LifeSystemScript ls, float dmg, int level, ElementTypes element)
    {
        try
        {
            ls.takeDamage(dmg, level, element);
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogError(e.StackTrace);
            return false;
        }

        return ls.IsDead;
    }

    /// <summary>
    /// deals damage to a single target that has a LifeSystemScript
    /// </summary>
    /// <param name="ls"></param>
    public virtual bool dealCriticalDamageToTarget(LifeSystemScript ls, float dmg, int level, ElementTypes element,
        float multiplier)
    {
         ls.takeDamageCritical(dmg, level, element, multiplier);
        
        return ls.IsDead;

    }

    public virtual bool ApplyElementEffect(LifeSystemScript ls, float elementDamage, float elementPotency,
        ElementTypes elementType)
    {
        bool isKill = false;
        switch (elementType)
        {
            case (ElementTypes.PHYSICAL):
                isKill = dealCriticalDamageToTarget(ls, elementDamage * UniversalValues.GetDamageMultiplier(elementType), 1,
                    elementType, 1);
                break;
            case (ElementTypes.FIRE):
                FireEffectScript newFireDebuff = new FireEffectScript();
                newFireDebuff.init(elementDamage * UniversalValues.GetDamageMultiplier(elementType), elementPotency,
                    tagList, layerMask);
                ls.ApplyDebuff(newFireDebuff);
                break;
            case (ElementTypes.ICE):
                IceEffectScript newIceDebuff = new IceEffectScript();
                newIceDebuff.init(elementDamage * UniversalValues.GetDamageMultiplier(elementType), elementPotency,
                    tagList, layerMask);
                ls.ApplyDebuff(newIceDebuff);
                break;
            case (ElementTypes.SHOCK):
                ShockEffectScript newShockDebuff = new ShockEffectScript();
                newShockDebuff.init(elementDamage * UniversalValues.GetDamageMultiplier(elementType), elementPotency,
                    tagList, layerMask);
                ls.ApplyDebuff(newShockDebuff);
                break;
        }

        return isKill;
    }
}