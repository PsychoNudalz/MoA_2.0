using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct RoomEnemySystemData
{
    public LevelSetData[] levelSetDatas;

    public RoomEnemySystemData(LevelSetData[] levelSetDatas)
    {
        this.levelSetDatas = levelSetDatas;
    }
}
[Serializable]
public struct LevelSetData
{
    public SpawnWaveData[] SpawnWaveDatas;

    public LevelSetData(SpawnWaveData[] spawnWaveDatas)
    {
        SpawnWaveDatas = spawnWaveDatas;
    }
}


[Serializable]
public struct SpawnWaveData
{
    public int condition;
    public SpawnSetData[] spawnSetDatas;

    public SpawnWaveData(int condition, SpawnSetData[] spawnSetDatas)
    {
        this.condition = condition;
        this.spawnSetDatas = spawnSetDatas;
    }
}

[Serializable]
public struct SpawnSetData
{
    public int enemyType;
    public int number;
    public int patrolIndex;

    public SpawnSetData(int enemyType, int number, int patrolIndex)
    {
        this.enemyType = enemyType;
        this.number = number;
        this.patrolIndex = patrolIndex;
    }
}
