using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyHandler : EnemyHandler
{

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Stagger(bool b = true)
    {
        if (b)
        {
            animator.SetTrigger("StaggerTrigger");
            animator.SetBool("Stagger",true);
        }
        else
        {
            animator.SetBool("Stagger",false);

        }
    }

    public void Death()
    {
        enemyAI.ChangeState(AIState.Dead);
        animator.SetTrigger("Dead");
    }
}
