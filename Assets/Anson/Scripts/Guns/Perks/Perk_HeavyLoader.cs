using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_HeavyLoader : Perk
{
    [SerializeField]
    private float maxMultiplier = .25f;
    [SerializeField]
    private AnimationCurve perkBoostCurve;

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
        if (perkBoostCurve.Evaluate(gunDamageScript.CurrentMag / gunDamageScript.MagazineSize) != 0)
        {
            OnActivatePerk();
        }
    }

    public override void OnReloadEnd()
    {
        OnDeactivatePerk();
    }

    public override void OnPerReload()
    {
        
    }

    public override void OnUnequip()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnDurationEnd()
    {
        
    }

    public override void OnActivatePerk(Object data = null)
    {
        base.OnActivatePerk(data);
        perkStatsScript.reloadSpeedM =
            (1 - perkBoostCurve.Evaluate(gunDamageScript.CurrentMag / gunDamageScript.MagazineSize) *
                (1 - maxMultiplier));
        gunDamageScript.AddPerkStats(perkStatsScript);
    }

    public override void OnDeactivatePerk()
    {
        if (isActive)
        {
            return;
        }
        base.OnDeactivatePerk();
        gunDamageScript.RemovePerkStats(perkStatsScript);

    }
}
