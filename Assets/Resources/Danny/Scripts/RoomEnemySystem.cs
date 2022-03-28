﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RoomEnemySystem : MonoBehaviour
{
    [SerializeField]
    private bool isActivated = false;
    
    EnemySpawner[] roomSpawners;

    PlayerUIScript UIScript;

    [SerializeField]
    private PatrolManager patrolManager;

    private float lastCheckTimeNow = 0f;

    [SerializeField]
    private float checkWaveTime = 2f;


    [Header("Enemy Waves")]
    [SerializeField]
    private LevelSet[] levelSets;

    [Space(5f)]
    [SerializeField]
    private int difficulty = 0;


    [Space(10f)]
    [SerializeField]
    private int waveIndex = -1;

    [SerializeField]
    private int enemyCountCurrent;

    [SerializeField]
    private int enemyCountTotal = 0;

    public PatrolManager PatrolManager => patrolManager;

    private void Awake()
    {
        roomSpawners = GetComponentsInChildren<EnemySpawner>();
        UIScript = FindObjectOfType<PlayerUIScript>();
    }

    private void FixedUpdate()
    {
        if (isActivated)
        {
            if (Time.time - lastCheckTimeNow > checkWaveTime)
            {
                lastCheckTimeNow = Time.time;
                StartNextWave();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var levelSet in levelSets)
        {
            foreach (var spawnWave in levelSet.spawnWaves)
            {
                spawnWave.TotalEnemyCount = spawnWave.GetEnemyCount();
            }
        }
    }

    private void UpdateEnemyNumberDisplay(bool start = false)
    {
        if (start)
        {
            UIScript.SetEnemiesRemainingText(enemyCountTotal, false);
            UIScript.SetPortalIconActive(false);
        }
        else if (enemyCountTotal == 0)
        {
            UIScript.SetEnemiesRemainingText(enemyCountTotal, true);
        }
        else
        {
            UIScript.SetEnemiesRemainingText(enemyCountTotal, false);
        }
    }

    /// <summary>
    /// Start the Room Spawning
    /// </summary>
    /// <param name="difficulty"></param>
    [ContextMenu("Start Room Spawners")]
    public void StartRoomSpawners(int difficulty = 0)
    {
        //OLD SYSTEM//
        if (roomSpawners.Length > 0)
        {
            foreach (EnemySpawner spawner in roomSpawners)
            {
                spawner.StartSpawning();
                UpdateEnemyNumberDisplay(true);
            }
        }
        else
        {
            Debug.LogWarning("No spawners found");
        }

        //NEW SYSTEM//
        isActivated = true;
    }
    
    

    public void StartNextWave()
    {
        if (waveIndex+1 < levelSets[difficulty].spawnWaves.Length)
        {
            SpawnWave nextWave = levelSets[difficulty].spawnWaves[waveIndex+1];
            if (nextWave.ConditionMet(enemyCountCurrent))
            {
                Debug.Log($"Spawning Wave: {waveIndex}");
                IncrementEnemies(nextWave.StartWave());
                waveIndex++;
            }
        }
    }

    [ContextMenu("Initialise Spawn")]
    public void InitialiseSpawns()
    {
        InitialisePatrolManager();


        RemoveChildren();
        int i = 1;
        GameObject tempParent;
        enemyCountTotal = 0;

        foreach (SpawnWave spawnWave in levelSets[difficulty].spawnWaves)
        {
            tempParent = Instantiate(new GameObject(), transform);
            tempParent.name = $"----WAVE {i}----";
            enemyCountTotal
                += spawnWave.InitialiseSpawn(tempParent.transform, this);
            i++;
        }
    }

    private void InitialisePatrolManager()
    {
        try
        {
            if (patrolManager.PatrolZones == null || patrolManager.PatrolZones.Length == 0 ||
                patrolManager.PatrolZones[0].PointPositions.Count == 0)
            {
                patrolManager.InitialiseAllZones();
            }
        }
        catch (NullReferenceException e)
        {
            patrolManager.InitialiseAllZones();
        }
    }


    [ContextMenu("Remove All Children")]
    public void RemoveChildren()
    {
        foreach (Transform children in transform.GetComponentsInChildren<Transform>())
        {
            if (children && !children.transform.Equals(transform))
            {
                DestroyImmediate(children.gameObject);
            }
        }
    }

    public bool IsRoomClear()
    {
        int spawnersLeft = GetComponentsInChildren<EnemySpawner>().Length;
        bool temp = spawnersLeft == 0 && enemyCountTotal == 0;
        return temp;
    }

    internal void IncrementEnemies()
    {
        IncrementEnemies(1);
    }

    internal void IncrementEnemies(int i)
    {
        enemyCountCurrent += i;
    }

    internal void DecrementEnemies()
    {
        enemyCountTotal--;
        enemyCountCurrent--;
        UpdateEnemyNumberDisplay();
    }

    public int GetEnemyInWave(int i, int levelSet = -1)
    {
        if (levelSet < 0)
        {
            levelSet = difficulty;
        }
        
        i = Mathf.Min(Mathf.Max(0, i), levelSets[levelSet].spawnWaves.Length-1);

        SpawnWave currentWave = levelSets[difficulty].spawnWaves[i];
        
        return currentWave.GetEnemyCount();
    }
    
    
    
    [ContextMenu("AutoSetPatrolZone")]
    public void AutoSetPatrolZone()
    {

        foreach (LevelSet levelSet in levelSets)
        {
            foreach (SpawnWave spawnWave in levelSet.spawnWaves)
            {
                spawnWave.AutoSetPatrolZone(patrolManager);
            }
        }
    }
    
    [ContextMenu("AutoSetPatrolZone_FORCE")]
    public void AutoSetPatrolZone_FORCE()
    {

        foreach (LevelSet levelSet in levelSets)
        {
            foreach (SpawnWave spawnWave in levelSet.spawnWaves)
            {
                spawnWave.AutoSetPatrolZone(patrolManager,true);
            }
        }
    }[ContextMenu("AutoSetPatrolZoneIndex")]
    public void AutoSetPatrolZoneIndex()
    {

        foreach (LevelSet levelSet in levelSets)
        {
            foreach (SpawnWave spawnWave in levelSet.spawnWaves)
            {
                spawnWave.AutoSetPatrolZoneIndex(patrolManager);
            }
        }
    }
    
    [ContextMenu("AutoSetPatrolZoneIndex_FORCE")]
    public void AutoSetPatrolZoneIndex_FORCE()
    {

        foreach (LevelSet levelSet in levelSets)
        {
            foreach (SpawnWave spawnWave in levelSet.spawnWaves)
            {
                spawnWave.AutoSetPatrolZoneIndex(patrolManager,true);
            }
        }
    }
}