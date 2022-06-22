using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_HeatingUp : Perk
{
    private void FixedUpdate()
    {
        OnFixedUpdate();
    }

    public override void OnTargetHit(ShotData shotData)
    {
        // OnActivatePerk();
    }

    public override void OnShot(ShotData shotData)
    {
    }

    public override void OnHit(ShotData shotData)
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
        OnActivatePerk();
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

    public override void OnActivatePerk(Object data = null)
    {
        base.OnActivatePerk(data);

        if (CanStack())
        {
            gunPerkController.AddPerkStatsAdditive(perkStatsScript);
            AddStacks(1);
        }

        ResetDuration();
    }

    public override void OnFixedUpdate()
    {
        IfDurationEnd();
    }



    public override void OnDurationEnd()
    {
        OnDeactivatePerk();
    }

    public override void OnDeactivatePerk()
    {
        DeactivatePerStack();

        //RemoveAllStackedStats();
    }

    

    public override void OnUnequip()
    {
        while (stack_Current>0)
        {
            OnDeactivatePerk();
        }
    }
}