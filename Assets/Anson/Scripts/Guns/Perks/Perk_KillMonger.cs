using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_KillMonger : Perk
{
    public Perk_KillMonger(PlayerGunDamageScript playerGunDamageScript, MainGunStatsScript mainGunStatsScript, GunStatsScript originalGunStatsScript, PlayerController playerController) : base(playerGunDamageScript, mainGunStatsScript, originalGunStatsScript, playerController)
    {
    }

    public override void OnShot()
    {
    }

    public override void OnHit(ShotData shotData)
    {
    }

    public override void OnMiss()
    {
    }

    public override void OnKill()
    {
        // OnActivate();
    }

    public override void OnReload()
    {
    }

    public override void OnPerReload()
    {
    }

    public override void OnActivatePerk(Object date = null)
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnDurationEnd()
    {
    }

    public override void OnDeactivatePerk()
    {
    }
}
