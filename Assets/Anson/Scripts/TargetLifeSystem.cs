using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLifeSystem : LifeSystemScript
{
    [Header("Target Handler")]
    [SerializeField] TargetHandlerScript targetHandler;
    [Header("Shader Effects")]
    [SerializeField] TargetMaterialHandlerScript targetMaterialHandler;
    [Header("Sound")]
    [SerializeField] TargetSoundScript targetSoundScript;
    [Header("Collider")]
    [SerializeField] Collider[] mainColliders;

    public TargetMaterialHandlerScript TargetMaterialHandler { get => targetMaterialHandler; }

    private void Awake()
    {
        base.Awake();

        targetHandler = GetComponent<TargetHandlerScript>();
        targetMaterialHandler = targetHandler.TargetMaterialHandler;
        targetSoundScript = targetHandler.TargetSoundScript;
    }

    public override void PlayTakeDamageEffect()
    {
        targetMaterialHandler.PlayerTakeDamageEffect();
    }

    public override int takeDamage(float dmg, int level, ElementTypes element, bool displayTakeDamageEffect = true)
    {
        targetMaterialHandler.StartDecay();
        if (displayTakeDamageEffect)
        {
            targetSoundScript.Play_TakeDamage();
        }
        return base.takeDamage(dmg, level, element, displayTakeDamageEffect);
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier, bool displayTakeDamageEffect = true)
    {
        targetMaterialHandler.StartDecay();
        targetSoundScript.Play_Stagger();
        return base.takeDamageCritical(dmg, level, element, multiplier, displayTakeDamageEffect);
    }
    public override void RemoveDebuff(FireEffectScript debuff = null)
    {
        base.RemoveDebuff(debuff as DebuffScript);
        targetMaterialHandler.SetFire(CheckIsStillOnFire() != null);
    }
    public override void ApplyDebuff(FireEffectScript debuff)
    {
        base.ApplyDebuff(debuff as DebuffScript);
        targetMaterialHandler.SetFire(true);

    }
    public override void ApplyDebuff(ShockEffectScript debuff)
    {
        base.ApplyDebuff(debuff as DebuffScript);
        targetMaterialHandler.SetShock(true);

    }

    public override void RemoveDebuff(ShockEffectScript debuff)
    {
        base.RemoveDebuff(debuff as DebuffScript);
        targetMaterialHandler.ResetShockList();
    }

    public override void RemoveDebuff(IceEffectScript debuff = null)
    {
        base.RemoveDebuff(debuff as DebuffScript);
        //print(name + " deactivate Ice");
        targetMaterialHandler.SetIce(false);
    }
    public override void ApplyDebuff(IceEffectScript debuff)
    {
        targetMaterialHandler.SetIce(true);
        base.ApplyDebuff(debuff as DebuffScript);

    }

    public override void ResetSystem()
    {
        base.ResetSystem();
        targetMaterialHandler.SetFire(false);
        foreach (Collider c in mainColliders)
        {
            c.enabled = true;

        }
    }

    public override void DeathBehaviour()
    {
        targetSoundScript.Play_Death();
        base.DeathBehaviour();
    }

    public override bool CheckDead()
    {
        bool b = base.CheckDead();
        if (b)
        {
            foreach (Collider c in mainColliders)
            {
                c.enabled = false;

            }

        }
        return b;
    }


}
