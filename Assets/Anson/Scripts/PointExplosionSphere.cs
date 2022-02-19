using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointExplosionSphere : MonoBehaviour
{
    [SerializeField]
    private PointExplosionLifeSystem pointExplosionLifeSystem;

    [SerializeField]
    private Perk_PointExplosion perkParent;

    [Header("Point Explosion")]
    [SerializeField]
    private bool isPrimed =false;
    
    [SerializeField]
    private SphereCastDamageScript explosiveDamage;

    [SerializeField]
    private float damageMultiplier = 2f;

    [SerializeField]
    private float explosiveRange = 5f;

    [SerializeField]
    private AnimationCurve explosiveCurve;


    public bool IsPrimed => isPrimed;

    public void Explode(float dmg, int level)
    {
        if (isPrimed)
        {
            explosiveDamage.SphereCastDamageArea(dmg * damageMultiplier, explosiveRange, explosiveCurve, level,
                ElementTypes.PHYSICAL);
            if (perkParent)
            {
                perkParent.OnDeactivatePerk();
            }

            isPrimed = false;
        }
    }

    public void SetPrime()
    {
        isPrimed = true;
    }
}
