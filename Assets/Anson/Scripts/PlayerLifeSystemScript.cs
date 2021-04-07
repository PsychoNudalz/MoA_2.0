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
    [SerializeField] PlayerMasterScript playerMasterScript;
    [SerializeField] AnsonTempUIScript UIScript;
    [Header("Player Animator")]
    public Animator animator;
    public string deathTriggerName = "Death";

    public PlayerMasterScript PlayerMasterScript { set => playerMasterScript = value; }
    public AnsonTempUIScript UIScript1 { set => UIScript = value; }

    private void Awake()
    {
        base.health_Current = base.Health_Max;
        UIScript = playerMasterScript.AnsonTempUIScript;
        UIScript.SetHealth(health_Current, Health_Max);

        //updateHealthBar();

    }

    public override void DeathBehaviour()
    {
        playerMasterScript.GameOver();
        base.DeathBehaviour();
    }

    public override int takeDamage(float dmg, int level, ElementTypes element,bool displayTakeDamageEffect = true)
    {
        int i = base.takeDamage(dmg,  level,  element, displayTakeDamageEffect);
        if (i > 0)
        {
            UIScript.SetHealth(health_Current, Health_Max);
        }
        return Health_Current;
    }

    /// <summary>
    /// applies a delay so that that the animatorcan play the death animation, before disabling the player GameObject
    /// </summary>
    /// <returns></returns>
    public override IEnumerator delayDeathRoutine()
    {
        if (animator != null)
        {
        animator.SetBool(deathTriggerName, IsDead);
        }
        yield return new WaitForSeconds(delayDeath);
        DeathBehaviour();
    }


    protected override void displayDamage(float dmg, ElementTypes e = ElementTypes.PHYSICAL)
    {
    }



}
