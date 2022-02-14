using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_HunkerDown : Perk
{
    [SerializeField]
    private float minSpeed = 6f;

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

    public override void OnUnequip()
    {
        base.OnUnequip();
        OnDeactivatePerk();
    }

    public override void OnFixedUpdate()
    {
        if (!isEquiped)
        {
            return;
        }

        if (playerController)
        {
            if (playerController.MoveSpeedCurrent <= minSpeed)
            {
                OnActivatePerk();
            }
            else
            {
                OnDeactivatePerk();
            }
        }
        else
        {
            if (isPlayerPerk)
            {
                SetPlayerController(GetComponentInParent<PlayerController>());
            }
        }
    }

    public override void OnDurationEnd()
    {
    }

    public override void OnActivatePerk(Object data = null)
    {
        if (isActive)
        {
            return;
        }

        base.OnActivatePerk(data);
        if (isPlayerPerk)
        {
            if (!playerController)
            {
                SetPlayerController(GetComponentInParent<PlayerController>());
            }
        }

        gunDamageScript.AddPerkStats(perkStatsScript);
    }

    public override void OnDeactivatePerk()
    {
        if (!isActive)
        {
            return;
        }

        base.OnDeactivatePerk();
        gunDamageScript.RemovePerkStats(perkStatsScript);
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }
}