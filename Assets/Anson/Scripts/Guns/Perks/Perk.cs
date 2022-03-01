using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class Perk : MonoBehaviour
{
    [Header("Componenets")]
    protected GunDamageScript gunDamageScript;

    protected MainGunStatsScript mainGunStatsScript;
    protected GunStatsScript originalGunStatsScript;
    protected PlayerController playerController;
    protected PerkEffectController perkEffectController;

    [Header("Stats")]
    [SerializeField]
    private Sprite perkSprite;

    [SerializeField]
    protected bool isPlayerPerk = false;

    [SerializeField]
    protected bool isActive = false;

    [SerializeField]
    protected bool isEquiped = false;

    [SerializeField]
    protected int stack_Current = 0;

    [SerializeField]
    protected int stack_Max = 1;

    [SerializeField]
    protected float duration = 1;

    [SerializeField]
    protected float duration_Current = 0;

    [SerializeField]
    protected PerkGunStatsScript perkStatsScript;

    public Sprite PerkSprite => perkSprite;

    public bool IsActive => isActive;

    public int StackCurrent => stack_Current;

    public float Duration => duration;

    public bool IsPlayerPerk
    {
        get => isPlayerPerk;
        set => isPlayerPerk = value;
    }
    
    

    public virtual void Initialise(GunDamageScript gunDamageScript, MainGunStatsScript mainGunStatsScript,
        GunStatsScript originalGunStatsScript)
    {
        this.gunDamageScript = gunDamageScript;
        this.mainGunStatsScript = mainGunStatsScript;
        this.originalGunStatsScript = originalGunStatsScript;
        if (!perkStatsScript)
        {
            perkStatsScript = GetComponent<PerkGunStatsScript>();
        }
    }

    public virtual void SetPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }


    public abstract void OnShot(ShotData shotData);
    public abstract void OnHit(ShotData shotData);
    public abstract void OnTargetHit(ShotData shotData);
    public abstract void OnCritical(ShotData shotData);
    public abstract void OnMiss(ShotData shotData);
    public abstract void OnKill(ShotData shotData);
    public abstract void OnElementTrigger(ShotData shotData);
    public abstract void OnReloadStart();
    public abstract void OnReloadEnd();
    public abstract void OnPerReload();

    public virtual void OnUnequip()
    {
        isEquiped = false;
    }

    public virtual void OnEquip()
    {
        isEquiped = true;

    }

    public virtual void OnActivatePerk(Object data = null)
    {
        isActive = true;
        if (isPlayerPerk)
        {
            PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.ADD);
            perkEffectController?.PlayActivate();
        }
    }

    public virtual void OnAwake()
    {
        if (!perkEffectController)
        {
            perkEffectController = GetComponent<PerkEffectController>();
        }

        if (perkEffectController)
        {
            perkEffectController.SetSprite(perkSprite);
        }
    }

    public abstract void OnFixedUpdate();
    public abstract void OnDurationEnd();

    public virtual void OnDeactivatePerk()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;
        if (isPlayerPerk)
        {
            PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.REMOVE);
            perkEffectController?.PlayDeactivate();
        }
    }

    protected virtual void ResetDuration()
    {
        duration_Current = duration;
        if (isPlayerPerk)
        {
            PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.UPDATE);
        }
    }

    private void Awake()
    {
        OnAwake();
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        if (duration_Current > 0)
        {
            duration_Current -= Time.deltaTime;
        }
    }

    protected virtual bool AddStacks(int i)
    {
        stack_Current = Mathf.Min(stack_Current + i, stack_Max);
        if (isPlayerPerk)
        {
            PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.UPDATE);
        }

        return stack_Max == stack_Current;
    }

    protected virtual void RemoveAllStackedStats()
    {
        for (int i = 0; i < stack_Current; i++)
        {
            gunDamageScript.RemovePerkStatsAdditive(perkStatsScript);
            
        }

        stack_Current = 0;
    }

    protected bool CanStack()
    {
        return stack_Current < stack_Max;
    }

    public float GetDurationFraction()
    {
        if (duration == 0)
        {
            return 1;
        }

        return duration_Current / duration;
    }
    
    protected void DeactivatePerStack(bool isAdditive = true)
    {
        if (stack_Current > 0)
        {
            AddStacks(-1);
            if (isAdditive)
            {
                gunDamageScript.RemovePerkStatsAdditive(perkStatsScript);
            }

            else
            {
                gunDamageScript.RemovePerkStats(perkStatsScript);
            }

            if (stack_Current != 0)
            {
                ResetDuration();
                if (isPlayerPerk)
                {
                    PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.UPDATE);
                }
            }
            else
            {
                OnDeactivatePerk();
            }
        }
        else
        {
        }
    }
    protected void IfDurationEnd()
    {
        if (isActive && duration_Current < 0)
        {
            duration_Current = 0;
            OnDurationEnd();
        }
    }
    
    
}