﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RoomEnemySystem : MonoBehaviour
{
    
    Keyboard kb;
    EnemySpawner[] roomSpawners;
    private int enemyCount = 0;
    AnsonTempUIScript UIScript;

    private void Awake()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        roomSpawners = GetComponentsInChildren<EnemySpawner>();
        UIScript = FindObjectOfType<AnsonTempUIScript>();
    }

    private void UpdateEnemyNumberDisplay(bool start = false)
    {
        
        if (start)
        {
            UIScript.SetEnemiesRemainingText(enemyCount,false);
            UIScript.SetPortalIconActive(false);
        }
        else if(enemyCount == 0)
        {
            UIScript.SetEnemiesRemainingText(enemyCount, true);
        }
        else
        {
            UIScript.SetEnemiesRemainingText(enemyCount, false);
        }
        

    }

    private void Update()
    {
        
        //For testing start spawning
        /*
        if (kb.numpadMinusKey.wasPressedThisFrame)
        {
            StartRoomSpawners();
        }
        
        */
    }

    public void StartRoomSpawners()
    {
        if(roomSpawners.Length > 0)
        {
            foreach(EnemySpawner spawner in roomSpawners)
            {
                spawner.StartSpawning();
                UpdateEnemyNumberDisplay(true);
            }
        }
        else
        {
            Debug.LogWarning("No spawners found");
        }
    }

    public bool IsRoomClear()
    {
        int spawnersLeft = GetComponentsInChildren<EnemySpawner>().Length;
        return spawnersLeft == 0;
    }

    internal void IncrementEnemies()
    {
        enemyCount++;
    }

    internal void DecrementEnemies()
    {
            enemyCount--;
            UpdateEnemyNumberDisplay();
    }
}
