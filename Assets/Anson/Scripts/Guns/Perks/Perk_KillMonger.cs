using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

public class Perk_KillMonger : Perk
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

    public override void OnReloadEnd( )
    {
    }

    public override void OnPerReload( )
    {
    }

    public override void OnActivatePerk(Object data = null)
    {
       base.OnActivatePerk(data);

       if (CanStack())
       {
           gunDamageScript.AddPerkStats(perkStatsScript);
           AddStacks(1);
           print($"Activate Kill monger {stack_Current}");
       }

       ResetDuration();
    }

    public override void OnFixedUpdate()
    {
        if (isActive&& duration_Current < 0)
        {
            duration_Current = 0;
            OnDurationEnd();
        }
    }

    public override void OnDurationEnd()
    {
        OnDeactivatePerk();
    }

    public override void OnDeactivatePerk()
    {
        base.OnDeactivatePerk();
        RemoveAllStack();
    }

    public override void OnUnequip()
    {
        OnDeactivatePerk();
    }
}