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
    [SerializeField] PlayerUIScript UIScript;
    [SerializeField] PlayerVolumeControllerScript playerVolumeControllerScript;
    [SerializeField] PlayerSoundScript playerSoundScript;
    [Header("Player Animator")]
    public Animator animator;
    public string deathTriggerName = "Death";
    [Header("Volumn Effects")]
    [Range(0f, 1f)]
    [SerializeField] float vignetteThreshold = 0.25f;

    public PlayerMasterScript PlayerMasterScript { set => playerMasterScript = value; }
    public PlayerUIScript UIScript1 { set => UIScript = value; }
    public PlayerVolumeControllerScript PlayerVolumeControllerScript { set => playerVolumeControllerScript = value; }
    public PlayerSoundScript PlayerSoundScript { get => playerSoundScript; set => playerSoundScript = value; }

    private void Awake()
    {
        base.health_Current = base.Health_Max;
        UIScript = playerMasterScript.PlayerUIScript;
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


    public override void displayDamage(float dmg, ElementTypes e = ElementTypes.PHYSICAL)
    {
    }

    public override void PlayTakeDamageEffect()
    {
        playerVolumeControllerScript.PlayCD();
        playerSoundScript.Play_TakeDamage();
        if (GetPercentageHealth() < vignetteThreshold)
        {
            //print("Changing Vignette: " + (health_Current / health_Max));
            playerVolumeControllerScript.SetBloodVignette(true, 1 - (health_Current / (Health_Max * vignetteThreshold)));
        }
    }
    public override int healHealth(float amount)
    {
        int temp = base.healHealth(amount);
        if (GetPercentageHealth() > vignetteThreshold)
        {
            playerVolumeControllerScript.SetBloodVignette(false);
        }
        else
        {
            playerVolumeControllerScript.SetBloodVignette(true, 1 - (health_Current / (Health_Max * vignetteThreshold)));

        }
        UIScript.SetHealth(health_Current, Health_Max);
        playerSoundScript.Play_Heal();


        return temp;
    }
    
    


}
