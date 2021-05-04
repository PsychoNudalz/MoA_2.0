using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

enum EnemyType {StoneEnemy,ShootingEnemy,TankEnemy,BossEnemy,RandomEnemies};
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyType SpawnerEnemyType;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private int maxEnemies = 3;
    [SerializeField] private float delayBetweenSpawns = 5;
    [Space]
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject stoneEnemy;
    [SerializeField] private GameObject shootingEnemy;
    [SerializeField] private GameObject TankEnemy;
    [SerializeField] private GameObject BossEnemy;
    [SerializeField] private bool isSpawning = false;
    private GameObject[] enemyPrefabs;
    private GameObject enemyToSpawn;
    private int enemiesSpawned;
    private float spawnCountdown;
    private RoomEnemySystem roomSystem;
    private Queue<GameObject> enemiesToSpawn;
    private List<GameObject> spawnedEnemies;

    internal void UpdateEnemyNumber()
    {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        roomSystem = transform.parent.GetComponent<RoomEnemySystem>();
        /*
         * Save enemy prefabs to spawn in array
         */
        enemyPrefabs = new GameObject[2];
        enemyPrefabs[0] = stoneEnemy;
        enemyPrefabs[1] = shootingEnemy;
        enemiesToSpawn = new Queue<GameObject>();
        spawnedEnemies = new List<GameObject>();
        if (SpawnerEnemyType.Equals(EnemyType.BossEnemy) || SpawnerEnemyType.Equals(EnemyType.TankEnemy)){
            numberOfEnemies = 1;
        }
        for (int i = 0; i < numberOfEnemies; i++)
        {
            CreateEnemy();
        }
        /*
         * Set spawn countdown
         */
        ResetSpawnCountdown();
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
            else if(transform.childCount == 0)
            {
                //If all enemies from spawner killed destroy spawner
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    internal void ResetSpawnCountdown()
    {
        spawnCountdown = delayBetweenSpawns;
    }
    private void CreateEnemy()
    {
        enemyToSpawn = GetEnemyToSpawn();
        GameObject enemySpawned = GameObject.Instantiate(enemyToSpawn, transform.position, transform.rotation, transform);
        enemySpawned.name = EnumToString.GetEnemyStringFromEnum(enemySpawned.GetComponent<EnemyTypeScript>().EnemyType);
        enemySpawned.SetActive(false);
        enemiesToSpawn.Enqueue(enemySpawned);
    }

    internal void RemoveFromSpawnedEnemies(GameObject enemyToRemove)
    {
        spawnedEnemies.Remove(enemyToRemove);
        ResetSpawnCountdown();
    }

    /*
     * Get an enemy prefab to spawn, spawn it, 
     * increment spawn count and reset delay countdown
     */
    private void SpawnEnemy()
    {
        /*
        if (SpawnerEnemyType.Equals(EnemyType.TankEnemy))
        {
            if(spawnedEnemies.Count == 0 && enemiesSpawned < numberOfEnemies)
            {
                GameObject enemy = enemiesToSpawn.Dequeue();
                enemy.SetActive(true);
                enemiesSpawned++;
                spawnedEnemies.Add(enemy);
                IncrementEnemies();
                ResetSpawnCountdown();
            }
        }
        else
        {*/
            if(spawnedEnemies.Count < maxEnemies && enemiesSpawned < numberOfEnemies)
            {
                GameObject enemy = enemiesToSpawn.Dequeue();
                enemy.SetActive(true);
                enemiesSpawned++;
                spawnedEnemies.Add(enemy);
                IncrementEnemies();
                if(spawnCountdown < delayBetweenSpawns / 5)
                {
                    ResetSpawnCountdown();
                }
            }
       // }
    }

    private void IncrementEnemies()
    {
        roomSystem.IncrementEnemies();
    }

    internal void DecrementEnemies()
    {
        roomSystem.DecrementEnemies();
    }

    /*
     * Return the set enemy prefab or random one if random selected
     */
    private GameObject GetEnemyToSpawn()
    {
        //print("getting enemy prefab to spawn");
        switch (SpawnerEnemyType)
        {
            case EnemyType.StoneEnemy:
                return enemyPrefabs[0];
            case EnemyType.ShootingEnemy:
                return enemyPrefabs[1];
            case EnemyType.TankEnemy:
                return TankEnemy;
            case EnemyType.BossEnemy:
                return BossEnemy;
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
