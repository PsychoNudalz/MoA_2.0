using UnityEngine;
using UnityEngine.AI;

public class Boss_AI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    private void Awake()
    {
        
    }
    private void Update()
    {
        MoveTwordsPlayer();
    }

    void MeleeAttack() 
    {
    
    }

    void RangedAttack() 
    {
    
    }

    void MissileAttack() 
    {
    
    }

    void LookAtPlayer() 
    {
    
    }

    void AttackCooldown() 
    {
    
    }

    void MoveTwordsPlayer() 
    {
        Vector3 playerLocation = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        agent.SetDestination(playerLocation);
    }

    void MoveAwayFromPlayer() 
    {
    
    }

    void CheckLineOfSight() 
    {
    
    }

    void Death() 
    { 
    
    }
    
    

}
