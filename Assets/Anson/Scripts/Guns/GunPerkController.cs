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
            perk.Initialise(gunDamageScript, mainGunStatsScript, originalGunStatsScript);
        }
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