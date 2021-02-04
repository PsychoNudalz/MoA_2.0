using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : StateMachineBehaviour
{
   [SerializeField] GameObject  bossGO;
    [SerializeField] Transform boss;
    [SerializeField] Transform player;
    Vector3 playerDirection;
    

    private void Awake()
    {
        
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerDirection = player.position;
        boss.LookAt(player, playerDirection);
    }

}
