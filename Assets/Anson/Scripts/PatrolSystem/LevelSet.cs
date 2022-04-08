using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using UnityEngine;

/*
 * GenericPropertyJSON:{"name":"levelSets","type":-1,"arraySize":1,"arrayType":"LevelSet","children":[{"name":"Array","type":-1,"arraySize":1,"arrayType":"LevelSet","children":[{"name":"size","type":12,"val":1},{"name":"data","type":-1,"children":[{"name":"spawnWaves","type":-1,"arraySize":3,"arrayType":"SpawnWave","children":[{"name":"Array","type":-1,"arraySize":3,"arrayType":"SpawnWave","children":[{"name":"size","type":12,"val":3},{"name":"data","type":-1,"children":[{"name":"condition","type":0,"val":0},{"name":"totalEnemyCount","type":0,"val":19},{"name":"spawnSets","type":-1,"arraySize":6,"arrayType":"SpawnSet","children":[{"name":"Array","type":-1,"arraySize":6,"arrayType":"SpawnSet","children":[{"name":"size","type":12,"val":6},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Boss_T1"},{"name":"number","type":0,"val":1},{"name":"patrolIndex","type":0,"val":0},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846302}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Shooter_T1"},{"name":"number","type":0,"val":6},{"name":"patrolIndex","type":0,"val":1},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846324}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Tank_T1"},{"name":"number","type":0,"val":2},{"name":"patrolIndex","type":0,"val":1},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846324}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Clower_T1"},{"name":"number","type":0,"val":4},{"name":"patrolIndex","type":0,"val":2},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846346}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Drone_T1"},{"name":"number","type":0,"val":4},{"name":"patrolIndex","type":0,"val":3},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846372}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Drone_T1"},{"name":"number","type":0,"val":2},{"name":"patrolIndex","type":0,"val":4},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846394}"}]}]}]},{"name":"spawnedEnemies","type":-1,"arraySize":0,"arrayType":"PPtr<$EnemyHandler>","children":[{"name":"Array","type":-1,"arraySize":0,"arrayType":"PPtr<$EnemyHandler>","children":[{"name":"size","type":12,"val":0}]}]}]},{"name":"data","type":-1,"children":[{"name":"condition","type":0,"val":6},{"name":"totalEnemyCount","type":0,"val":12},{"name":"spawnSets","type":-1,"arraySize":5,"arrayType":"SpawnSet","children":[{"name":"Array","type":-1,"arraySize":5,"arrayType":"SpawnSet","children":[{"name":"size","type":12,"val":5},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Shooter_T2"},{"name":"number","type":0,"val":2},{"name":"patrolIndex","type":0,"val":2},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846346}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Shooter_T1"},{"name":"number","type":0,"val":2},{"name":"patrolIndex","type":0,"val":1},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846324}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Clower_T2"},{"name":"number","type":0,"val":4},{"name":"patrolIndex","type":0,"val":2},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846346}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Drone_T1"},{"name":"number","type":0,"val":2},{"name":"patrolIndex","type":0,"val":3},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846372}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Drone_T1"},{"name":"number","type":0,"val":2},{"name":"patrolIndex","type":0,"val":4},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846394}"}]}]}]},{"name":"spawnedEnemies","type":-1,"arraySize":0,"arrayType":"PPtr<$EnemyHandler>","children":[{"name":"Array","type":-1,"arraySize":0,"arrayType":"PPtr<$EnemyHandler>","children":[{"name":"size","type":12,"val":0}]}]}]},{"name":"data","type":-1,"children":[{"name":"condition","type":0,"val":6},{"name":"totalEnemyCount","type":0,"val":10},{"name":"spawnSets","type":-1,"arraySize":4,"arrayType":"SpawnSet","children":[{"name":"Array","type":-1,"arraySize":4,"arrayType":"SpawnSet","children":[{"name":"size","type":12,"val":4},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Shooter_T1"},{"name":"number","type":0,"val":2},{"name":"patrolIndex","type":0,"val":2},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846346}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Tank_T1"},{"name":"number","type":0,"val":2},{"name":"patrolIndex","type":0,"val":1},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846324}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Clower_T2"},{"name":"number","type":0,"val":4},{"name":"patrolIndex","type":0,"val":2},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846346}"}]},{"name":"data","type":-1,"children":[{"name":"enemyType","type":7,"val":"Enum:Drone_T1"},{"name":"number","type":0,"val":2},{"name":"patrolIndex","type":0,"val":3},{"name":"patrolZone","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":-846372}"}]}]}]},{"name":"spawnedEnemies","type":-1,"arraySize":0,"arrayType":"PPtr<$EnemyHandler>","children":[{"name":"Array","type":-1,"arraySize":0,"arrayType":"PPtr<$EnemyHandler>","children":[{"name":"size","type":12,"val":0}]}]}]}]}]}]}]}]}
 */

[Serializable]
public struct LevelSet
{
    public SpawnWave[] spawnWaves;

    public LevelSet(LevelSetData levelSetData)
    {
        spawnWaves = new SpawnWave[levelSetData.SpawnWaveDatas.Length];
        for (var i = 0; i < spawnWaves.Length; i++)
        {
            spawnWaves[i] = new SpawnWave(levelSetData.SpawnWaveDatas[i]);
        }

    }

    public LevelSet(SpawnWave[] newSpawnWaves)
    {
        spawnWaves = newSpawnWaves;
        
    }

    public LevelSetData GetData()
    {
        SpawnWaveData[] spawnWaveDatas = new SpawnWaveData[spawnWaves.Length];
        for (var i = 0; i < spawnWaves.Length; i++)
        {
            spawnWaveDatas[i] = spawnWaves[i].GetData();
            
        }

        return new LevelSetData(spawnWaveDatas);
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

    public SpawnSet(SpawnSetData spawnSetData)
    {
        this.enemyType = (EnemyType)spawnSetData.enemyType;
        this.number = spawnSetData.number;
        this.patrolIndex = spawnSetData.patrolIndex;
        this.patrolZone = null;
    }


    public SpawnSetData GetData()
    {
        return new SpawnSetData((int)enemyType, number, patrolIndex);
    }
    
    
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

    public int Condition => condition;

    public SpawnSet[] SpawnSets => spawnSets;

    public List<EnemyHandler> SpawnedEnemies => spawnedEnemies;

    public SpawnWave(SpawnWaveData spawnWaveData)
    {
        this.condition = spawnWaveData.condition;
        spawnSets = new SpawnSet[spawnWaveData.spawnSetDatas.Length];
        for (int i = 0; i < spawnSets.Length; i++)
        {
            spawnSets[i] = new SpawnSet(spawnWaveData.spawnSetDatas[i]);
        }
    }

    public SpawnWaveData GetData()
    {
        SpawnSetData[] spawnSetDatas = new SpawnSetData[spawnSets.Length];
        for (var i = 0; i < spawnSets.Length; i++)
        {
            spawnSetDatas[i] = spawnSets[i].GetData();
            
        }

        return new SpawnWaveData(condition, spawnSetDatas);
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

