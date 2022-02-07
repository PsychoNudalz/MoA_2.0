using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class Perk : MonoBehaviour
{
    [Header("Componenets")]
    protected PlayerGunDamageScript playerGunDamageScript;
    protected MainGunStatsScript mainGunStatsScript;
    protected GunStatsScript originalGunStatsScript;
    protected PlayerController playerController;

    [Header("Stats")]
    protected bool isActive = false;
    protected int stack_Current = 0;
    protected int stack_Max = 1;
    protected float duration = 0;
    protected float duration_Current = 0;
    protected GunStatsScript perkStatsScript;
    


    protected Perk(PlayerGunDamageScript playerGunDamageScript, MainGunStatsScript mainGunStatsScript,
        GunStatsScript originalGunStatsScript, PlayerController playerController)
    {
        this.playerGunDamageScript = playerGunDamageScript;
        this.mainGunStatsScript = mainGunStatsScript;
        this.originalGunStatsScript = originalGunStatsScript;
        this.playerController = playerController;
    }


    public abstract void OnShot();
    public abstract void OnHit(ShotData shotData);
    public abstract void OnMiss();
    public abstract void OnKill();
    public abstract void OnReload();
    public abstract void OnPerReload();
    public abstract void OnActivatePerk(Object date = null);
    public abstract void OnFixedUpdate();
    public abstract void OnDurationEnd();
    public abstract void OnDeactivatePerk();

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
}