using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class MainGunStatsScript : GunStatsScript
{
    [Header("Gun Property")]
    [SerializeField] GunTypes gunType = GunTypes.RIFLE;
    [SerializeField] Rarity rarity;
    [SerializeField] ElementTypes elementType = ElementTypes.PHYSICAL;
    [SerializeField] FireTypes fireType = FireTypes.HitScan;
    [SerializeField] GameObject projectileGO;

    [SerializeField] protected AnimationCurve rangeCurve;

    [SerializeField] bool isFullAuto = true;
    [SerializeField] int projectilePerShot;
    [SerializeField] float timeBetweenProjectile = 0f;
    [SerializeField] bool isFullReload = true;
    [SerializeField] int amountPerReload = 1;

    [SerializeField] AnimationCurve recoilPattern_X;
    [SerializeField] AnimationCurve recoilPattern_Y;
    [SerializeField] float timeToRecenter = 3f;

    [SerializeField] Transform sightLocation;
    [SerializeField] Vector3 sightOffset;



    [Header("Effects")]
    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] GameObject impactEffect;
    [SerializeField] VisualEffect muzzleEffect;

    [Header("Connected Components")]
    [SerializeField] GunComponent_Body gunComponent_Body;
    [SerializeField] GunComponent_Sight component_Sight;
    [SerializeField] List<GunComponent> components;

    [Header("Animator")]
    [SerializeField] Animator animator;

    [Header("Sound")]
    SoundManager soundManager;
    [SerializeField] Sound sound_Fire;
    [SerializeField] Sound sound_StartReload;
    [SerializeField] Sound sound_EndReload;

    [Header("Saved Stats")]
    [SerializeField] float currentMag;

    public int ProjectilePerShot { get => projectilePerShot; }
    public float TimeBetweenProjectile { get => timeBetweenProjectile; }
    public float CurrentMag { get => currentMag; set => currentMag = value; }
    public ElementTypes ElementType { get => elementType; }
    public GunTypes GunType { get => gunType; }
    public bool IsFullAuto { get => isFullAuto; set => isFullAuto = value; }
    public int AmountPerReload { get => amountPerReload; }
    public bool IsFullReload { get => isFullReload; }

    public ParticleSystem BulletParticle { get => bulletParticle; }
    public VisualEffect MuzzleEffect { get => muzzleEffect; set => muzzleEffect = value; }
    public GameObject ImpactEffect { get => impactEffect; set => impactEffect = value; }

    public AnimationCurve RecoilPattern_X { get => recoilPattern_X; }
    public AnimationCurve RecoilPattern_Y { get => recoilPattern_Y; }
    public float TimeToRecenter { get => timeToRecenter; set => timeToRecenter = value; }
    public Transform SightLocation { get => sightLocation; set => sightLocation = value; }
    public Vector3 SightOffset { get => sightOffset; set => sightOffset = value; }

    public GunComponent_Sight Component_Sight { get => component_Sight; }
    public FireTypes FireType { get => fireType; set => fireType = value; }
    public GameObject ProjectileGO { get => projectileGO; set => projectileGO = value; }

    public AnimationCurve RangeCurve { get => rangeCurve; }
    public Rarity Rarity { get => rarity; set => rarity = value; }

    private void Start()
    {
        if (gunComponent_Body != null)
        {
            SetBody(gunComponent_Body);
        }
    }

    public void SetBody(GunComponent_Body b)
    {
        name = b.name.Substring(0, b.name.IndexOf("_"));
        soundManager = FindObjectOfType<SoundManager>();
        gunComponent_Body = b;

        gunType = b.GTypes[0];
        rarity = b.Rarity;
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
        sightOffset = b.SightOffset;
        animator = b.GetAnimator;
        sound_Fire = b.Sound_Fire;
        sound_StartReload = b.Sound_StartReload;
        sound_EndReload = b.Sound_EndReload;
        isFullReload = b.IsFullReload;
        amountPerReload = b.AmountPerReload;
        component_Sight = b.Component_Sight;
        fireType = b.FireType;
        elementType = b.ElementType;
        projectileGO = b.ProjectileGO;
        rangeCurve = b.RangeCurve;
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

        transform.position += transform.forward;
        GetComponentInChildren<Rigidbody>().isKinematic = false;
        GetComponentInChildren<Rigidbody>().AddForce(transform.up * 1000f);
        transform.parent = null;
    }

    public void PlayAnimationTrigger(string s, float animationSpeed = 1)
    {
        if (animator == null)
        {
            return;
        }
        animator.SetTrigger(s);
        animator.speed = animationSpeed;

    }


    public void Play_Fire()
    {
        if (sound_Fire != null)
        {
            soundManager.Play(sound_Fire);
        }
    }

    public void Play_StartReload()
    {
        if (sound_StartReload != null)
        {
            soundManager.Play(sound_StartReload);
        }
    }
    public void Play_EndReload()
    {
        if (sound_EndReload != null)
        {
            soundManager.Play(sound_EndReload);
        }
    }

    public void StopAnimation()
    {
        animator.StopPlayback();
    }


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
            name, "\n",
            gunType.ToString(), " ", elementType.ToString(), "\n",
            "Rarity: ", rarity.ToString(), "\n",
            "Damage: ", damagePerProjectile, " x ", projectilePerShot, "\n",
            "RPM: ", RPM, " Recoil: ", recoil.ToString(), "\n",
            "Hip Fire: ", recoil_HipFire.ToString(), "\n",
            "Mag: ", magazineSize, " Reload Speed: ", ReloadSpeed, "\n",
            "Range: ", range, "\n",
            " Drop Offs: ", Mathf.Round(rangeCurve.Evaluate(0) * damagePerProjectile), ", ", Mathf.Round(rangeCurve.Evaluate(0.5f) * damagePerProjectile), ", ", Mathf.Round(rangeCurve.Evaluate(1) * damagePerProjectile)

            );
        return returnString;
    }

    public string GetName()
    {
        return name;
    }

}
