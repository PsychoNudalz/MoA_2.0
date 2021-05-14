using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastDamageScript : DamageScript
{
    [SerializeField] float lineOfSightOffset = 0f;
    public bool SphereCastDamageArea(float dmg, float range, AnimationCurve rangeCurve, int level, ElementTypes elementType, bool needLineOfSight = false)
    {
        attackedTargets = new List<LifeSystemScript>();
        bool hitTarget = false;
        bool shockFlag = false;
        int calculatedDamage;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, transform.forward, range, layerMask);
        foreach (RaycastHit h in hits)
        {
            Collider c = h.collider;
            print("Check line of sight to:" + c.name);

            if (!needLineOfSight || rayCastLineOfSight(c, range))
            {


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
            }
            //print(tagList.Contains(c.tag));
        }

        return hitTarget;
    }

    int CalculateDamage(float dmg, float range, AnimationCurve rangeCurve, Vector3 pos)
    {
        return Mathf.RoundToInt(dmg * rangeCurve.Evaluate((pos - transform.position).magnitude / range)) + 1;
    }

    bool rayCastLineOfSight(Collider c, float range)
    {
        Vector3 targetOffset = new Vector3(0, 0.5f, 0);
        Vector3 dir = (c.transform.position+targetOffset - transform.position).normalized;
        RaycastHit hit;
        Vector3 offset = dir * lineOfSightOffset;
        Debug.DrawRay(transform.position+offset, dir * range, Color.blue, 3f);
        if (Physics.Raycast(transform.position, dir, out hit, range, layerMask))
        {
            //print("Check line of sight found:" + hit.collider.name);
            if (hit.collider.Equals(c))
            {
                //print("have line of sight");
                return true;
            }
        }
        //print("no line of sight");

        return false;
    }
}
