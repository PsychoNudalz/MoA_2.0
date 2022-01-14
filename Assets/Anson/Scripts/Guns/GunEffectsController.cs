using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunEffectsController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField]
    Animator animator;

    [SerializeField]
    float shootAnimationLerp = 1;

    [SerializeField]
    GunHandController gunHandController;

    [Space(5)]
    [Header("Effects")]
    [SerializeField]
    ParticleSystem bulletParticle;

    [SerializeField]
    GameObject impactEffect;

    [SerializeField]
    VisualEffect muzzleEffect;

    [SerializeField]
    private ParticleSystem bulletCaseParticle;

    [Header("Bullet Trail")]
    [SerializeField]
    private BulletTrailControllerScript bulletTrailControllerScript;

    [SerializeField]
    private List<RaycastHit> bulletTrailCache_RaycastHit = new List<RaycastHit>();

    [SerializeField]
    private List<Vector3> bulletTrailCache_FireDir = new List<Vector3>();

    [Space(5)]
    [Header("Sound")]
    [SerializeField]
    Sound sound_Fire;

    [SerializeField]
    Sound sound_StartReload;

    [SerializeField]
    Sound sound_EndReload;

    [Header("Components")]
    [SerializeField]
    private MainGunStatsScript mainGunStat;

    public GunHandController GunHandController => gunHandController;

    private void Awake()
    {
        if (!bulletTrailControllerScript)
        {
            bulletTrailControllerScript = GetComponentInChildren<BulletTrailControllerScript>();
        }

        if (!gunHandController)
        {
            gunHandController = GetComponentInChildren<GunHandController>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Initialise(MainGunStatsScript gunStat)
    {
        this.mainGunStat = gunStat;
        SetEffectsElement(60f / gunStat.GetRPM, gunStat.ElementType);
    }

    public void SetEffectsElement(float timeBetweenShots, ElementTypes elementType)
    {
        muzzleEffect.SetInt("ElementEnum", (int) elementType);
        if (bulletTrailControllerScript)
        {
            bulletTrailControllerScript.InitialiseTrails((int) elementType, timeBetweenShots);
        }
    }

    public void PlayAnimationTrigger(string s, float animationSpeed = 1)
    {
        if (animator == null)
        {
            return;
        }

        if (s.Equals("Shoot"))
        {
            animator.speed = Mathf.Lerp(1, animationSpeed, shootAnimationLerp);
        }
        else
        {
            animator.speed = animationSpeed;
        }

        animator.SetTrigger(s);
    }


    public void PlaySound_Fire()
    {
        if (sound_Fire != null)
        {
            sound_Fire.PlayF();
        }
    }

    public void PlaySound_StartReload()
    {
        if (sound_StartReload != null)
        {
            sound_StartReload.PlayF();
        }
    }

    public void PlaySound_EndReload()
    {
        if (sound_EndReload != null)
        {
            sound_EndReload.PlayF();
        }
    }

    public void StopAnimation()
    {
        animator.StopPlayback();
    }


    public void PlayEffect_GunShoot()
    {
        if (bulletParticle != null)
        {
            bulletParticle.Play();
        }

        if (muzzleEffect != null)
        {
            muzzleEffect.Play();
        }
    }

    /// <summary>
    /// Called on animator
    /// </summary>
    /// <param name="notEjectCase"></param>
    public void PlayGunShootEffect(int notEjectCase = 0)
    {
        try
        {
            //Debug.Log(name + " play muzzle");

            bulletParticle.Play();
            muzzleEffect.Play();

            if (mainGunStat.GunType != GunTypes.SHOTGUN)
            {
                if (notEjectCase == 0)
                {
                    bulletCaseParticle.Play();
                }
            }


            PlayBulletTrails();
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogWarning(name + " Missing shoot effect");
            Debug.Log(e.StackTrace.Substring(0, 60));
        }
    }

    public void PlayBulletTrails()
    {
        if (!bulletTrailControllerScript)
        {
            return;
        }

        if (bulletTrailCache_RaycastHit.Count > 0)
        {
            bulletTrailControllerScript.PlayTrail(bulletTrailCache_RaycastHit);
        }
        else if (bulletTrailCache_FireDir.Count > 0)
        {
            bulletTrailControllerScript.PlayTrail(bulletTrailCache_FireDir);
        }

        WipeBulletTrailCache();
    }

    public void SetBulletTrail(List<RaycastHit> raycastHits, List<Vector3> fireDirs = null)
    {
        bulletTrailCache_RaycastHit = raycastHits;
        if (fireDirs == null)
        {
            bulletTrailCache_FireDir = fireDirs;
        }
    }

    public void AddBulletTrail(RaycastHit raycastHit)
    {
        bulletTrailCache_RaycastHit.Add(raycastHit);
    }

    public void AddBulletTrail(Vector3 fireDir)
    {
        bulletTrailCache_FireDir.Add(fireDir);
    }

    public void WipeBulletTrailCache()
    {
        bulletTrailCache_FireDir = new List<Vector3>();
        bulletTrailCache_RaycastHit = new List<RaycastHit>();
    }

    public void AddPointLeft(int i)
    {
        gunHandController.AddPoint_Left(i);
    }

    public void RemovePointLeft(int i)
    {
        gunHandController.RemovePoint_Left(i);
    }

    public void SetHandOnBarrel(GunComponent_Barrel b, bool ignore)
    {
        if (gunHandController && !ignore)
        {
            if (b.Hpp_Left)
            {
                gunHandController.SetNewRestPoint_Left(b.Hpp_Left);
            }

            if (b.Hpp_Right)
            {
            }
        }
    }

    /// <summary>
    /// used for getting things from body components
    /// </summary>
    /// <param name="b"></param>
    public void GetBodyData(GunComponent_Body b)
    {
        bulletParticle = b.BulletParticle;
        impactEffect = b.ImpactEffect;
        muzzleEffect = b.MuzzleEffect;
        animator = b.GetAnimator;
        shootAnimationLerp = b.ShootAnimationLerp;
        gunHandController = b.GunHandController;
        bulletParticle = b.BulletParticle;
        impactEffect = b.ImpactEffect;
        muzzleEffect = b.MuzzleEffect;
        bulletCaseParticle = b.BulletCaseParticle;
        sound_Fire = b.Sound_Fire;
        sound_StartReload = b.Sound_StartReload;
        sound_EndReload = b.Sound_EndReload;
    }
}