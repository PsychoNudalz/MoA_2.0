using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectileScript : ProjectileScript
{
    [Header("Explosive Stats")]
    [SerializeField] float maxDamage;
    [SerializeField] float maxRange;
    [SerializeField] AnimationCurve rangeFalloff;
    [SerializeField] SphereCastDamageScript sphereCastDamageScript;


    public override void Explode()
    {
        base.Explode();
        sphereCastDamageScript.SphereCastDamageArea(maxDamage, maxRange, rangeFalloff, Level, ElementType);
        Destroy(gameObject);
    }
}
