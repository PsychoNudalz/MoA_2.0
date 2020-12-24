using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Anson:
/// Extends Life System Script
/// for player
/// </summary>
public class PlayerLifeSystemScript : LifeSystemScript
{
    [Header("Components+")]
    [Header("Player Animator")]
    public Animator animator;
    public string deathTriggerName = "Death";

    private void Awake()
    {
        base.health_Current = base.Health_Max;
        //updateHealthBar();

    }

    public override void DeathBehaviour()
    {
        base.DeathBehaviour();
    }

    public override int takeDamage(float dmg, int level, ElementTypes element)
    {
        int i = base.takeDamage(dmg,  level,  element);
        if (i > 0)
        {
        }
        return Health_Current;
    }

    /// <summary>
    /// applies a delay so that that the animatorcan play the death animation, before disabling the player GameObject
    /// </summary>
    /// <returns></returns>
    public override IEnumerator delayDeathRoutine()
    {
        animator.SetBool(deathTriggerName, IsDead);
        yield return new WaitForSeconds(delayDeath);
        DeathBehaviour();
    }




}
