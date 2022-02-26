using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointExplosionSphere : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PointExplosionLifeSystem pointExplosionLifeSystem;

    [SerializeField]
    private Perk_PointExplosion perkParent;

    [SerializeField]
    private EffectPlayer effectPlayer;

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

    [SerializeField]
    private float fuseTime = 1f;


    public bool IsPrimed => isPrimed;

    public PointExplosionLifeSystem PointExplosionLifeSystem => pointExplosionLifeSystem;

    private void Awake()
    {
        if (animator)
        {
            animator.SetFloat("FuseTime",1f/fuseTime);
        }
    }

    public void Explode(float dmg, int level)
    {
        if (isPrimed)
        {
            isPrimed = false;
            StartCoroutine(delayExplode(dmg, level));

            if (perkParent)
            {
                perkParent.OnDeactivatePerk();
            }
        }
    }

    public void SetPrime()
    {
        if (isPrimed)
        {
            return;
        }
        isPrimed = true;
        animator.SetTrigger("Reset");

    }

    IEnumerator delayExplode(float dmg, int level)
    {
        animator.SetTrigger("Explode");
        effectPlayer.PlayUE(0);
        yield return new WaitForSeconds(fuseTime);
        effectPlayer.PlayUE(1);

        explosiveDamage.SphereCastDamageArea(dmg * damageMultiplier, explosiveRange, explosiveCurve, level,
            ElementTypes.PHYSICAL);
        

    }
}
