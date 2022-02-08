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

    [Header("Stats")]
    [SerializeField]
    protected bool isActive = false;
    [SerializeField]
    protected int stack_Current = 0;
    [SerializeField]
    protected int stack_Max = 1;
    [SerializeField]
    protected float duration = 0;
    [SerializeField]
    protected float duration_Current = 0;
    [SerializeField]
    protected PerkGunStatsScript perkStatsScript;
    



    
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
    public abstract void OnReload();
    public abstract void OnPerReload();

    public virtual void OnActivatePerk(Object data = null)
    {
        isActive = true;
    }
    public abstract void OnFixedUpdate();
    public abstract void OnDurationEnd();

    public virtual void OnDeactivatePerk()
    {
        isActive = false;
    }

    protected virtual void ResetDuration()
    {
        duration_Current = duration;
    }

    private void Update()
    {
        if (duration_Current > 0)
        {
            duration_Current -= Time.deltaTime;
        }
    }

    protected virtual bool AddStacks(int i)
    {
        stack_Current = Mathf.Min(stack_Current + i, stack_Max);
        return stack_Max == stack_Current;
    }

    protected bool CanStack()
    {
        return stack_Current < stack_Max;
    }


}