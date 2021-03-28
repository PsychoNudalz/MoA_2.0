using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunComponent_Body : GunComponent
{
    [Header("Extra")]

    [SerializeField] protected bool isSetElement = false;
    [SerializeField] protected ElementTypes elementType;
    [SerializeField] protected Rarity rarity;
    [SerializeField] protected AnimationCurve rangeCurve;


    [Header("Effects")]
    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] GameObject impactEffect;
    [SerializeField] VisualEffect muzzleEffect;
    [SerializeField] ParticleSystem bulletCaseParticle;

    [Header("Recoil")]
    [SerializeField] AnimationCurve recoilPattern_X;
    [SerializeField] AnimationCurve recoilPattern_Y;
    [SerializeField] float timeToRecenter = 3f;

    [Header("Fire Type")]
    [SerializeField] FireTypes fireType = FireTypes.HitScan;
    [SerializeField] GameObject projectileGO;
    [SerializeField] int projectilePerShot = 1;
    [SerializeField] float timeBetweenProjectile = 0f;
    [SerializeField] bool isFullAuto = true;
    [SerializeField] bool isFullReload = true;
    [SerializeField] int amountPerReload = 1;

    [Header("Component")]
    [SerializeField] GunComponent_Sight component_Sight;
    [SerializeField] Transform sightLocation;
    [SerializeField] Vector3 sightOffset;
    [SerializeField] Transform muzzleLocation;

    [Header("Animator")]
    [SerializeField] Animator animator;


    [Header("Sound")]
    [SerializeField] Sound sound_Fire;
    [SerializeField] Sound sound_StartReload;
    [SerializeField] Sound sound_EndReload;




    //Getters
    public float TimeBetweenProjectile { get => timeBetweenProjectile; }
    public bool IsFullAuto { get => isFullAuto; set => isFullAuto = value; }
    public AnimationCurve RecoilPattern_X { get => recoilPattern_X; }
    public AnimationCurve RecoilPattern_Y { get => recoilPattern_Y; }
    public ParticleSystem BulletParticle { get => BulletParticle1; }
    public int ProjectilePerShot { get => projectilePerShot; }
    public float TimeToRecenter { get => timeToRecenter; }
    public Transform SightLocation { get => sightLocation; }
    public ParticleSystem BulletParticle1 { get => bulletParticle; }
    public VisualEffect MuzzleEffect { get => muzzleEffect; }
    public GameObject ImpactEffect { get => impactEffect; }
    public Animator GetAnimator { get => animator; }
    public Sound Sound_Fire { get => sound_Fire; }
    public Sound Sound_StartReload { get => sound_StartReload; }
    public Sound Sound_EndReload { get => sound_EndReload; }
    public int AmountPerReload { get => amountPerReload; }
    public bool IsFullReload { get => isFullReload; }
    public GunComponent_Sight Component_Sight { get => component_Sight; }
    public FireTypes FireType { get => fireType; set => fireType = value; }
    public GameObject ProjectileGO { get => projectileGO; set => projectileGO = value; }
    public AnimationCurve RangeCurve { get => rangeCurve; }
    public ElementTypes ElementType { get => elementType; }
    public Vector3 SightOffset { get => sightOffset; set => sightOffset = value; }
    public Rarity Rarity { get => rarity; set => rarity = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (component_Sight != null)
        {
            SetSight(component_Sight);
        }
    }

    public void SetSight(GunComponent_Sight s)
    {
        component_Sight = s;
        sightLocation = s.SightLocation;
        sightOffset = sightLocation.position - transform.position;
    }

    public void SetMuzzle(Transform m)
    {
        muzzleLocation = m;
        muzzleEffect.transform.position = muzzleLocation.position;
    }

    public void SetProjectile(GameObject g)
    {
        if (fireType == FireTypes.Projectile)
        {
            projectileGO = g;
        }
    }

    public void PlayGunShootEffect()
    {
        try
        {
            //Debug.Log(name + " play muzzle");

            bulletParticle.Play();
            muzzleEffect.SetInt("ElementEnum", (int)elementType);
            muzzleEffect.Play();
            
            if (GTypes[0] != GunTypes.SHOTGUN)
            {
                bulletCaseParticle.Play();
            }
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogWarning(name + " Missing shoot effect");
        }
    }

    public void PlayBulletCaseEffect()
    {
        try
        {
            bulletCaseParticle.Play();
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogWarning(name + " Missing shoot effect");
        }
    }

    public void SetElement(ElementTypes e)
    {
        if (!isSetElement)
        {
            elementType = e;

        }
    }

}
