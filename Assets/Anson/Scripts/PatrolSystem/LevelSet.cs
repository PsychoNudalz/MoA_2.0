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

    [Space(15f)]
    [SerializeField]
    private int totalEnemyCount = 0;
    [SerializeField]
    SpawnSet[] spawnSets;

    [SerializeField]
    List<EnemyHandler> spawnedEnemies;

    public int TotalEnemyCount
    {
        get => totalEnemyCount;
        set => totalEnemyCount = value;
    }

    public int InitialiseSpawn(Transform parent, RoomEnemySystem roomEnemySystem)
    {
        EnemyHandler enemyHandlerTemp;
        spawnedEnemies = new List<EnemyHandler>();
        int totalEnemy = 0;

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
                totalEnemy++;
            }
        }

        return totalEnemy;
    }

    public bool ConditionMet(int remainingEnemies)
    {
        return remainingEnemies <= condition;
    }

    public int StartWave()
    {
        int totalEnemy = 0;
        foreach (EnemyHandler spawnedEnemy in spawnedEnemies)
        {
            spawnedEnemy.gameObject.SetActive(true);
            spawnedEnemy.SpawnEnemy();
            totalEnemy++;
        }

        return totalEnemy;
    }

    public int GetEnemyCount()
    {
        int i = 0;
        foreach (SpawnSet spawnSet in spawnSets)
        {
            i += spawnSet.number;
        }

        return i;
    }

    
    public void AutoSetPatrolZone(PatrolManager patrolManager,bool toAll = false)
    {
        for (var i = 0; i < spawnSets.Length; i++)
        {
            SpawnSet spawnSet = spawnSets[i];
            if (toAll || spawnSet.patrolZone == null)
            {
                spawnSet.patrolZone = patrolManager.GetZone(spawnSet.patrolIndex);
            }

            spawnSets[i] = spawnSet;
        }
    }public void AutoSetPatrolZoneIndex(PatrolManager patrolManager,bool toAll = false)
    {
        for (var i = 0; i < spawnSets.Length; i++)
        {
            SpawnSet spawnSet = spawnSets[i];
            if (spawnSet.patrolZone != null&&(toAll || spawnSet.patrolIndex<0))
            {
                spawnSet.patrolIndex = patrolManager.GetZoneIndex(spawnSet.patrolZone);
            }
            spawnSets[i] = spawnSet;

        }
    }
}