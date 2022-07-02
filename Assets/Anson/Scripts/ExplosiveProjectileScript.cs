using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectileScript : ProjectileScript
{
    [Header("Explosive Stats")]
    [SerializeField]
    float damageMultiplier;

    [SerializeField]
    float maxRange;

    [SerializeField]
    AnimationCurve rangeFalloff;

    [SerializeField]
    SphereCastDamageScript sphereCastDamageScript;


    public override void Explode()
    {
        sphereCastDamageScript.SphereCastDamageArea(BaseDamage * damageMultiplier, maxRange, rangeFalloff, Level,
            ElementType, true, TriggerElement,ElementDamage,ElementPotency, GunPerkController);
        if (this.GunPerkController)
        {
            this.GunPerkController.OnProjectile_Explode(this.ShotData);
        }
        base.Explode();
        Destroy(gameObject,Time.deltaTime);
    }
}