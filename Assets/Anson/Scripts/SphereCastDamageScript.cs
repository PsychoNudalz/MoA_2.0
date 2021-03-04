using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastDamageScript : DamageScript
{
    public bool SphereCastDamageArea(float dmg, float range, AnimationCurve rangeCurve, int level, ElementTypes elementType)
    {
        attackedTargets = new List<LifeSystemScript>();
        bool hitTarget = false;
        bool shockFlag = false;
        int calculatedDamage;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, transform.forward, range, layerMask);
        foreach (RaycastHit h in hits)
        {
            Collider c = h.collider;
            if (tagList.Contains(c.tag) && c.GetComponentInParent<LifeSystemScript>() != null)
            {
                LifeSystemScript lss = c.GetComponentInParent<LifeSystemScript>();
                if (!attackedTargets.Contains(lss))
                {
                    hitTarget = true;
                    attackedTargets.Add(lss);
                    calculatedDamage = CalculateDamage(dmg, range, rangeCurve, c.transform.position);
                    dealDamageToTarget(lss, calculatedDamage, level, elementType);

                    if (!(lss is PlayerLifeSystemScript))
                    {

                        if (elementType.Equals(ElementTypes.SHOCK))
                        {
                            if (!shockFlag)
                            {
                                ApplyElementEffect(lss, calculatedDamage * .5f, range, elementType);
                                shockFlag = true;
                            }
                        }
                        else
                        {
                            ApplyElementEffect(lss, calculatedDamage * .5f, range, elementType);

                        }
                    }
                }
            }
            //print(tagList.Contains(c.tag));
        }

        return hitTarget;
    }

    int CalculateDamage(float dmg, float range, AnimationCurve rangeCurve, Vector3 pos)
    {
        return Mathf.RoundToInt(dmg * rangeCurve.Evaluate((pos - transform.position).magnitude / range)) + 1;
    }
}
