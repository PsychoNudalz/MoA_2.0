using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_RewardEncouragement : Perk
{
    [Header("Current Stat")]
    [SerializeField]
    private int shotsHit = 0;

    [SerializeField]
    private PerkGunStatsScript secondaryStats;

    [SerializeField]
    private bool addedSecondaryStats = false;
    public override void OnShot(ShotData shotData)
    {
       
    }

    public override void OnHit(ShotData shotData)
    {
       
    }

    public override void OnTargetHit(ShotData shotData)
    {
        if (isPlayerPerk)
        {
            PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.ADD);

        }
        
        shotsHit++;
        stack_Current = shotsHit;
        if (isPlayerPerk)
        {
            PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.UPDATE);
        }
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
       OnDeactivatePerk();
    }

    public override void OnReloadEnd()
    {
       OnActivatePerk();
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

    public override void OnActivatePerk(Object data = null)
    {
        if (shotsHit > gunDamageScript.MagazineSize / 2)
        {
            OnActivate_Hit();
        }
        else
        {
            OnActivate_Miss();
        }
        shotsHit = 0;

        base.OnActivatePerk(data);
    }

    private void OnActivate_Miss()
    {
        gunPerkController.AddPerkStatsAdditive(secondaryStats);
        addedSecondaryStats = true;
        duration_Current = 0;
    }

    private void OnActivate_Hit()
    {
        gunPerkController.AddPerkStatsAdditive(perkStatsScript);
        addedSecondaryStats = false;
        ResetDuration();
    }

    public override void OnDeactivatePerk()
    {
        if (isActive)
        {
            base.OnDeactivatePerk();
            if (secondaryStats)
            {
                gunPerkController.RemovePerkStatsAdditive(secondaryStats);

            }
            else
            {
                gunPerkController.RemovePerkStatsAdditive(perkStatsScript);

            }

            stack_Current = 0;
        }
    }

    protected override void OnUpdate()
    {
    }
}
