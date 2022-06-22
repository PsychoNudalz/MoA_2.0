using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_IronGrip : Perk
{
    public override void OnShot(ShotData shotData)
    {
    }

    public override void OnHit(ShotData shotData)
    {
    }

    public override void OnTargetHit(ShotData shotData)
    {
    }

    public override void OnCritical(ShotData shotData)
    {
    }

    public override void OnMiss(ShotData shotData)
    {
    }

    public override void OnKill(ShotData shotData)
    {
    }

    public override void OnElementTrigger(ShotData shotData)
    {
    }

    public override void OnReloadStart()
    {
    }

    public override void OnReloadEnd()
    {
    }

    public override void OnPerReload()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnDurationEnd()
    {
    }

    public override void OnUnequip()
    {
        base.OnUnequip();
        OnDeactivatePerk();
    }

    public override void OnEquip()
    {
        base.OnEquip();
        OnActivatePerk();
    }

    public override void OnActivatePerk(Object data = null)
    {
        if (isActive)
        {
            DeactivatePerStack();
        }
        base.OnActivatePerk(data);
        CalculateHipFire();
        gunPerkController.AddPerkStats(perkStatsScript);

    }

    public override void OnDeactivatePerk()
    {
        base.OnDeactivatePerk();
        gunPerkController.RemovePerkStats(perkStatsScript);
    }

    void CalculateHipFire()
    {
        Vector2 recoilHipFire = gunDamageScript.RecoilHipFire;
        Vector2 hipFire = perkStatsScript.Recoil_HipFire;
        if (recoilHipFire.x < recoilHipFire.y)
        {
            hipFire.y = recoilHipFire.x - recoilHipFire.y;
        }
        else
        {
            hipFire.x = recoilHipFire.y - recoilHipFire.x;
        }

        perkStatsScript.Recoil_HipFire = hipFire;
    }
}