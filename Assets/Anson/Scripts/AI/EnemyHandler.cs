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
    protected TargetSoundScript soundScript;

    [SerializeField]
    protected Animator animator;


    public AILogic EnemyAI => enemyAI;

    public EnemyAttacks EnemyAttacks => enemyAttacks;

    public EnemyLifeSystem EnemyLifeSystem => enemyLifeSystem;

    public TargetSoundScript SoundScript => soundScript;

    public Animator Animator => animator;
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

        if (!enemyAttacks.Animator)
        {
            enemyAttacks.Animator = animator;
        }

        if (!enemyLifeSystem)
        {
            enemyLifeSystem = GetComponent<EnemyLifeSystem>();
        }

        if (!enemyLifeSystem.TargetSoundScript)
        {
            enemyLifeSystem.TargetSoundScript = GetComponentInChildren<TargetSoundScript>();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
