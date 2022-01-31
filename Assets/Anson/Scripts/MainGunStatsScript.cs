using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class MainGunStatsScript : GunStatsScript
{
    [Header("Gun Effects")]
    [SerializeField]
    VisualEffect rarityEffect;

    [Header("Gun Property")]
    [SerializeField]
    GunTypes gunType = GunTypes.RIFLE;

    [SerializeField]
    Rarity rarity;

    [SerializeField]
    ElementTypes elementType = ElementTypes.PHYSICAL;

    [SerializeField]
    FireTypes fireType = FireTypes.HitScan;

    [SerializeField]
    GameObject projectileGo;

    [SerializeField]
    protected AnimationCurve rangeCurve;

    [SerializeField]
    bool isFullAuto = true;

    [SerializeField]
    int projectilePerShot;

    [SerializeField]
    float timeBetweenProjectile = 0f;

    [SerializeField]
    bool isFullReload = true;

    [SerializeField]
    int amountPerReload = 1;

    [SerializeField]
    AnimationCurve recoilPatternX;

    [SerializeField]
    AnimationCurve recoilPatternY;

    [SerializeField]
    float timeToRecenter = 3f;

    [SerializeField]
    Transform sightLocation;

    [SerializeField]
    Vector3 sightOffset;


    [Header("Effects")]
    [SerializeField]
    private GunEffectsController gunEffectsController;


    [SerializeField]
    ParticleSystem bulletParticle;

    [SerializeField]
    GameObject impactEffect;

    [SerializeField]
    VisualEffect muzzleEffect;

    [Header("Connected Components")]
    [SerializeField]
    GunComponent_Body gunComponentBody;

    [SerializeField]
    GunComponent_Sight componentSight;

    [SerializeField]
    List<GunComponent> components;

    [Header("Animator")]
    [SerializeField]
    Animator animator;

    public Animator Animator => animator;

    public float ShootAnimationLerp => shootAnimationLerp;

    [SerializeField]
    float shootAnimationLerp = 1;


    [Header("Sound")]
    SoundManager soundManager;

    [SerializeField]
    Sound soundFire;

    [SerializeField]
    Sound soundStartReload;

    [SerializeField]
    Sound soundEndReload;

    [Header("Saved Stats")]
    [SerializeField]
    float currentMag;

    [SerializeField]
    private bool isEquiped = false;

    public bool IsEquiped
    {
        get => isEquiped;
        set => isEquiped = value;
    }

    public VisualEffect RarityEffect => rarityEffect;

    public Sound SoundFire => soundFire;

    public Sound SoundStartReload => soundStartReload;

    public Sound SoundEndReload => soundEndReload;

    public int ProjectilePerShot
    {
        get => projectilePerShot;
    }

    public float TimeBetweenProjectile
    {
        get => timeBetweenProjectile;
    }

    public float CurrentMag
    {
        get => currentMag;
        set => currentMag = value;
    }

    public ElementTypes ElementType
    {
        get => elementType;
    }

    public GunTypes GunType
    {
        get => gunType;
    }

    public bool IsFullAuto
    {
        get => isFullAuto;
        set => isFullAuto = value;
    }

    public int AmountPerReload
    {
        get => amountPerReload;
    }

    public bool IsFullReload
    {
        get => isFullReload;
    }

    public ParticleSystem BulletParticle
    {
        get => bulletParticle;
    }

    public VisualEffect MuzzleEffect
    {
        get => muzzleEffect;
        set => muzzleEffect = value;
    }

    public GameObject ImpactEffect
    {
        get => impactEffect;
        set => impactEffect = value;
    }

    public AnimationCurve RecoilPatternX
    {
        get => recoilPatternX;
    }

    public AnimationCurve RecoilPatternY
    {
        get => recoilPatternY;
    }

    public float TimeToRecenter
    {
        get => timeToRecenter;
        set => timeToRecenter = value;
    }

    public Transform SightLocation
    {
        get => sightLocation;
        set => sightLocation = value;
    }

    public Vector3 SightOffset
    {
        get => sightOffset;
        set => sightOffset = value;
    }

    public GunComponent_Sight ComponentSight
    {
        get => componentSight;
    }

    public FireTypes FireType
    {
        get => fireType;
        set => fireType = value;
    }

    public GameObject ProjectileGo
    {
        get => projectileGo;
        set => projectileGo = value;
    }

    public AnimationCurve RangeCurve
    {
        get => rangeCurve;
    }

    public Rarity Rarity
    {
        get => rarity;
        set => rarity = value;
    }

    public GunComponent_Body GunComponentBody
    {
        get => gunComponentBody;
        set => gunComponentBody = value;
    }

    public GunEffectsController GunEffectsController
    {
        get => gunEffectsController;
    }

    private void Start()
    {
        if (gunComponentBody != null)
        {
            SetBody(gunComponentBody);
        }
    }

    public void SetBody(GunComponent_Body b)
    {
        try
        {
            name = b.name.Substring(0, b.name.IndexOf("_"));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        soundManager = FindObjectOfType<SoundManager>();
        gunComponentBody = b;

        gunType = b.GTypes[0];
        rarity = b.Rarity;
        recoilPatternX = b.RecoilPattern_X;
        recoilPatternY = b.RecoilPattern_Y;
        timeToRecenter = b.TimeToRecenter;
        isFullAuto = b.IsFullAuto;
        bulletParticle = b.BulletParticle;
        projectilePerShot = b.ProjectilePerShot;
        impactEffect = b.ImpactEffect;
        muzzleEffect = b.MuzzleEffect;
        timeBetweenProjectile = b.TimeBetweenProjectile;
        currentMag = magazineSize;
        sightLocation = b.SightLocation;
        sightOffset = b.SightOffset;
        animator = b.GetAnimator;
        shootAnimationLerp = b.ShootAnimationLerp;
        soundFire = b.Sound_Fire;
        soundStartReload = b.Sound_StartReload;
        soundEndReload = b.Sound_EndReload;
        isFullReload = b.IsFullReload;
        amountPerReload = b.AmountPerReload;
        componentSight = b.Component_Sight;
        fireType = b.FireType;
        elementType = b.ElementType;
        projectileGo = b.ProjectileGO;
        rangeCurve = b.RangeCurve;
        gunEffectsController = b.GunEffectsController;

        //b.transform.rotation = Quaternion.Euler(0, -90, 0) * transform.rotation;

        //Rarity effect


    }


    public override void AddStats(GunStatsScript g)
    {
        base.AddStats(g);
    }

    public override void AddStats(ComponentGunStatsScript g)
    {
        base.AddStats(g);
    }

    public void FinishAssembly()
    {
        if (recoil.x < 0)
        {
            recoil.x = 0;
        }

        if (recoil.y < 0)
        {
            recoil.y = 0;
        }

        transform.position += transform.forward;
        GetComponentInChildren<Rigidbody>().isKinematic = false;
        GetComponentInChildren<Rigidbody>().AddForce(transform.up * 1000f);
        transform.parent = null;
        gunEffectsController.Initialise(this);
        gunEffectsController.SetEffectsElement(60f / RPM,elementType);
    }

    /// <summary>
    /// Depreciated 
    /// </summary>
    /// <param name="s"></param>
    /// <param name="animationSpeed"></param>
    [Obsolete("Moved to GunEffectController", true)]
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

    [Obsolete("Moved to GunEffectController", true)]
    public void Play_Fire()
    {
        if (soundFire != null)
        {
            soundFire.PlayF();
        }
    }

    [Obsolete("Moved to GunEffectController", true)]
    public void Play_StartReload()
    {
        if (soundStartReload != null)
        {
            soundStartReload.PlayF();
        }
    }

    [Obsolete("Moved to GunEffectController", true)]
    public void Play_EndReload()
    {
        if (soundEndReload != null)
        {
            soundEndReload.PlayF();
        }
    }

    [Obsolete("MOved to GunEffectController", true)]
    public void StopAnimation()
    {
        animator.StopPlayback();
    }

    [Obsolete("MOved to GunEffectController", true)]
    public void PlayGunShootEffect()
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

    public override string ToString()
    {
        string returnString = string.Concat(
            "<b>", name, "</b>", "\n",
            rarity.ToString(), " ", gunType.ToString(), "\n",
            "DPS: ", CalculateDps().ToString("0"), " Dmg/Sec.", "\n",
            "Damage: ", damagePerProjectile.ToString("0"), " x ", projectilePerShot, "\n",
            "RPM: ", RPM.ToString("0"), "\n",
            "Recoil: ", recoil.ToString(), "\n",
            "Hip Fire: ", recoil_HipFire.ToString(), "\n",
            "Mag: ", magazineSize, "\n",
            "Reload: ", ReloadSpeed, "\n",
            "Range: ", range, "\n",
            "Element: ", elementType.ToString(), "\n",
            (elementDamage * damagePerProjectile).ToString("0"), " Dmg, ", (elementChance * 100f).ToString("0"), "%, ",
            elementPotency, " Pow."
        );
        return returnString;
    }

    public string GetName()
    {
        return name;
    }

    public void SetRarityEffect(bool b)
    {
        gunEffectsController.SetRarityEffect(b);
    }

    public float CalculateDps()
    {
        float dps = (1 / ((60f / RPM) * magazineSize + reloadSpeed)) * damagePerProjectile * projectilePerShot *
                    magazineSize;
        if (elementType != ElementTypes.PHYSICAL)
        {
            dps = dps * UniversalValues.ElementDamageNerf;
        }

        dps += (1 / ((60f / RPM) * magazineSize + reloadSpeed)) * damagePerProjectile * elementDamage * elementChance *
               UniversalValues.GetDamageMultiplier(elementType) * projectilePerShot * magazineSize;
        /*
        switch (elementType)
        {
            case ElementTypes.PHYSICAL:
                break;
            case ElementTypes.FIRE:
                break;
            case ElementTypes.ICE:
                break;
            case ElementTypes.SHOCK:
                break;
        }
        */
        return dps;
    }

    public void ResetToWorldLoot()
    {
        transform.position += transform.forward;
        GetComponentInChildren<Rigidbody>().isKinematic = false;
        GetComponentInChildren<Rigidbody>().AddForce(transform.up * 1000f);
        gameObject.transform.parent = null;
        SetRarityEffect(true);
    }
}