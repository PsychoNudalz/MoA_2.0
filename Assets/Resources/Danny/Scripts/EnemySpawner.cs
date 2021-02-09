using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

enum EnemyType {StoneEnemy,ShootingEnemy,RandomEnemies};
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private float delayBetweenSpawns;
    [Space]
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject stoneEnemy;
    [SerializeField] private GameObject shootingEnemy;
    [SerializeField] private bool isSpawning = false;
    private GameObject[] enemyPrefabs;
    private GameObject enemyToSpawn;
    private int enemiesSpawned;
    private float spawnCountdown;
    private int numberOfWaypoints;
    

    // Start is called before the first frame update
    void Start()
    {
        /*
         * Set number of waypoints
         */
        numberOfWaypoints = transform.parent.GetComponentsInChildren<EnemyWaypoint>().Length -1;
        /*
         * Save enemy prefabs to spawn in array
         */
        enemyPrefabs = new GameObject[2];
        enemyPrefabs[0] = stoneEnemy;
        enemyPrefabs[1] = shootingEnemy;
        /*
         * Set spawn countdown
         */
        spawnCountdown = delayBetweenSpawns;
        /*
         * If spawning on start spawn first enemy
         */
        if (isSpawning)
        {
            SpawnEnemy();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
    
        /*
         * If spawning started...
         */
        if (isSpawning)
        {
            /*
             * If enemies left to spawn and delay time reached
             * spawn another enemy
             */
            if (spawnCountdown <= 0)
            {
                SpawnEnemy();
            }
            else if (enemiesSpawned < numberOfEnemies)
            {
                spawnCountdown -= Time.deltaTime;
            }
            else
            {
                /*
                 * If all enemies spawned and been killed
                 * remove spawner
                 */
                if(transform.childCount == 0)
                {
                    GameObject.Destroy(this.gameObject);
                }
            }
        }
    }

    /*
     * Get an enemy prefab to spawn, spawn it, 
     * increment spawn count and reset delay countdown
     */
    private void SpawnEnemy()
    {
        enemyToSpawn = GetEnemyToSpawn();
        if (enemyToSpawn.gameObject.name.Equals("ShootingEnemy"))
        {
            if (SufficientWaypoints())
            {
                enemiesSpawned++;
                GameObject.Instantiate(enemyToSpawn, transform.position, transform.rotation, transform);
                spawnCountdown = delayBetweenSpawns;
            }
            else
            {
                Debug.LogWarning("Insufficeient waypoints for shooting enemy spawn");
                spawnCountdown = Random.Range(0f,1f);
            }
        }
        else
        {
            GameObject.Instantiate(enemyToSpawn,transform.position,transform.rotation,transform);
            enemiesSpawned++;
            spawnCountdown = delayBetweenSpawns;
        }

        bool SufficientWaypoints()
        {
            return transform.parent.GetComponentsInChildren<ShootingEnemyAgent>().Length + 1 < numberOfWaypoints;
        }
    }

    /*
     * Return the set enemy prefab or random one if random selected
     */
    private GameObject GetEnemyToSpawn()
    {
        print("getting enemy prefab to spawn");
        switch (enemyType)
        {
            case EnemyType.StoneEnemy:
                return enemyPrefabs[0];
            case EnemyType.ShootingEnemy:
                return enemyPrefabs[1];
            default:
                int index = Random.Range(0, enemyPrefabs.Length);
                return enemyPrefabs[index];
        }
        
    }

    /*
     * Start spawning enemies instantly if enemies left to spawn
     */
    public void StartSpawning()
    {
        isSpawning = true;
        if(numberOfEnemies > 0)
        {
            SpawnEnemy();
        }
    }
}
