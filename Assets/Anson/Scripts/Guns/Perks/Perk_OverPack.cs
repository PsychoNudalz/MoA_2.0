using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_OverPack : Perk
{
    private float previousMag = 0;
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
        OnActivatePerk();
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
        previousMag = Mathf.Min(gunDamageScript.CurrentMag, gunDamageScript.MagazineSize);
        if (previousMag >= 0)
        {
            base.OnActivatePerk(data);
        }
    }

    public override void OnDeactivatePerk()
    {
        gunDamageScript.AddAmmoToCurrentMag(Mathf.RoundToInt(previousMag));

        base.OnDeactivatePerk();
    }
}
