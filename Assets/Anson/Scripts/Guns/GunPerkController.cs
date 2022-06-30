using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunPerkController : MonoBehaviour
{
    [SerializeField]
    private bool isPlayerPerk = false;

    public bool IsPlayerPerk
    {
        get => isPlayerPerk;
        set => isPlayerPerk = value;
    }

    [SerializeField]
    private Perk[] perks = new Perk[] { };

    public Perk[] Perks => perks;

    [SerializeField]
    private bool isChange = false;

    [Space(20f)]
    [Header("Gun Stats Flat")]
    [SerializeField]
    protected float damagePerProjectile = 0;

    [SerializeField]
    protected float RPM = 0;

    [SerializeField]
    protected float reloadSpeed = 0;

    [SerializeField]
    protected Vector2 recoil = new Vector2(0, 0);

    [SerializeField]
    protected Vector2 recoil_HipFire = new Vector2(0, 0);

    [SerializeField]
    protected float range = 0;

    [SerializeField]
    protected float magazineSize = 0;

    [Space(5f)]
    [Range(0f, 2f)]
    [SerializeField]
    protected float elementDamage = 0;

    [SerializeField]
    protected float elementPotency = 0; //effect duration or range

    [Range(0f, 1f)]
    [SerializeField]
    protected float elementChance = 0;

    [Space(10f)]
    [Header("Gun Stats Multi")]
    [SerializeField]
    public float damagePerProjectileM = 0;

    [SerializeField]
    private float RPMM = 0;

    [SerializeField]
    private float reloadSpeedM = 0;

    [SerializeField]
    private Vector2 recoilM = new Vector2(0, 0);

    [SerializeField]
    private Vector2 hipfireM = new Vector2(0, 0);

    [SerializeField]
    private float rangeM = 0;

    [SerializeField]
    private float magazineSizeM = 0;

    [Space(10f)]
    [Header("Components")]
    [SerializeField]
    private MainGunStatsScript mainGunStatsScript;

    [SerializeField]
    private GunDamageScript gunDamageScript;

    public float DamagePerProjectile
    {
        get => damagePerProjectile;
        set => damagePerProjectile = value;
    }

    public float GetRPM
    {
        get => RPM;
        set => RPM = value;
    }

    public float ReloadSpeed
    {
        get => reloadSpeed;
        set => reloadSpeed = value;
    }

    public Vector2 Recoil
    {
        get => recoil;
        set => recoil = value;
    }

    public Vector2 Recoil_HipFire
    {
        get => recoil_HipFire;
        set => recoil_HipFire = value;
    }

    public float Range
    {
        get => range;
        set => range = value;
    }

    public float MagazineSize
    {
        get => magazineSize;
        set => magazineSize = value;
    }

    public float ElementDamage
    {
        get => elementDamage;
        set => elementDamage = value;
    }

    public float ElementPotency
    {
        get => elementPotency;
        set => elementPotency = value;
    }

    public float ElementChance
    {
        get => elementChance;
        set => elementChance = value;
    }

    public float DamagePerProjectileM => damagePerProjectileM;

    public float Rpmm => RPMM;

    public float ReloadSpeedM => reloadSpeedM;

    public Vector2 RecoilM => recoilM;

    public Vector2 HipfireM => hipfireM;

    public float RangeM => rangeM;

    public float MagazineSizeM => magazineSizeM;


    public void AddPerk(Perk p)
    {
        List<Perk> temp = new List<Perk>(perks);
        temp.Add(p);
        perks = temp.ToArray();
    }

    public void InitialisePerks()
    {
        List<Perk> temp = new List<Perk>();

        foreach (GunComponent_Perk gunComponentPerk in GetComponentsInChildren<GunComponent_Perk>())
        {
            temp.Add(gunComponentPerk.Perk);
        }

        perks = temp.ToArray();
    }

    public void InitialisePerks(GunDamageScript gunDamageScript, MainGunStatsScript mainGunStatsScript,
        GunStatsScript originalGunStatsScript)
    {
        foreach (Perk perk in perks)
        {
            perk.Initialise(gunDamageScript, mainGunStatsScript, originalGunStatsScript, this);
        }

        this.gunDamageScript = gunDamageScript;
        this.mainGunStatsScript = mainGunStatsScript;
    }

    public void InitialisePerkPlayerController(PlayerController playerController)
    {
        foreach (Perk perk in perks)
        {
            perk.SetPlayerController(playerController);
        }
    }

    public void SetPlayerPerk(bool b)
    {
        isPlayerPerk = b;
        foreach (Perk perk in perks)
        {
            perk.IsPlayerPerk = b;
        }
    }

    //Stat Combinations
    public void UndoStats()
    {
        gunDamageScript.RemovePerkStats(this);
        //gunDamageScript.ResetMainStats();
    }

    public void ApplyStats()
    {
        gunDamageScript.AddPerkStats(this);
    }

    public void UndoStats(ModifiedStat[] modifiedStats)
    {
        gunDamageScript.RemovePerkStats(this,modifiedStats);
        //gunDamageScript.ResetMainStats();
    }

    public void ApplyStats(ModifiedStat[] modifiedStats)
    {
        gunDamageScript.AddPerkStats(this,modifiedStats);
    }

    public void AddPerkStats(PerkGunStatsScript g)
    {
        if (g.ModifiedStats.Length > 0)
        {
            AddPerkStats(g,g.ModifiedStats);
            return;
        }
        UndoStats();
        damagePerProjectile += g.DamagePerProjectile;
        RPM += g.GetRPM;
        reloadSpeed += g.ReloadSpeed;
        recoil += g.Recoil;
        recoil_HipFire += g.Recoil_HipFire;
        range += g.Range;
        magazineSize += g.MagazineSize;
        elementDamage += g.ElementDamage;
        elementPotency += g.ElementPotency;
        elementChance += g.ElementChance;

        damagePerProjectileM += g.damagePerProjectileM - 1f;
        RPMM += g.RPMM - 1f;
        reloadSpeedM += g.reloadSpeedM - 1f;
        //recoil = recoil * g.recoilM;
        recoilM += new Vector2(g.recoilM.x - 1f, g.recoilM.y - 1f);
        hipfireM += new Vector2(g.hipfireM.x - 1f, g.hipfireM.y - 1f);
        rangeM += g.rangeM - 1f;
        magazineSizeM += g.magazineSizeM - 1f;

        ApplyStats();
    }

    /// <summary>
    /// Increase state based on original state
    /// </summary>
    /// <param name="g"></param>
    public void AddPerkStatsAdditive(PerkGunStatsScript g)
    {
        if (g.ModifiedStats.Length > 0)
        {
            AddPerkStatsAdditive(g,g.ModifiedStats);
            return;
        }
        UndoStats();
        damagePerProjectile += mainGunStatsScript.DamagePerProjectile * (g.damagePerProjectileM - 1);
        RPM += mainGunStatsScript.GetRPM * (g.RPMM - 1);
        reloadSpeed += mainGunStatsScript.ReloadSpeed * (g.reloadSpeedM - 1);
        //recoil = recoil * g.recoilM;
        recoil += new Vector2(mainGunStatsScript.Recoil.x * (g.recoilM.x - 1),
            mainGunStatsScript.Recoil.y * (g.recoilM.y - 1));
        recoil_HipFire += new Vector2(mainGunStatsScript.Recoil_HipFire.x * (g.hipfireM.x - 1),
            mainGunStatsScript.Recoil_HipFire.y * (g.hipfireM.y - 1));
        range += mainGunStatsScript.Range * (g.rangeM - 1);
        magazineSize += mainGunStatsScript.MagazineSize * (g.magazineSizeM - 1);
        //
        // if (GunType != GunTypes.SHOTGUN)
        // {
        //     recoil_HipFire.y = Mathf.Max(recoil_HipFire.y, recoil_HipFire.x);
        // }
        ApplyStats();
    }

    public void RemovePerkStats(PerkGunStatsScript g)
    {
        if (g.ModifiedStats.Length > 0)
        {
            RemovePerkStats(g,g.ModifiedStats);
            return;
        }
        UndoStats();
        damagePerProjectile -= g.DamagePerProjectile;
        RPM -= g.GetRPM;
        reloadSpeed -= g.ReloadSpeed;
        recoil -= g.Recoil;
        recoil_HipFire -= g.Recoil_HipFire;
        range -= g.Range;
        magazineSize -= g.MagazineSize;
        elementDamage -= g.ElementDamage;
        elementPotency -= g.ElementPotency;
        elementChance -= g.ElementChance;

        damagePerProjectileM -= g.damagePerProjectileM - 1f;
        RPMM -= g.RPMM - 1f;
        reloadSpeedM -= g.reloadSpeedM - 1f;
        //recoil = recoil * g.recoilM;
        recoilM -= new Vector2(g.recoilM.x - 1f, g.recoilM.y - 1f);
        hipfireM -= new Vector2(g.hipfireM.x - 1f, g.hipfireM.y - 1f);
        rangeM -= g.rangeM - 1f;
        magazineSizeM -= g.magazineSizeM - 1f;
        ApplyStats();
    }

    /// <summary>
    /// Decrease state based on original state
    /// </summary>
    /// <param name="g"></param>
    public void RemovePerkStatsAdditive(PerkGunStatsScript g)
    {
        if (g.ModifiedStats.Length > 0)
        {
            RemovePerkStatsAdditive(g,g.ModifiedStats);
            return;
        }
        UndoStats();
        damagePerProjectile -= mainGunStatsScript.DamagePerProjectile * (g.damagePerProjectileM - 1);
        RPM -= mainGunStatsScript.GetRPM * (g.RPMM - 1);
        reloadSpeed -= mainGunStatsScript.ReloadSpeed * (g.reloadSpeedM - 1);
        //recoil = recoil * g.recoilM;
        recoil -= new Vector2(mainGunStatsScript.Recoil.x * (g.recoilM.x - 1),
            mainGunStatsScript.Recoil.y * (g.recoilM.y - 1));
        recoil_HipFire -= new Vector2(mainGunStatsScript.Recoil_HipFire.x * (g.hipfireM.x - 1),
            mainGunStatsScript.Recoil_HipFire.y * (g.hipfireM.y - 1));
        range -= mainGunStatsScript.Range * (g.rangeM - 1);
        magazineSize -= mainGunStatsScript.MagazineSize * (g.magazineSizeM - 1);
        ApplyStats();
    }

    public void AddPerkStats(PerkGunStatsScript g, ModifiedStat[] modifiedStats)
    {
        UndoStats(modifiedStats);

        foreach (ModifiedStat modifiedStat in modifiedStats)
        {
            switch (modifiedStat)
            {
                case ModifiedStat.DAMAGE:
                    damagePerProjectile += g.DamagePerProjectile;

                    break;
                case ModifiedStat.RPM:
                    RPM += g.GetRPM;

                    break;
                case ModifiedStat.RELOAD:
                    reloadSpeed += g.ReloadSpeed;

                    break;
                case ModifiedStat.RECOIL:
                    recoil += g.Recoil;

                    break;
                case ModifiedStat.HIPFIRE:
                    recoil_HipFire += g.Recoil_HipFire;

                    break;
                case ModifiedStat.RANGE:
                    range += g.Range;

                    break;
                case ModifiedStat.MAGAZINE:
                    magazineSize += g.MagazineSize;

                    break;
                case ModifiedStat.EDAMAGE:
                    elementDamage += g.ElementDamage;

                    break;
                case ModifiedStat.EPOTENCY:
                    elementPotency += g.ElementPotency;

                    break;
                case ModifiedStat.ECHANCE:
                    elementChance += g.ElementChance;

                    break;
                case ModifiedStat.DAMAGE_M:
                    damagePerProjectileM += g.damagePerProjectileM - 1f;

                    break;
                case ModifiedStat.RPM_M:
                    RPMM += g.RPMM - 1f;

                    break;
                case ModifiedStat.RELOAD_M:
                    reloadSpeedM += g.reloadSpeedM - 1f;

                    break;
                case ModifiedStat.RECOIL_M:
                    recoilM += new Vector2(g.recoilM.x - 1f, g.recoilM.y - 1f);

                    break;
                case ModifiedStat.HIPFIRE_M:
                    hipfireM += new Vector2(g.hipfireM.x - 1f, g.hipfireM.y - 1f);

                    break;
                case ModifiedStat.RANGE_M:
                    rangeM += g.rangeM - 1f;

                    break;
                case ModifiedStat.MAGAZINE_M:
                    magazineSizeM += g.magazineSizeM - 1f;

                    break;
            }
        }


        ApplyStats(modifiedStats);
    }

    /// <summary>
    /// Increase state based on original state
    /// </summary>
    /// <param name="g"></param>
    public void AddPerkStatsAdditive(PerkGunStatsScript g, ModifiedStat[] modifiedStats)
    {
        UndoStats(modifiedStats);
        foreach (ModifiedStat modifiedStat in modifiedStats)
        {
            switch (modifiedStat)
            {
                case ModifiedStat.DAMAGE:
                    damagePerProjectile += mainGunStatsScript.DamagePerProjectile * (g.damagePerProjectileM - 1);

                    break;
                case ModifiedStat.RPM:
                    RPM += mainGunStatsScript.GetRPM * (g.RPMM - 1);

                    break;
                case ModifiedStat.RELOAD:
                    reloadSpeed += mainGunStatsScript.ReloadSpeed * (g.reloadSpeedM - 1);

                    break;
                case ModifiedStat.RECOIL:
                    recoil += new Vector2(mainGunStatsScript.Recoil.x * (g.recoilM.x - 1),
                        mainGunStatsScript.Recoil.y * (g.recoilM.y - 1));
                    break;
                case ModifiedStat.HIPFIRE:
                    recoil_HipFire += new Vector2(mainGunStatsScript.Recoil_HipFire.x * (g.hipfireM.x - 1),
                        mainGunStatsScript.Recoil_HipFire.y * (g.hipfireM.y - 1));
                    break;
                case ModifiedStat.RANGE:
                    range += mainGunStatsScript.Range * (g.rangeM - 1);

                    break;
                case ModifiedStat.MAGAZINE:
                    magazineSize += mainGunStatsScript.MagazineSize * (g.magazineSizeM - 1);

                    break;

            }
        }


        ApplyStats(modifiedStats);
    }

    public void RemovePerkStats(PerkGunStatsScript g, ModifiedStat[] modifiedStats)
    {
        UndoStats(modifiedStats);
        foreach (ModifiedStat modifiedStat in modifiedStats)
        {
            switch (modifiedStat)
            {
                case ModifiedStat.DAMAGE:
                    damagePerProjectile -= g.DamagePerProjectile;

                    break;
                case ModifiedStat.RPM:
                    RPM -= g.GetRPM;

                    break;
                case ModifiedStat.RELOAD:
                    reloadSpeed -= g.ReloadSpeed;

                    break;
                case ModifiedStat.RECOIL:
                    recoil -= g.Recoil;

                    break;
                case ModifiedStat.HIPFIRE:
                    recoil_HipFire -= g.Recoil_HipFire;

                    break;
                case ModifiedStat.RANGE:
                    range -= g.Range;

                    break;
                case ModifiedStat.MAGAZINE:
                    magazineSize -= g.MagazineSize;

                    break;
                case ModifiedStat.EDAMAGE:
                    elementDamage -= g.ElementDamage;

                    break;
                case ModifiedStat.EPOTENCY:
                    elementPotency -= g.ElementPotency;

                    break;
                case ModifiedStat.ECHANCE:
                    elementChance -= g.ElementChance;

                    break;
                case ModifiedStat.DAMAGE_M:
                    damagePerProjectileM -= g.damagePerProjectileM - 1f;

                    break;
                case ModifiedStat.RPM_M:
                    RPMM -= g.RPMM - 1f;

                    break;
                case ModifiedStat.RELOAD_M:
                    reloadSpeedM -= g.reloadSpeedM - 1f;

                    break;
                case ModifiedStat.RECOIL_M:
                    recoilM -= new Vector2(g.recoilM.x - 1f, g.recoilM.y - 1f);

                    break;
                case ModifiedStat.HIPFIRE_M:
                    hipfireM -= new Vector2(g.hipfireM.x - 1f, g.hipfireM.y - 1f);

                    break;
                case ModifiedStat.RANGE_M:
                    rangeM -= g.rangeM - 1f;

                    break;
                case ModifiedStat.MAGAZINE_M:
                    magazineSizeM -= g.magazineSizeM - 1f;

                    break;
            }
        }

        ApplyStats(modifiedStats);
    }

    /// <summary>
    /// Decrease state based on original state
    /// </summary>
    /// <param name="g"></param>
    public void RemovePerkStatsAdditive(PerkGunStatsScript g, ModifiedStat[] modifiedStats)
    {
        UndoStats(modifiedStats);
        foreach (ModifiedStat modifiedStat in modifiedStats)
        {
            switch (modifiedStat)
            {
                case ModifiedStat.DAMAGE:
                    damagePerProjectile -= mainGunStatsScript.DamagePerProjectile * (g.damagePerProjectileM - 1);

                    break;
                case ModifiedStat.RPM:
                    RPM -= mainGunStatsScript.GetRPM * (g.RPMM - 1);

                    break;
                case ModifiedStat.RELOAD:
                    reloadSpeed -= mainGunStatsScript.ReloadSpeed * (g.reloadSpeedM - 1);

                    break;
                case ModifiedStat.RECOIL:
                    recoil -= new Vector2(mainGunStatsScript.Recoil.x * (g.recoilM.x - 1),
                        mainGunStatsScript.Recoil.y * (g.recoilM.y - 1));
                    break;
                case ModifiedStat.HIPFIRE:
                    recoil_HipFire -= new Vector2(mainGunStatsScript.Recoil_HipFire.x * (g.hipfireM.x - 1),
                        mainGunStatsScript.Recoil_HipFire.y * (g.hipfireM.y - 1));
                    break;
                case ModifiedStat.RANGE:
                    range -= mainGunStatsScript.Range * (g.rangeM - 1);

                    break;
                case ModifiedStat.MAGAZINE:
                    magazineSize -= mainGunStatsScript.MagazineSize * (g.magazineSizeM - 1);

                    break;

            }
        }
        ApplyStats(modifiedStats);
    }


    //EVENTS

    public void OnShoot(ShotData shotData)
    {
        foreach (Perk perk in perks)
        {
            perk.OnShot(shotData);
            if (shotData.IsHit)
            {
                perk.OnHit(shotData);
            }
            else
            {
                perk.OnMiss(shotData);
            }

            if (shotData.IsKill)
            {
                perk.OnKill(shotData);
            }

            if (shotData.IsCritical)
            {
                perk.OnCritical(shotData);
            }

            if (shotData.IsElementTrigger)
            {
                perk.OnElementTrigger(shotData);
            }

            if (shotData.IsTargetHit)
            {
                perk.OnTargetHit(shotData);
            }
        }
    }

    public void OnReloadStart()
    {
        foreach (Perk perk in perks)
        {
            perk.OnReloadStart();
        }
    }

    public void OnReloadEnd()
    {
        foreach (Perk perk in perks)
        {
            perk.OnReloadEnd();
        }
    }

    public void OnPerReload()
    {
        foreach (Perk perk in perks)
        {
            perk.OnPerReload();
        }
    }

    public void OnUnequip()
    {
        foreach (Perk perk in perks)
        {
            perk.OnUnequip();
        }
    }

    public void OnEquip()
    {
        foreach (Perk perk in perks)
        {
            perk.OnEquip();
        }
    }
}