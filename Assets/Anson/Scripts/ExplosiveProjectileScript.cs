using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectileScript : ProjectileScript
{
    [Header("Explosive Stats")]
    [SerializeField] float damageMultiplier;
    [SerializeField] float maxRange;
    [SerializeField] AnimationCurve rangeFalloff;
    [SerializeField] SphereCastDamageScript sphereCastDamageScript;


    public override void Explode()
    {
        if (ElementType.Equals(ElementTypes.FIRE))
        {
            Debug.LogWarning(this.name+" called Fire");
        }
        base.Explode();
        sphereCastDamageScript.SphereCastDamageArea(BaseDamage*damageMultiplier, maxRange, rangeFalloff, Level, ElementType, true);
        Destroy(gameObject);
    }
}
