using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using UnityEngine;


[Serializable]
public struct LevelSet
{
    public SpawnWave[] spawnWaves;


    public LevelSet(SpawnWave[] newSpawnWaves)
    {
        spawnWaves = newSpawnWaves;
    }
}

[Serializable]
public struct SpawnSet
{
    public EnemyType enemyType;

    [Min(1)]
    public int number;

    public int patrolIndex;
    public PatrolZone patrolZone;
}

[Serializable]
public class SpawnWave
{
    [SerializeField]
    int condition;

    [SerializeField]
    SpawnSet[] spawnSets;

    [SerializeField]
    List<EnemyHandler> spawnedEnemies;

    public void InitialiseSpawn(Transform parent, RoomEnemySystem roomEnemySystem)
    {
        EnemyHandler enemyHandlerTemp;
        spawnedEnemies = new List<EnemyHandler>();
        for (int i = 0; i < spawnSets.Length; i++)
        {
            SpawnSet spawnSet = spawnSets[i];
            for (int j = 0; j < spawnSet.number; j++)
            {
                enemyHandlerTemp = GameObject.Instantiate(EnemyManager.GetEnemyS(spawnSet.enemyType), parent)
                    .GetComponent<EnemyHandler>();
                spawnedEnemies.Add(enemyHandlerTemp);
                if (spawnSet.patrolZone == null)
                {
                    spawnSet.patrolZone = roomEnemySystem.PatrolManager.PatrolZones[
                        Mathf.Min(roomEnemySystem.PatrolManager.PatrolZones.Length - 1, spawnSet.patrolIndex)];
                }

                enemyHandlerTemp.transform.position = spawnSet.patrolZone.GetRandomPoint();
                enemyHandlerTemp.SetPatrolZone(spawnSet.patrolZone);
                enemyHandlerTemp.gameObject.SetActive(false);
                enemyHandlerTemp.SetSpawner(roomEnemySystem);
            }
        }
    }

    public int StartWave()
    {
        int totalEnemy = 0;
        foreach (SpawnSet spawnSet in spawnSets)
        {
            foreach (EnemyHandler spawnedEnemy in spawnedEnemies)
            {
                spawnedEnemy.gameObject.SetActive(true);
                spawnedEnemy.SpawnEnemy();
                totalEnemy++;
            }
        }

        return totalEnemy;
    }
}