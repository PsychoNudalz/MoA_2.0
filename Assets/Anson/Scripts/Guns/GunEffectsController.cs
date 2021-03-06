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
    private bool RPMChangeAnimationSpeed = true;

    [SerializeField]
    float shootAnimationLerp = 1;

    [SerializeField]
    GunHandController gunHandController;

    [SerializeField]
    private Coroutine delayAnimationCoroutine;

    [SerializeField]
    private bool noAnimationOnBurst;
    
    [SerializeField]
    private bool animationOnlyOnShoot = false;

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

    [Header("Gun Effects")]
    [SerializeField]
    VisualEffect rarityEffect;

    [Header("Components")]
    [SerializeField]
    private MainGunStatsScript mainGunStat;

    [SerializeField]
    private GunDamageScript gunDamageScript;

    public Animator Animator => animator;

    public GunHandController GunHandController => gunHandController;

    public float ShootAnimationLerp => shootAnimationLerp;

    public bool AnimationOnlyOnShoot => animationOnlyOnShoot;

    public bool NoAnimationOnBurst => noAnimationOnBurst;

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

    private void OnEnable()
    {
        if (mainGunStat)
        {
            Initialise(mainGunStat);
        }
    }

    public void Initialise(MainGunStatsScript gunStat)
    {
        this.mainGunStat = gunStat;
        rarityEffect = gunStat.RarityEffect;
        SetEffectsElement(60f / gunStat.GetRPM, gunStat.ElementType);
        rarityEffect.SetInt("Rarity", (int) gunStat.Rarity);
        rarityEffect.SetInt("Element", (int) gunStat.ElementType);

        updateAnimatorSpeeds();
    }

    
    public void SetGunDamage(GunDamageScript gunDamageScript)
    {
            this.gunDamageScript = gunDamageScript;
    }

    public void updateAnimatorSpeeds()
    {
        animator.SetFloat("ReloadSpeed", 1f / mainGunStat.ReloadSpeed);
        if (RPMChangeAnimationSpeed)
        {
            if (mainGunStat.GunType.Equals(GunTypes.SHOTGUN))
            {
                animator.SetFloat("ShootSpeed", 1f / ((60f / mainGunStat.GetRPM)));
            }
            else
            {
                animator.SetFloat("ShootSpeed", 1f / ((60f / mainGunStat.GetRPM) / mainGunStat.ProjectilePerShot));
            }
        }
    }

    public void updateAnimatorSpeeds(float r, float s)
    {
        animator.SetFloat("ReloadSpeed", 1f / r);
        if (RPMChangeAnimationSpeed)
        {
            if (mainGunStat.GunType.Equals(GunTypes.SHOTGUN))
            {
                animator.SetFloat("ShootSpeed", 1f / ((60f / s)));
            }
            else
            {
                animator.SetFloat("ShootSpeed", 1f / ((60f / s) / mainGunStat.ProjectilePerShot));
            }
        }
    }


    void Update()
    {
    }

    public void SetRarityEffect(bool b)
    {
        rarityEffect.gameObject.SetActive(b);
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

        if (Math.Abs(animator.speed - animationSpeed) > 0.01f)
        {
            delayAnimationCoroutine = StartCoroutine(DelayReloadCoroutine(s));
        }
        else
        {
            animator.SetTrigger(s);
        }


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
            PlayBulletTrails();

            if (mainGunStat.GunType != GunTypes.SHOTGUN)
            {
                if (notEjectCase == 0)
                {
                    bulletCaseParticle.Play();
                }
            }
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

        if (bulletTrailCache_FireDir.Count > 0)
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
        //Debug.Log($"Wiping trail cache {bulletTrailCache_FireDir.Count} {bulletTrailCache_RaycastHit.Count}");

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

    public void SetFireSound(Sound s)
    {
        sound_Fire = s;
    }

    IEnumerator DelayReloadCoroutine(string s)
    {
        yield return new WaitForFixedUpdate();
        animator.SetTrigger(s);
    }

    /// <summary>
    /// allow animation to call fire from animation
    /// </summary>
    public void AnimationCallShoot()
    {
        if (gunDamageScript)
        {
            gunDamageScript.ShootWeapon();
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

    [ContextMenu("Transfer Body")]
    public void GetBodyData()
    {
        GunComponent_Body temp = GetComponent<GunComponent_Body>();
        GetBodyData(temp);
    }

    [ContextMenu("Transfer Gun State")]
    public void GetStateData()
    {
        MainGunStatsScript b = GetComponent<MainGunStatsScript>();
        bulletParticle = b.BulletParticle;
        impactEffect = b.ImpactEffect;
        muzzleEffect = b.MuzzleEffect;
        animator = b.Animator;
        shootAnimationLerp = b.ShootAnimationLerp;
        bulletParticle = b.BulletParticle;
        impactEffect = b.ImpactEffect;
        muzzleEffect = b.MuzzleEffect;
        sound_Fire = b.SoundFire;
        sound_StartReload = b.SoundStartReload;
        sound_EndReload = b.SoundEndReload;
        rarityEffect = b.RarityEffect;
    }
}