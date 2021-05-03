using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Anson:
/// base Life system super class, handles heealth, taking damage, healing
/// </summary>

public class LifeSystemScript : MonoBehaviour
{

    [Header("States")]
    [SerializeField] protected int health_Current;
    [SerializeField] protected int health_Max = 10;
    [SerializeField] bool isDead = false;

    [Header("On Death")]
    public GameObject deathGameObject;
    public bool disableOnDeath = true;
    public bool destroyOnDeath;
    public float delayDeath = 0;
    public bool detatchPopUps = true;
    public bool reatatchPopUps = true;

    Vector3 popUpLocation;
    Vector3 particleLocation;

    [Header("Debuffs")]
    [SerializeField] protected List<DebuffScript> debuffList = new List<DebuffScript>();
    //[SerializeField] 

    [Header("Components")]
    public DamagePopScript damagePopScript;

    public int Health_Current { get => health_Current; }
    public int Health_Max { get => health_Max; }
    public bool IsDead { get => isDead; }

    public static LifeSystemScript GetLifeSystemScript(GameObject go)
    {
        LifeSystemScript ls = go.GetComponentInChildren<LifeSystemScript>();
        if (ls == null)
        {
            ls = go.GetComponentInParent<LifeSystemScript>();
        }
        return ls;
    }

    protected void Awake()
    {

        AwakeBehaviour();
    }

    protected virtual void AwakeBehaviour()
    {
        health_Current = health_Max;
        try
        {
            // updateHealthBar();
            popUpLocation = damagePopScript.transform.position - transform.position;
        }
        catch (System.Exception)
        {
            print("LifeSystemScript error - ");
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < debuffList.Count; i++)
        {
            if (debuffList[i].TickEffect(Time.deltaTime))
            {
                i--;
            }
        }
        //StartCoroutine(TickDebuffs());
    }


    public void OverrideHealth(int hp)
    {
        health_Current = hp;
        health_Max = hp;
    }

    public virtual void ResetSystem()
    {
        health_Current = health_Max;
        debuffList = new List<DebuffScript>();
        isDead = false;
    }

    /// <summary>
    /// deal damage to the gameobject
    /// damage rounded to the closest integer
    /// triggers death event if health reaches 0
    /// </summary>
    /// <param name="dmg"></param>
    /// <returns> health remaining </returns>
    public virtual int takeDamage(float dmg, int level, ElementTypes element, bool displayTakeDamageEffect = true)
    {

            health_Current -= Mathf.RoundToInt(dmg);
        if (!isDead)
        {
            print(name + " take damage: " + dmg);
            //updateHealthBar();
            displayDamage(dmg, element);
            if (displayTakeDamageEffect)
            {

                PlayTakeDamageEffect();
            }
        }

        CheckDead();
        return health_Current;

    }

