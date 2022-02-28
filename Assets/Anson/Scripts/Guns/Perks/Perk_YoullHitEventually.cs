using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_YoullHitEventually : Perk
{
    private void FixedUpdate()
    {
        OnFixedUpdate();
    }

    public override void OnShot(ShotData shotData)
    {
        if (shotData.IsTargetHit)
        {
            OnDeactivatePerk();
        }
        else
        {
            OnActivatePerk();
        }
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
        IfDurationEnd();
    }

    public override void OnDurationEnd()
    {
        OnDeactivatePerk();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame


    public override void OnUnequip()
    {
        base.OnUnequip();
        while (stack_Current > 0)
        {
            OnDeactivatePerk();
        }
    }

    public override void OnEquip()
    {
        base.OnEquip();
    }

    public override void OnActivatePerk(Object data = null)
    {
        base.OnActivatePerk(data);

        if (CanStack())
        {
            gunDamageScript.AddPerkStats(perkStatsScript);
            AddStacks(1);
            print($"Activate you'll hit {stack_Current}");
        }

        ResetDuration();
    }

    public override void OnDeactivatePerk()
    {
        DeactivatePerStack(false);
    }
}