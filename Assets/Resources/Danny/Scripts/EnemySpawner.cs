using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyType {StoneEnemy,ShootingEnemy};
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyType enemyType;
    [SerializeField]
    private int numberOfEnemies;
    [SerializeField]
    private float delayBetweenSpawns;

    private GameObject enemyToSpawn;
    private int enemiesSpawned;
    private float spawnCountdown;

    // Start is called before the first frame update
    void Start()
    {
        spawnCountdown = delayBetweenSpawns;
        switch (enemyType)
        {
            case EnemyType.StoneEnemy:
                enemyToSpawn = (GameObject)Resources.Load("Danny/Prefabs/StoneEnemy");
                break;
            default:
                Debug.LogError("Failed to load " + enemyType.ToString() + " prefab");
                break;
        }
        if(numberOfEnemies > 0)
        {
            SpawnEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesSpawned < numberOfEnemies && enemyToSpawn != null)
        {
            spawnCountdown -= Time.deltaTime;
            if (spawnCountdown <= 0)
            {
                SpawnEnemy();
            }
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
        
    }

    private void SpawnEnemy()
    {
        GameObject.Instantiate(enemyToSpawn, transform.position, transform.rotation);
        enemiesSpawned++;
        spawnCountdown = delayBetweenSpawns;
    }
}
