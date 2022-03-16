using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyHandler : EnemyHandler
{
    [SerializeField]
    private AILogic enemyAI;

    [SerializeField]
    private EnemyAttacks enemyAttacks;

    [SerializeField]
    private EnemyLifeSystem enemyLifeSystem;

    [SerializeField]
    private TargetSoundScript soundScript;

    [SerializeField]
    private Animator animator;
    // Start is called before the first frame update
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
