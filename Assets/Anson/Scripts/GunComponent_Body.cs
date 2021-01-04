using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunComponent_Body : GunComponent
{
    [Header("Extra")]
    [SerializeField] protected List<ElementTypes> elementTypes;


    [Header("Effects")]
    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] GameObject impactEffect;
    [SerializeField] VisualEffect muzzleEffect;

    [Header("Recoil")]
    [SerializeField] AnimationCurve recoilPattern_X;
    [SerializeField] AnimationCurve recoilPattern_Y;
    [SerializeField] float timeToRecenter = 3f;

    [Header("Fire Type")]
    [SerializeField] int projectilePerShot = 1;
    [SerializeField] float timeBetweenProjectile = 0f;
    [SerializeField] bool isFullAuto = true;
    [SerializeField] bool isFullReload = true;
    [SerializeField] int amountPerReload = 1;

    [Header("Component")]
    [SerializeField] GunComponent_Sight component_Sight;
    [SerializeField] Transform sightLocation;
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
    public GunComponent_Sight Component_Sight { get => component_Sight;}

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSight(GunComponent_Sight s)
    {
        component_Sight = s;
        sightLocation = s.SightLocation;
    }

    public void SetMuzzle(Transform m)
    {
        muzzleLocation = m;
        muzzleEffect.transform.position = muzzleLocation.position;
    }



}