    /// <summary>
    /// deal critical damage to the gameobject
    /// damage rounded to the closest integer
    /// triggers death event if health reaches 0
    /// </summary>
    /// <param name="dmg"></param>
    /// <returns> health remaining </returns>
    public virtual int takeDamageCritical(float dmg, int level, ElementTypes element, float multiplier = 1, bool displayTakeDamageEffect = true)
    {

            health_Current -= Mathf.RoundToInt(dmg * multiplier);
        if (!isDead)
        {
            print(name + " take " + element + " damage: " + dmg + " x " + multiplier);
            //updateHealthBar();
            displayDamageCritical(dmg * multiplier);
            if (displayTakeDamageEffect)
            {

                PlayTakeDamageEffect();
            }
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
    /// heal gameobject
    /// amount based on maximum health
    /// 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns> health remaining</returns>
    public virtual int healHealth_Percentage(float amount)
    {
        amount = Mathf.Clamp(amount, 0f, 1f);
        if (!isDead)
        {
            health_Current += Mathf.RoundToInt(amount*health_Max);
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
    /// heal gameobject
    /// amount based on missing health
    /// 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns> health remaining</returns>
    public virtual int healHealth_PercentageMissing(float amount)
    {
        amount = Mathf.Clamp(amount, 0f, 1f);
        if (!isDead)
        {
            health_Current += Mathf.RoundToInt(amount * (1-GetPercentageHealth()));
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

    protected virtual void displayDamage(float dmg, ElementTypes e = ElementTypes.PHYSICAL)
    {
        if (damagePopScript == null)
        {
            Debug.LogWarning(name + " missing damage numbers");
            return;
        }
        damagePopScript.displayDamage(dmg, e);

    }

    void displayDamageCritical(float dmg)
    {
        if (damagePopScript == null)
        {
            Debug.LogWarning(name + " missing damage numbers");
            return;
        }
        damagePopScript.displayCriticalDamage(dmg);
    }

    public virtual void PlayTakeDamageEffect()
    {

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
                damagePopScript.transform.SetParent(null);
                //damagePopScript.transform.position = transform.position;
                //groupParticleSystemScript.transform.position = transform.position;
                if (reatatchPopUps)
                {
                    StartCoroutine(reattach());
                }
            }
            gameObject.SetActive(false);
        }
        else if (destroyOnDeath)
        {
            if (detatchPopUps)
            {
                damagePopScript.transform.SetParent(null);
                if (reatatchPopUps)
                {
                    StartCoroutine(reattach());
                }
            }
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

        yield return new WaitForSeconds(3f);
        damagePopScript.transform.SetParent(transform);
        damagePopScript.transform.position = transform.position + popUpLocation;

    }


    public virtual void ApplyDebuff(DebuffScript debuff)
    {
        if (debuffList.Count <= 100)
        {
            debuffList.Add(debuff);
            debuff.ApplyEffect(this);
        }
        else
        {
            Debug.LogError(name + " debuff overload");
        }
    }

    public virtual void ApplyDebuff(FireEffectScript debuff)
    {
        ApplyDebuff(debuff as DebuffScript);
    }

    public virtual void ApplyDebuff(ShockEffectScript debuff)
    {
        ApplyDebuff(debuff as DebuffScript);
    }

    public virtual void ApplyDebuff(IceEffectScript debuff)
    {
        ApplyDebuff(debuff as DebuffScript);
    }

    public virtual void RemoveDebuff(FireEffectScript debuff = null)
    {
        RemoveDebuff(debuff);

    }
    public virtual void RemoveDebuff(ShockEffectScript debuff)
    {
        RemoveDebuff(debuff as DebuffScript);
    }
    public virtual void RemoveDebuff(IceEffectScript debuff)
    {
        RemoveDebuff(debuff as DebuffScript);
    }
    public virtual void RemoveDebuff(DebuffScript debuff = null)
    {

        debuffList.Remove(debuff);

    }


    private void OnEnable()
    {
        try
        {
            if (reatatchPopUps)
            {
                damagePopScript.transform.SetParent(transform);
                damagePopScript.transform.position = transform.position + popUpLocation;
            }
        }
        catch (System.Exception)
        {

        }

    }

    IEnumerator TickDebuffs()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < debuffList.Count; i++)
        {
            if (debuffList[i].TickEffect(Time.deltaTime))
            {
                i--;
            }
        }
    }

    public FireEffectScript CheckIsStillOnFire()
    {
        foreach (DebuffScript d in debuffList)
        {
            if (d is FireEffectScript)
            {
                return d as FireEffectScript;
            }
        }
        return null;

    }
    public IceEffectScript CheckIsStillOnIce(IceEffectScript iceComparedTo = null)
    {
        foreach (DebuffScript d in debuffList)
        {
            if (d is IceEffectScript)
            {
                if (iceComparedTo == null)
                {
                    return d as IceEffectScript;
                }
                if (!d.Equals(iceComparedTo))
                {
                    return d as IceEffectScript;
                }
            }
        }
        return null;

    }

    public float GetPercentageHealth()
    {
        return Mathf.Clamp((float)health_Current / (float)health_Max, 0f, 1f);
    }



}
