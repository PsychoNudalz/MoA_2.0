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
    

    // Start is called before the first frame update
    void Start()
    {
        enemyPrefabs = new GameObject[2];
        enemyPrefabs[0] = stoneEnemy;
        enemyPrefabs[1] = shootingEnemy;
        spawnCountdown = delayBetweenSpawns;
        if (isSpawning)
        {
            SpawnEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawning)
        {
            if (enemiesSpawned < numberOfEnemies)
            {
                spawnCountdown -= Time.deltaTime;
                if (spawnCountdown <= 0)
                {
                    SpawnEnemy();
                }
            }
            /*
            else
            {
                GameObject.Destroy(this.gameObject);
            }*/
        }
    }

    private void SpawnEnemy()
    {
        enemyToSpawn = GetEnemyToSpawn();
        GameObject.Instantiate(enemyToSpawn,transform.position,transform.rotation,transform);
        enemiesSpawned++;
        spawnCountdown = delayBetweenSpawns;
    }

    private GameObject GetEnemyToSpawn()
    {
        switch (enemyType)
        {
            case EnemyType.StoneEnemy:
                return enemyPrefabs[0];
            case EnemyType.ShootingEnemy:
                return enemyPrefabs[1];
            default:
                int index = Random.Range(0, enemyPrefabs.Length);
                print(index);
                return enemyPrefabs[index];
        }
        
    }

    public void StartSpawning()
    {
        isSpawning = true;
    }
}
