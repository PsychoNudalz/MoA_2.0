using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_AdrenalineBoost : Perk
{
    [SerializeField]
    private float speedBoost = .2f;
    
    private void FixedUpdate()
    {
        OnFixedUpdate();
    }
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

    public override void OnUnequip()
    {
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


    public override void Initialise(GunDamageScript gunDamageScript, MainGunStatsScript mainGunStatsScript,
        GunStatsScript originalGunStatsScript, GunPerkController gunPerkController)
    {
        base.Initialise(gunDamageScript, mainGunStatsScript, originalGunStatsScript, gunPerkController);
    }

    public override void SetPlayerController(PlayerController playerController)
    {
        base.SetPlayerController(playerController);
    }

    public override void OnActivatePerk(Object data = null)
    {
        base.OnActivatePerk(data);

        if (isPlayerPerk)
        {
            if (!playerController)
            {
                SetPlayerController(GetComponentInParent<PlayerController>());
            }
        }
        
        if (CanStack())
        {
            playerController.BoostStatsMoveSpeedMultiplier(speedBoost);

            AddStacks(1);
            print($"Activate Kill monger {stack_Current}");
        }

        ResetDuration();
    }

    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void OnDeactivatePerk()
    {
        base.OnDeactivatePerk();
        for (int i = 0; i < stack_Current; i++)
        {
            playerController.BoostStatsMoveSpeedMultiplier(-speedBoost);

        }

        stack_Current = 0;
    }

    protected override void ResetDuration()
    {
        base.ResetDuration();
    }

    protected override bool AddStacks(int i)
    {
        return base.AddStacks(i);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
