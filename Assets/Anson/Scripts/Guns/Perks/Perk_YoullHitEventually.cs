using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_YoullHitEventually : Perk
{
    [Header("Current Stat")]
    [SerializeField]
    private int RewardStacks = 0;

    [SerializeField]
    private int EncoragementStacks = 0;

    [SerializeField]
    private PerkGunStatsScript encorageStats;
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

    public override void OnFixedUpdate()
    {
       
    }

    public override void OnDurationEnd()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetStacks()
    {
        RewardStacks = 0;
        EncoragementStacks = 0;
    }

    void AddReward()
    {
        if (EncoragementStacks > 0)
        {
            ResetStacks();
            base.OnActivatePerk();
            ResetDuration();
        }

        if (RewardStacks < stack_Max)
        {
            
            RewardStacks++;
            stack_Current = RewardStacks;
            if (isPlayerPerk)
            {
                PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.UPDATE);
            }
        }
    }

    void AddEncouragement()
    {
        if (RewardStacks > 0)
        {
            ResetStacks();
            base.OnActivatePerk();
            duration_Current = 0;
        }

        if (EncoragementStacks < stack_Max)
        {
            EncoragementStacks++;
            stack_Current = EncoragementStacks;
            if (isPlayerPerk)
            {
                PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.UPDATE);
            }
        }
    }
}
