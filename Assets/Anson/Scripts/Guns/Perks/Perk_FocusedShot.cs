using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

public class Perk_FocusedShot : Perk
{
    [Space(10f)]
    [Header("Stats")]
    [Range(0f, 1f)]
    [SerializeField]
    private float stackPortion = .1f;

    public override void OnShot(ShotData shotData)
    {
        if (isActive)
        {
            OnDeactivatePerk();
        }
        else
        {
            duration_Current = 0;
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
        if (isEquiped && !isActive && duration_Current >= duration)
        {
            OnActivatePerk();
            ResetDuration();
        }
    }

    public override void OnDurationEnd()
    {
        OnActivatePerk();
    }

    protected override void OnUpdate()
    {
        if (!isActive && duration_Current < duration)
        {
            duration_Current += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }

    public override void OnUnequip()
    {
        stack_Current = 1;
        OnDeactivatePerk();
        isActive = false;
        if (isPlayerPerk)
        {
            PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.REMOVE);
        }

        base.OnUnequip();
    }

    public override void OnEquip()
    {
        base.OnEquip();
        if (isPlayerPerk)
        {
            PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.ADD);
        }
    }

    public override void OnActivatePerk(Object data = null)
    {
        stack_Max = Mathf.Max(1, Mathf.FloorToInt(gunDamageScript.MagazineSize * stackPortion));
        stack_Current = stack_Max;
        gunDamageScript.AddPerkStats(perkStatsScript);

        base.OnActivatePerk(data);
    }

    public override void OnDeactivatePerk()
    {
        if (!isActive)
        {
            return;
        }

        if (stack_Current > 0)
        {
            AddStacks(-1);

            if (stack_Current != 0)
            {
                if (isPlayerPerk)
                {
                    PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.UPDATE);
                }
            }
            else
            {
                gunDamageScript.RemovePerkStats(perkStatsScript);
                duration_Current = 0;
                isActive = false;

                if (isPlayerPerk)
                {
                    PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.UPDATE);
                    perkEffectController?.PlayDeactivate();
                }
            }
        }
    }
}