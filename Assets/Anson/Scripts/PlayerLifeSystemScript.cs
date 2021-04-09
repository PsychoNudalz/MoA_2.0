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
    [SerializeField] PlayerVolumeControllerScript playerVolumeControllerScript;
    [Header("Player Animator")]
    public Animator animator;
    public string deathTriggerName = "Death";
    [Header("Volumn Effects")]
    [Range(0f, 1f)]
    [SerializeField] float vignetteThreshold = 0.25f;

    public PlayerMasterScript PlayerMasterScript { set => playerMasterScript = value; }
    public AnsonTempUIScript UIScript1 { set => UIScript = value; }
    public PlayerVolumeControllerScript PlayerVolumeControllerScript { set => playerVolumeControllerScript = value; }

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

    public override int takeDamage(float dmg, int level, ElementTypes element, bool displayTakeDamageEffect = true)
    {
        int i = base.takeDamage(dmg, level, element, displayTakeDamageEffect);
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

    public override void PlayTakeDamageEffect()
    {
        playerVolumeControllerScript.PlayCD();
        if ((health_Current / (float)health_Max) < vignetteThreshold)
        {
            print("Changing Vignette: " + (health_Current / health_Max));
            playerVolumeControllerScript.SetBloodVignette(true, 1 - (health_Current / (Health_Max * vignetteThreshold)));
        }
    }
    public override int healHealth(float amount)
    {
        int temp = base.healHealth(amount);
        if (health_Current / (float)health_Max > vignetteThreshold)
        {
            playerVolumeControllerScript.SetBloodVignette(false);
        }
        return temp;
    }


}
