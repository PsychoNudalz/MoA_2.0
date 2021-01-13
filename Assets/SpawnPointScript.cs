using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    [SerializeField] GameObject targetGO;
    [SerializeField] TargetLifeSystem targetLifeSystem;
    public ShootingRangeScript currentSR;
    [SerializeField] bool isDead = true;

    private void FixedUpdate()
    {
        if (!isDead && targetLifeSystem != null &&targetLifeSystem.IsDead)
        {
            isDead = true;
            currentSR.increaseWaveKills();
        }
    }


    public void Spawn(int health = 0)
    {
        //Despawn();

        isDead = false;
        //targetLifeSystem = Instantiate(targetGO, transform.position+new Vector3(0,1,0), transform.rotation, transform).GetComponent<TargetLifeSystem>();
        targetGO.SetActive(true);
        //targetLifeSystem = targetGO.GetComponent<TargetLifeSystem>();
        targetLifeSystem.ResetSystem();

        if (health != 0)
        {
            targetLifeSystem.OverrideHealth(health);
        }

    }

    public void Despawn()
    {
        if (targetLifeSystem != null)
        {
            isDead = true;
            targetGO.SetActive(false);
            //Destroy(targetLifeSystem.gameObject);
        }
    }
}
