using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastDamageScript : DamageScript
{
    public bool SphereCastDamageArea(float dmg, float range, AnimationCurve rangeCurve, int level, ElementTypes elementType)
    {
        attackedTargets = new List<LifeSystemScript>();
        bool hitTarget = false;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, transform.forward, range, layerMask);
        foreach (RaycastHit h in hits)
        {
            Collider c = h.collider;
            if (tagList.Contains(c.tag) && c.GetComponentInParent<LifeSystemScript>() != null)
            {
                LifeSystemScript lss = c.GetComponentInParent<LifeSystemScript>();
                attackedTargets.Add(lss);
                hitTarget = true;
                dealDamageToTarget(lss, CalculateDamage(dmg,range,rangeCurve,c.transform.position), level, elementType);
            }
            //print(tagList.Contains(c.tag));
        }

        return hitTarget;
    }

    int CalculateDamage(float dmg, float range, AnimationCurve rangeCurve,Vector3 pos)
    {
        return Mathf.RoundToInt( dmg * rangeCurve.Evaluate((pos - transform.position).magnitude / range))+1;
    }
}
