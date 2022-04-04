using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystemRouterScript : LifeSystemScript
{
    [Header("Life System Router")]
    [SerializeField] LifeSystemScript routedLS;
    public override void ApplyDebuff(DebuffScript debuff)
    {
        routedLS.ApplyDebuff(debuff);
    }

    public override void ApplyDebuff(FireEffectScript debuff)
    {
        routedLS.ApplyDebuff(debuff);
    }

    public override void ApplyDebuff(ShockEffectScript debuff)
    {
        routedLS.ApplyDebuff(debuff);
    }

    public override void ApplyDebuff(IceEffectScript debuff)
    {
        routedLS.ApplyDebuff(debuff);
    }

    public override bool CheckDead()
    {
        return routedLS.CheckDead();
    }

    public override void DeathBehaviour()
    {
        routedLS.DeathBehaviour();
    }

    public override IEnumerator delayDeathRoutine()
    {
        return routedLS.delayDeathRoutine();
    }

    public override bool Equals(object other)
    {
        return routedLS.Equals(other);
    }

    public override Vector3 GetEffectCenter()
    {
        return routedLS.GetEffectCenter();
    }

    public override int GetHashCode()
    {
        return routedLS.GetHashCode();
    }

    public override int healHealth(float amount)
    {
        return routedLS.healHealth(amount);
    }

    public override int healHealth_Percentage(float amount)
    {
        return routedLS.healHealth_Percentage(amount);
    }

    public override int healHealth_PercentageMissing(float amount)
    {
        return routedLS.healHealth_PercentageMissing(amount);
    }

    public override void PlayTakeDamageEffect()
    {
        routedLS.PlayTakeDamageEffect();
    }

    public override IEnumerator reattach()
    {
        return routedLS.reattach();
    }

    public override void RemoveDebuff(FireEffectScript debuff = null)
    {
        routedLS.RemoveDebuff(debuff);
    }

    public override void RemoveDebuff(ShockEffectScript debuff)
    {
        routedLS.RemoveDebuff(debuff);
    }

    public override void RemoveDebuff(IceEffectScript debuff)
    {
        routedLS.RemoveDebuff(debuff);
    }

    public override void RemoveDebuff(DebuffScript debuff = null)
    {
        routedLS.RemoveDebuff(debuff);
    }

    public override void ResetSystem()
    {
        routedLS.ResetSystem();
    }

    public override int takeDamage(float dmg, int level, ElementTypes element, bool displayTakeDamageEffect = true)
    {
        // print($"Route damage to:{routedLS.name} ");
        return routedLS.takeDamage(dmg, level, element, displayTakeDamageEffect);
    }

    public override int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier = 1, bool displayTakeDamageEffect = true)
    {
        return routedLS.takeDamageCritical(dmg, level, element, multiplier, displayTakeDamageEffect);
    }

    public override string ToString()
    {
        return routedLS.ToString();
    }
    public override void AwakeBehaviour()
    {
    }

    public override void displayDamage(float dmg, ElementTypes e = ElementTypes.PHYSICAL)
    {
        routedLS.displayDamage(dmg, e);
    }
}
