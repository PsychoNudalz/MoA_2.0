using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_CrankShot : Perk
{
    private bool startReload = false;
    public override void OnShot(ShotData shotData)
    {
        if (isActive)
        {
            OnDeactivatePerk();
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
        startReload = true;
    }

    public override void OnReloadEnd()
    {
        if (startReload)
        {
            startReload = false;
        }
    }

    public override void OnPerReload()
    {
        if (startReload)
        {
            OnActivatePerk();
        }
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnDurationEnd()
    {
        
    }

    public override void OnActivatePerk(Object data = null)
    {
        gunPerkController.AddPerkStatsAdditive(perkStatsScript,perkStatsScript.ModifiedStats);
        AddStacks(1);
        base.OnActivatePerk(data);

    }

    public override void OnDeactivatePerk()
    {
        RemoveAllStackedStats();
        base.OnDeactivatePerk();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnUnequip()
    {
        OnDeactivatePerk();

    }
}
