using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

enum EnemyTypeOld
{
    StoneEnemy,
    ShootingEnemy,
    TankEnemy,
    BossEnemy,
    RandomEnemies
};

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyTypeOld spawnerEnemyTypeOld;

    [SerializeField]
    private int numberOfEnemies;

    [SerializeField]
    private int maxEnemies = 3;

    [SerializeField]
    private float delayBetweenSpawns = 5;

    [Space]
    [Header("Enemy Prefabs")]
    [SerializeField]
    private GameObject stoneEnemy;

    [SerializeField]
    private GameObject shootingEnemy;

    [SerializeField]
    private GameObject TankEnemy;

    [SerializeField]
    private GameObject BossEnemy;

    [SerializeField]
    private bool isSpawning = false;

    private GameObject[] enemyPrefabs;
    private GameObject enemyToSpawn;
    private int enemiesSpawned;
    private float spawnCountdown;
    private RoomEnemySystem roomSystem;
    private Queue<GameObject> enemiesToSpawn;
    private List<GameObject> spawnedEnemies;

    public int NumberOfEnemies
    {
        get => numberOfEnemies;
    }

    internal void UpdateEnemyNumber()
    {
        throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        // roomSystem = transform.parent.GetComponent<RoomEnemySystem>();
        // /*
        //  * Save enemy prefabs to spawn in array
        //  */
        // enemyPrefabs = new GameObject[2];
        // enemyPrefabs[0] = stoneEnemy;
        // enemyPrefabs[1] = shootingEnemy;
        // enemiesToSpawn = new Queue<GameObject>();
        // spawnedEnemies = new List<GameObject>();
        // if (spawnerEnemyTypeOld.Equals(EnemyTypeOld.BossEnemy) || spawnerEnemyTypeOld.Equals(EnemyTypeOld.TankEnemy))
        // {
        //     numberOfEnemies = 1;
        // }
        //
        // for (int i = 0; i < numberOfEnemies; i++)
        // {
        //     CreateEnemy();
        // }
        //
        // /*
        //  * Set spawn countdown
        //  */
        // ResetSpawnCountdown();
        // /*
        //  * If spawning on start spawn first enemy
        //  */
        // if (isSpawning)
        // {
        //     SpawnEnemy();
        // }
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
             * If enemies left to spawn and delay cooldown reached
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
            else if (transform.childCount == 0)
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
        GameObject enemySpawned =
            GameObject.Instantiate(enemyToSpawn, transform.position, transform.rotation, transform);
        enemySpawned.name = EnumToString.GetEnemyStringFromEnum(enemySpawned.GetComponent<EnemyTypeScript>().EnemyTypeOld);
        enemySpawned.SetActive(false);
        IncrementEnemies();
        enemiesToSpawn.Enqueue(enemySpawned);
    }

    private void IncrementEnemies()
    {
        roomSystem.IncrementEnemies();
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
        if (spawnedEnemies.Count < maxEnemies && enemiesToSpawn.Count > 0)
        {
            GameObject enemy = enemiesToSpawn.Dequeue();
            enemy.SetActive(true);
            enemiesSpawned++;
            spawnedEnemies.Add(enemy);

            if (spawnCountdown < delayBetweenSpawns / 5)
            {
                ResetSpawnCountdown();
            }
        }
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
        switch (spawnerEnemyTypeOld)
        {
            case EnemyTypeOld.StoneEnemy:
                return enemyPrefabs[0];
            case EnemyTypeOld.ShootingEnemy:
                return enemyPrefabs[1];
            case EnemyTypeOld.TankEnemy:
                return TankEnemy;
            case EnemyTypeOld.BossEnemy:
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
        if (numberOfEnemies > 0)
        {
            SpawnEnemy();
        }
    }
}