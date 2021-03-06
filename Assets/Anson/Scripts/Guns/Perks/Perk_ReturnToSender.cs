using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_ReturnToSender : Perk
{
    public override void OnActivatePerk(Object data = null)
    {
        base.OnActivatePerk(data);
        AddStacks(1);
        if (stack_Current == stack_Max)
        {
            gunDamageScript.AddAmmoToCurrentMag(1);

            OnDeactivatePerk();
        }
    }

    public override void OnDeactivatePerk()
    {
        base.OnDeactivatePerk();
        stack_Current = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public override void OnShot(ShotData shotData)
    {
    }

    public override void OnHit(ShotData shotData)
    {
    }

    public override void OnTargetHit(ShotData shotData)
    {
        OnActivatePerk();
    }

    public override void OnCritical(ShotData shotData)
    {
        OnActivatePerk();
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

    public override void OnUnequip()
    {
        OnDeactivatePerk();
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnDurationEnd()
    {
    }
}