using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLifeSystem : LifeSystemScript
{
    [Header("Target Handler")]
    [SerializeField] TargetHandlerScript targetHandler;
    [Header("Shader Effects")]
    [SerializeField] TargetEffectController targetEffectController;
    [Header("Sound")]
    [SerializeField] TargetSoundScript targetSoundScript;
    [Header("Collider")]
    [SerializeField] Collider[] mainColliders;

    public TargetEffectController TargetMaterialHandler { get => targetEffectController; }

    private void Awake()
    {
        //base.Awake();
        AwakeBehaviour();
        targetHandler = GetComponent<TargetHandlerScript>();
        targetEffectController = targetHandler.TargetMaterialHandler;
        targetSoundScript = targetHandler.TargetSoundScript;
        if (!centreOfMass)
        {
            centreOfMass = targetEffectController.transform;
        }
    }

    public override void PlayTakeDamageEffect()
    {
        targetEffectController.TakeDamageEffect();
    }

    public override int takeDamage(float dmg, int level, ElementTypes element, bool displayTakeDamageEffect = true)
    {
        targetEffectController.StartDecay();
        if (displayTakeDamageEffect)
        {
            targetSoundScript.Play_TakeDamage();
        }
        return base.takeDamage(dmg, level, element, displayTakeDamageEffect);
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier, bool displayTakeDamageEffect = true)
    {
        targetEffectController.StartDecay();
        targetSoundScript?.Play_Stagger();
        return base.takeDamageCritical(dmg, level, element, multiplier, displayTakeDamageEffect);
    }
    public override void RemoveDebuff(FireEffectScript debuff = null)
    {
        base.RemoveDebuff(debuff as DebuffScript);
        targetEffectController.SetFire(CheckIsStillOnFire() != null);
    }
    public override void ApplyDebuff(FireEffectScript debuff)
    {
        base.ApplyDebuff(debuff as DebuffScript);
        targetEffectController.SetFire(true);

    }
    public override void ApplyDebuff(ShockEffectScript debuff)
    {
        base.ApplyDebuff(debuff as DebuffScript);
        targetEffectController.SetShock(true);

    }

    public override void RemoveDebuff(ShockEffectScript debuff)
    {
        base.RemoveDebuff(debuff as DebuffScript);
        targetEffectController.ResetShockList();
    }

    public override void RemoveDebuff(IceEffectScript debuff = null)
    {
        base.RemoveDebuff(debuff as DebuffScript);
        //print(name + " deactivate Ice");
        targetEffectController.SetIce(false);
    }
    public override void ApplyDebuff(IceEffectScript debuff)
    {
        targetEffectController.SetIce(true);
        base.ApplyDebuff(debuff as DebuffScript);

    }

    public override void ResetSystem()
    {
        base.ResetSystem();
        targetEffectController.SetFire(false);
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

    public override Vector3 GetEffectCenter()
    {
        return targetEffectController.transform.position;
    }


}
