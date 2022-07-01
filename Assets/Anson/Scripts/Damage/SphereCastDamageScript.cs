using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastDamageScript : DamageScript
{
    [SerializeField]
    float lineOfSightOffset = 0f;

    public bool SphereCastDamageArea(float dmg, float range, AnimationCurve rangeCurve, int level,
        ElementTypes elementType, bool needLineOfSight = false)
    {
        if (elementType.Equals(ElementTypes.FIRE))
        {
            Debug.LogWarning("Something called Fire");
        }


        attackedTargets = new List<LifeSystemScript>();
        bool hitTarget = false;
        bool shockFlag = false;
        int calculatedDamage;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, transform.forward, range, layerMask);
        LifeSystemScript lss;
        foreach (RaycastHit h in hits)
        {
            Collider c = h.collider;
            if (!needLineOfSight || RayCastLineOfSight(c, range))
            {
                 lss = c.GetComponentInParent<LifeSystemScript>();

                if (tagList.Contains(c.tag) && lss)
                {
                    if (!attackedTargets.Contains(lss))
                    {
                        hitTarget = true;
                        attackedTargets.Add(lss);

                        calculatedDamage = CalculateDamage(dmg, range, rangeCurve, lss.GetCentreOfMass().position);
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
            }
            //print(tagList.Contains(c.tag));
        }

        return hitTarget;
    }

    int CalculateDamage(float dmg, float range, AnimationCurve rangeCurve, Vector3 pos)
    {
        return Mathf.RoundToInt(dmg * rangeCurve.Evaluate((pos - transform.position).magnitude / range)) + 1;
    }

    bool RayCastLineOfSight(Collider c, float range)
    {
        Vector3 dir = (c.transform.position - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, range, layerMask))
        {
            Debug.DrawRay(transform.position, dir * range, Color.blue, 3f);

            //print("Check line of sight found:" + hit.collider.name);
            if (hit.collider.Equals(c))
            {
                //print("have line of sight");
                return true;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, dir * range, Color.red, 3f);

        }
        //print("no line of sight");

        return false;
    }
}