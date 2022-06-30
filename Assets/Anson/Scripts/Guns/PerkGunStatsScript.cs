using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkGunStatsScript : GunStatsScript
{
    [Header("Gun Stats Multiplier")]
    [Range(0.0000001f, 10f)]
    [SerializeField]
    public float damagePerProjectileM = 1;

    [Range(0.0000001f, 10f)]
    [SerializeField]
    public float RPMM = 1;

    [Range(0.0000001f, 10f)]
    [SerializeField]
    public float reloadSpeedM = 1;

    [SerializeField]
    public Vector2 recoilM = new Vector2(1, 1);

    [SerializeField]
    public Vector2 hipfireM = new Vector2(1, 1);

    [Range(0.0000001f, 10f)]
    [SerializeField]
    public float rangeM = 1;

    [SerializeField]
    public float magazineSizeM = 1;

    [Header("Affected Stats")]
    [SerializeField]
    private ModifiedStat[] modifiedStats = Array.Empty<ModifiedStat>();

    public ModifiedStat[] ModifiedStats => modifiedStats;


    [ContextMenu("InitialiseStatArray")]
    private void InitialiseStatArray()
    {
        List<ModifiedStat> temp = new List<ModifiedStat>();

        if (damagePerProjectile != 0)
        {
            temp.Add(ModifiedStat.DAMAGE);
        }

        if (RPM != 0)
        {
            temp.Add(ModifiedStat.RPM);
        }

        if (reloadSpeed != 0)
        {
            temp.Add(ModifiedStat.RELOAD);
        }

        if (recoil.magnitude != 0)
        {
            temp.Add(ModifiedStat.RECOIL);
        }

        if (recoil_HipFire.magnitude != 0)
        {
            temp.Add(ModifiedStat.HIPFIRE);
        }

        if (range != 0)
        {
            temp.Add(ModifiedStat.RANGE);
        }

        if (magazineSize != 0)
        {
            temp.Add(ModifiedStat.MAGAZINE);
        }

        if (elementDamage != 0)
        {
            temp.Add(ModifiedStat.EDAMAGE);
        }

        if (elementPotency != 0)
        {
            temp.Add(ModifiedStat.EPOTENCY);
        }

        if (elementChance != 0)
        {
            temp.Add(ModifiedStat.ECHANCE);
        }

        if (damagePerProjectileM != 1)
        {
            temp.Add(ModifiedStat.DAMAGE);
            temp.Add(ModifiedStat.DAMAGE_M);
        }

        if (RPMM != 1)
        {
            temp.Add(ModifiedStat.RPM);
            temp.Add(ModifiedStat.RPM_M);
        }

        if (reloadSpeedM != 1)
        {
            temp.Add(ModifiedStat.RELOAD);
            temp.Add(ModifiedStat.RELOAD_M);
        }

        if (recoilM.x != 1 || recoilM.y != 1)
        {
            temp.Add(ModifiedStat.RECOIL);
            temp.Add(ModifiedStat.RECOIL_M);
        }

        if (hipfireM.x != 1 || hipfireM.y != 1)
        {
            temp.Add(ModifiedStat.HIPFIRE);
            temp.Add(ModifiedStat.HIPFIRE_M);
        }

        if (rangeM != 1)
        {
            temp.Add(ModifiedStat.RANGE);
            temp.Add(ModifiedStat.RANGE_M);
        }

        if (magazineSizeM != 1)
        {
            temp.Add(ModifiedStat.MAGAZINE);
            temp.Add(ModifiedStat.MAGAZINE_M);
        }

        modifiedStats = temp.ToArray();
    }
}