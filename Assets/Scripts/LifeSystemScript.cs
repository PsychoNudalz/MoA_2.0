using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Anson:
/// base Life system super class, handles heealth, taking damage, healing
/// </summary>

public class LifeSystemScript : MonoBehaviour
{
    [Header("States")]
    [SerializeField] protected int health_Current;
    [SerializeField] int health_Max = 10;
    [SerializeField] bool isDead = false;

    [Header("On Death")]
    public GameObject deathGameObject;
    public bool disableOnDeath = true;
    public bool destroyOnDeath;
    public float delayDeath = 0;
    public bool detatchPopUps = true;

    [Header("Components")]
    public DamagePopScript damagePopScript;
    public GroupParticleSystemScript groupParticleSystemScript;

    public int Health_Current { get => health_Current; }
    public int Health_Max { get => health_Max; }
    public bool IsDead { get => isDead; }

    private void Awake()
    {
        health_Current = health_Max;
       // updateHealthBar();
    }

    /// <summary>
    /// deal damage to the gameobject
    /// damage rounded to the closest integer
    /// triggers death event if health reaches 0
    /// </summary>
    /// <param name="dmg"></param>
    /// <returns> health remaining </returns>
    public virtual int takeDamage(float dmg, int level, ElementTypes element)
    {

        if (!isDead)
        {
            health_Current -= Mathf.RoundToInt(dmg);
            print(name + " take damage: " + dmg);
            //updateHealthBar();
            displayDamage(dmg);
            playDamageParticles();
        }

        CheckDead();
        return health_Current;

    }
    /// <summary>
    /// heal gameobject
    /// amount rounded to the closest integer
    /// 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns> health remaining</returns>
    public virtual int healHealth(float amount)
    {
        if (!isDead)
        {
            health_Current += Mathf.RoundToInt(amount);
            print(name + " heal damage: " + amount);
            if (health_Current > health_Max)
            {
                health_Current = health_Max;
            }
            //updateHealthBar();
        }
        return health_Current;
    }


    /// <summary>
    /// check if the gameobject is dead
    /// plays death event when health reaches 0
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckDead()
    {
        if (health_Current <= 0)
        {
            isDead = true;
            health_Current = 0;

            StartCoroutine(delayDeathRoutine());
        }
        return isDead;
    }

    void displayDamage(float dmg)
    {
        if (damagePopScript == null)
        {
            Debug.LogWarning(name + " missing damage numbers");
            return;
        }
        damagePopScript.displayDamage(dmg);
    }
    void playDamageParticles()
    {
        if (groupParticleSystemScript == null)
        {
            return;
        }
        groupParticleSystemScript.Play();
    }

    /*
    protected void updateHealthBar()
    {
        if (healthBarController != null)
        {
            healthBarController.SetMaxHealth(health_Max);
            healthBarController.SetHealth((float)health_Current);
        }
    }
    */


    /// <summary>
    /// how the game object behave when killed
    /// </summary>
    public virtual void DeathBehaviour()
    {
        if (deathGameObject != null)
        {
            Instantiate(deathGameObject, deathGameObject.transform.position, deathGameObject.transform.rotation).SetActive(true);
        }

        if (disableOnDeath)
        {
            if (detatchPopUps)
            {
                StartCoroutine(reattach());
            }
            gameObject.SetActive(false);
        }
        else if (destroyOnDeath)
        {
            Destroy(gameObject);
        }


    }

    /// <summary>
    /// delay death behaviour by a certain time
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator delayDeathRoutine()
    {
        yield return new WaitForSeconds(delayDeath);
        DeathBehaviour();
    }

    public virtual IEnumerator reattach()
    {
        damagePopScript.transform.SetParent(null);
        groupParticleSystemScript.transform.SetParent(null);
        yield return new WaitForSeconds(3f);
        damagePopScript.transform.SetParent(transform);
        groupParticleSystemScript.transform.SetParent(transform);
    } 
}
