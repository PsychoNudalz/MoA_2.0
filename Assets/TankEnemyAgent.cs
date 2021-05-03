using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemyAgent : MonoBehaviour
{

    [SerializeField] private float minAttackDelay = 1f;
    [SerializeField] private float maxAttackDelay = 5f;
    //[SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;
    
    private Animator TankEnemyAnimator;
    private float currentAttackTimer;
    private GameObject player;
    private AIGunDamageScript gun;

    private bool isShooting;
    private bool IsStaggering;
    [SerializeField] private bool IsDead = false;

    [Header("Tank")]
    [SerializeField] GunDamageScript gunDamageScript;

    // Start is called before the first frame update
    void Start()
    {
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        TankEnemyAnimator = GetComponent<Animator>();
        gun = GetComponentInChildren<AIGunDamageScript>();
        ResetAttackTimer();
    }

    /*
     * Set next attack timer to random value between min and max values set
     */
    private void ResetAttackTimer()
    {
        currentAttackTimer = Mathf.Clamp(Random.Range(minAttackDelay, maxAttackDelay), 1.5f, maxAttackDelay);
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget();
        if (!IsStaggering && !IsDead)
        {

            if (currentAttackTimer <= 0 && !isShooting)
            {
                RaycastHit hit;
                Vector3 playerDirection = player.transform.position - firePoint.transform.position;
                Debug.DrawRay(firePoint.transform.position, playerDirection, Color.red, 2f);
                if (Physics.Raycast(firePoint.transform.position, playerDirection, out hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        StartCoroutine(Shoot(2.2f));
                    }
                }
            }
            if (!isShooting)
            {
                currentAttackTimer -= Time.deltaTime;
                
            }

        }
        if (IsDead)
        {
            transform.parent.GetComponent<EnemySpawner>().RemoveFromSpawnedEnemies(this.gameObject);
            GameObject.Destroy(this.transform.gameObject, 5f);
            transform.GetComponentInParent<EnemySpawner>().ResetSpawnCountdown();
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        gun.transform.LookAt(player.transform);
    }

    IEnumerator Shoot(float delay)
    {
        isShooting = true;
        FaceTarget();
        TankEnemyAnimator.SetTrigger("Shoot");
        yield return new WaitForSeconds(delay);
        ResetAttackTimer();
        isShooting = false;
        EndFire();

    }

    public void DeathAnimation()
    {
        IsDead = true;
        TankEnemyAnimator.SetBool("IsDead", true);
    }

    public void Stagger()
    {
        if (!IsStaggering)
        {
            StartCoroutine(StaggerDelay());
        }
    }

    IEnumerator StaggerDelay()
    {
        IsStaggering = true;
        TankEnemyAnimator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        IsStaggering = false;

    }

    public void Fire()
    {
        gunDamageScript.Fire(true);
    }

    public void EndFire()
    {
        gunDamageScript.Fire(false);

    }
}
