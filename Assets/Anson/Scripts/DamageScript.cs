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
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected List<string> tagList;
    [Header("Debug")]
    [SerializeField] protected List<LifeSystemScript> attackedTargets = new List<LifeSystemScript>();
    [SerializeField] float timeBetweenAttack_TimeNow = 0;

    private void Awake()
    {

    }

    private void FixedUpdate()
    {
        if (timeBetweenAttack_TimeNow > 0)
        {
            timeBetweenAttack_TimeNow -= Time.deltaTime;
        }
    }
    /// <summary>
    /// deals damage to a single target that has a LifeSystemScript
    /// </summary>
    /// <param name="ls"></param>
    public virtual void dealDamageToTarget(LifeSystemScript ls, float dmg, int level, ElementTypes element)
    {
        ls.takeDamage(dmg, level, element);
    }

    /// <summary>
    /// deals damage to a single target that has a LifeSystemScript
    /// </summary>
    /// <param name="ls"></param>
    public virtual void dealCriticalDamageToTarget(LifeSystemScript ls, float dmg, int level, ElementTypes element, float multiplier)
    {
        ls.takeDamageCritical(dmg, level, element,multiplier);
    }




}
