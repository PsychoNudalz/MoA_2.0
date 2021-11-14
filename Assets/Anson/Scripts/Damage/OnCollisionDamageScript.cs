using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Anson:
/// Extends Damage Script
/// Deals damage on collisionm or on trigger
/// requires collider
/// </summary>
public class OnCollisionDamageScript : DamageScript
{
    [Header("Collision Behaviour")]
    public bool onTrigger;
    public bool onEnter = true;
    //public bool onStay;
    public float tickIntervals = 0.5f;
    float lastDamageTick;
    public float damage;
    public ElementTypes elementType;

    //public bool onExit;
    //public bool addToTargetsOnEnter = true;
    //public bool removeFromTargetsOnExit = true;

    private void FixedUpdate()
    {
        if (Time.time - lastDamageTick > tickIntervals)
        {
            DealDamageToTargets();
        }
        if (tickIntervals == 0)
        {
            DealDamageToTargets();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onTrigger && onEnter && tagList.Contains(other.gameObject.tag) && LifeSystemScript.GetLifeSystemScript(other.gameObject) != null)
        {
            AddTargetToList(other.gameObject.GetComponentInParent<LifeSystemScript>());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        LifeSystemScript ls = LifeSystemScript.GetLifeSystemScript(collision.gameObject);
        print("Collision: " + ls);
        if (!onTrigger && onEnter && tagList.Contains(collision.gameObject.tag) && LifeSystemScript.GetLifeSystemScript(collision.gameObject) != null)
        {
            AddTargetToList(collision.gameObject.GetComponentInParent<LifeSystemScript>());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!onTrigger && onEnter && tagList.Contains(collision.gameObject.tag) && LifeSystemScript.GetLifeSystemScript(collision.gameObject) != null)
        {
            LifeSystemScript ls = LifeSystemScript.GetLifeSystemScript(collision.gameObject);
            if (attackedTargets.Contains(ls))
            {
                attackedTargets.Remove(ls);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (onTrigger && onEnter && tagList.Contains(other.gameObject.tag) && LifeSystemScript.GetLifeSystemScript(other.gameObject) != null)
        {
            LifeSystemScript ls = LifeSystemScript.GetLifeSystemScript(other.gameObject);
            if (attackedTargets.Contains(ls))
            {
                attackedTargets.Remove(ls);
            }
        }
    }



    public void AddTargetToList(LifeSystemScript ls)
    {
        print("Add: " + ls);
        if (attackedTargets.Contains(ls))
        {
            return;
        }
        attackedTargets.Add(ls);
        dealDamageToTarget(ls, damage, -1, elementType);
    }

    void DealDamageToTargets()
    {
        foreach (LifeSystemScript ls in attackedTargets)
        {
            if (ls != null)
            {
                dealDamageToTarget(ls, damage * ((Time.time - lastDamageTick) / tickIntervals), -1, elementType);
            }
        }
        while (attackedTargets.Contains(null))
        {

        attackedTargets.Remove(null);
        }
        lastDamageTick = Time.time;
    }






}
