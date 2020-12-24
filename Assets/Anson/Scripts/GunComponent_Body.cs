using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent_Body : GunComponent
{
    [Header("Extra")]
    [SerializeField] protected List<ElementTypes> elementTypes;
    [SerializeField] bool isFullAuto = true;

    [Header("Effects")]
    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] GameObject impactEffect;
    [SerializeField] GameObject muzzleEffect;

    [Header("Recoil")]
    [SerializeField] AnimationCurve recoilPattern_X;
    [SerializeField] AnimationCurve recoilPattern_Y;
    [SerializeField] float timeToRecenter = 3f;

    [Header("Fire Type")]
    [SerializeField] int projectilePerShot =1;
    [SerializeField] float timeBetweenProjectile = 0f;

    [Header("Sight Location")]
    [SerializeField] Transform sightLocation;

    [Header("Animator")]
    [SerializeField] Animator animator;


    [Header("Sound")]
    [SerializeField] Sound sound_Fire;
    [SerializeField] Sound sound_StartReload;
    [SerializeField] Sound sound_EndReload;




    //Getters
    public float TimeBetweenProjectile { get => timeBetweenProjectile; }
    public bool IsFullAuto { get => isFullAuto; set => isFullAuto = value; }
    public AnimationCurve RecoilPattern_X { get => recoilPattern_X;}
    public AnimationCurve RecoilPattern_Y { get => recoilPattern_Y;}
    public ParticleSystem BulletParticle { get => BulletParticle1;}
    public int ProjectilePerShot { get => projectilePerShot;}
    public float TimeToRecenter { get => timeToRecenter; set => timeToRecenter = value; }
    public Transform SightLocation { get => sightLocation; set => sightLocation = value; }
    public ParticleSystem BulletParticle1 { get => bulletParticle; set => bulletParticle = value; }
    public GameObject MuzzleEffect { get => muzzleEffect; set => muzzleEffect = value; }
    public GameObject ImpactEffect { get => impactEffect; set => impactEffect = value; }
    public Animator GetAnimator { get => animator; set => animator = value; }
    public Sound Sound_Fire { get => sound_Fire; set => sound_Fire = value; }
    public Sound Sound_StartReload { get => sound_StartReload; set => sound_StartReload = value; }
    public Sound Sound_EndReload { get => sound_EndReload; set => sound_EndReload = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSight(Transform s)
    {
        sightLocation = s;
    }

}
