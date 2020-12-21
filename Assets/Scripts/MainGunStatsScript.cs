using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGunStatsScript : GunStatsScript
{
    [Header("Gun Property")]
    [SerializeField] GunTypes gunType = GunTypes.MID;
    [SerializeField] ElementTypes elementType = ElementTypes.PHYSICAL;
    [SerializeField] bool isFullAuto = true;
    [SerializeField] int projectilePerShot;
    [SerializeField] float timeBetweenProjectile = 0f;

    [SerializeField] AnimationCurve recoilPattern_X;
    [SerializeField] AnimationCurve recoilPattern_Y;
    [SerializeField] float timeToRecenter = 3f;

    [SerializeField] Transform sightLocation;



    [Header("Effects")]
    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] GameObject impactEffect;
    [SerializeField] GameObject muzzleEffect;

    [Header("Connected Components")]
    [SerializeField] GunComponent_Body gunComponent_Body;
    [SerializeField] List<GunComponent> components;
    [Header("Saved Stats")]
    [SerializeField] float currentMag;

    public int ProjectilePerShot { get => projectilePerShot;}
    public float TimeBetweenProjectile { get => timeBetweenProjectile;}
    public float CurrentMag { get => currentMag; set => currentMag = value; }
    public ElementTypes ElementType { get => elementType;}
    public GunTypes GunType { get => gunType;}
    public bool IsFullAuto { get => isFullAuto; set => isFullAuto = value; }

    public ParticleSystem BulletParticle { get => bulletParticle; }
    public GameObject MuzzleEffect { get => muzzleEffect; set => muzzleEffect = value; }
    public GameObject ImpactEffect { get => impactEffect; set => impactEffect = value; }

    public AnimationCurve RecoilPattern_X { get => recoilPattern_X; }
    public AnimationCurve RecoilPattern_Y { get => recoilPattern_Y; }
    public float TimeToRecenter { get => timeToRecenter; set => timeToRecenter = value; }
    public Transform SightLocation { get => sightLocation; set => sightLocation = value; }

    public void SetBody(GunComponent_Body b)
    {
        gunComponent_Body = b;
        recoilPattern_X = b.RecoilPattern_X;
        recoilPattern_Y = b.RecoilPattern_Y;
        timeToRecenter = b.TimeToRecenter;
        isFullAuto = b.IsFullAuto;
        bulletParticle = b.BulletParticle;
        projectilePerShot = b.ProjectilePerShot;
        impactEffect = b.ImpactEffect;
        muzzleEffect = b.MuzzleEffect;
        timeBetweenProjectile = b.TimeBetweenProjectile;
        currentMag = magazineSize;
        sightLocation = b.SightLocation;
        //b.transform.rotation = Quaternion.Euler(0, -90, 0) * transform.rotation;
    }

    public override void AddStats(GunStatsScript g)
    {
        base.AddStats(g);
    }

    public override void AddStats(ComponentGunStatsScript g)
    {
        base.AddStats(g);
    }

    public void FinishAssemply()
    {
        if (recoil.x < 0)
        {
            recoil.x = 0; 
        }
        if (recoil.y < 0)
        {
            recoil.y = 0; 
        }
    }



}
