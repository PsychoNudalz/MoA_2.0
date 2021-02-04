using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Right_Attack : StateMachineBehaviour
{

    [SerializeField] public float bossSpeed;
    Transform player;
    Rigidbody bossRB;
    
   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     player =  GameObject.FindGameObjectWithTag("player").transform;
       bossRB = animator.GetComponent<Rigidbody>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 target = new Vector3(player.position.x, bossRB.position.y, player.position.z);
       Vector3 newPos = Vector3.MoveTowards(bossRB.position, target, bossSpeed * Time.deltaTime);
        bossRB.MovePosition(newPos);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    
}
