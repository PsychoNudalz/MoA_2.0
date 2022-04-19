using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField]
    protected AILogic enemyAI;

    [SerializeField]
    protected EnemyAttacks enemyAttacks;

    [SerializeField]
    protected EnemyLifeSystem enemyLifeSystem;

    [SerializeField]
    private TargetEffectController enemyEffectController;

    [SerializeField]
    protected TargetSoundScript soundScript;

    [SerializeField]
    protected Animator animator;

    [Space]
    [Header("Death")]
    [SerializeField]
    private HealthPickupType healthPickupType;

    [SerializeField]
    private AnimationCurve healthSpawnChance;


    public AILogic EnemyAI => enemyAI;

    public EnemyAttacks EnemyAttacks => enemyAttacks;

    public EnemyLifeSystem EnemyLifeSystem => enemyLifeSystem;

    public TargetSoundScript SoundScript => soundScript;

    public TargetEffectController EnemyEffectController => enemyEffectController;

    public Animator Animator => animator;
    
    [ContextMenu("Awake")]

    void Awake()
    {
        if (!enemyAI)
        {
            enemyAI = GetComponent<AILogic>();
        }

        if (!enemyAttacks)
        {
            enemyAttacks = GetComponent<EnemyAttacks>();
        }

        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }


        if (!enemyLifeSystem)
        {
            enemyLifeSystem = GetComponent<EnemyLifeSystem>();
        }

        if (!enemyLifeSystem.TargetSoundScript)
        {
            enemyLifeSystem.TargetSoundScript = GetComponentInChildren<TargetSoundScript>();
        }
        
        
        if (!enemyAttacks.Animator)
        {
            enemyAttacks.Animator = animator;
        }

        if (!enemyAttacks.SoundScript)
        {
            enemyAttacks.SoundScript = soundScript;
        }

        if (!enemyAttacks.AILogic)
        {
            enemyAttacks.AILogic = enemyAI;
        }

        if (!enemyEffectController)
        {
            enemyEffectController = GetComponentInChildren<TargetEffectController>();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetPatrolZone(PatrolZone patrolZone)
    {
        enemyAI.SetPatrolZone(patrolZone);
    }

    public void SetSpawner(RoomEnemySystem roomEnemySystem)
    {
        enemyLifeSystem.Spawner = roomEnemySystem;
    }

    public virtual void SpawnEnemy()
    {
        gameObject.SetActive(true);
        Debug.Log($"Spawn: {name} ");
        enemyEffectController.SpawnEffect();
        soundScript.Play_Spawn();
    }
    
    public virtual void Stagger(bool b = true)
    {
        if (b)
        {
            animator.SetTrigger("StaggerTrigger");
            animator.SetBool("Stagger",true);
            enemyEffectController.SetStagger(true);
            soundScript.Play_Stagger();

        }
        else
        {
            animator.SetBool("Stagger",false);
            enemyEffectController.SetStagger(false);
            soundScript.Play_Stagger(false);


        }
    }

    public virtual void Death()
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.gameObject.SetActive(false);
        }
        SpawnHealth();
        enemyAI.ChangeState(AIState.Dead);
        animator.SetTrigger("Dead");
        soundScript.Play_Death();

    }

    public virtual void OnMove(Vector3 velocity)
    {
        animator.SetFloat("MoveSpeed",velocity.magnitude);
    }

    public virtual void SpawnHealth()
    {
        if (Random.Range(0f, 1f) <
            healthSpawnChance.Evaluate(PlayerMasterScript.current.PlayerLifeSystemScript.GetPercentageHealth()))
        {
            HealthManager.SpawnHealth(healthPickupType,transform.position);
        }
    }
}
