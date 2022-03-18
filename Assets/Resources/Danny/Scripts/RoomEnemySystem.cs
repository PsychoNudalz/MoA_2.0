using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RoomEnemySystem : MonoBehaviour
{
    EnemySpawner[] roomSpawners;
    private int enemyCountTotal = 0;
    PlayerUIScript UIScript;

    [SerializeField]
    private PatrolManager patrolManager;

    [Header("Enemy Waves")]
    [SerializeField]
    private LevelSet[] levelSets;

    [Space(5f)]
    [SerializeField]
    private int difficulty = 0;


    [Space(10f)]
    [SerializeField]
    private int waveIndex = 0;

    [SerializeField]
    private int enemyCountCurrent;

    public PatrolManager PatrolManager => patrolManager;

    private void Awake()
    {
        roomSpawners = GetComponentsInChildren<EnemySpawner>();
        UIScript = FindObjectOfType<PlayerUIScript>();
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
    public void StartRoomSpawners(int difficulty)
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
    }

    [ContextMenu("Initialise Spawn")]
    public void InitialiseSpawns()
    {
        try
        {
            if (patrolManager.PatrolZones==null||patrolManager.PatrolZones.Length==0 || patrolManager.PatrolZones[0].PointPositions.Count == 0)
            {
                patrolManager.InitialiseAllZones();
            }
        }
        catch (NullReferenceException e)
        {
            patrolManager.InitialiseAllZones();

        }
        

        RemoveChildren();
        int i = 1;
        GameObject tempParent;
        foreach (SpawnWave spawnWave in levelSets[difficulty].spawnWaves)
        {
            tempParent = Instantiate(new GameObject(), transform);
            tempParent.name = $"----WAVE {i}----";
            spawnWave.InitialiseSpawn(tempParent.transform, this);
            i++;
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
        return spawnersLeft == 0;
    }

    internal void IncrementEnemies()
    {
        IncrementEnemies(1);
    }

    internal void IncrementEnemies(int i)
    {
        enemyCountTotal += i;
        enemyCountCurrent += i;
    }

    internal void DecrementEnemies()
    {
        enemyCountTotal--;
        enemyCountCurrent--;
        UpdateEnemyNumberDisplay();
    }
}