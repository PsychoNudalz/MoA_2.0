using System;
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

    private void Awake()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        roomSpawners = GetComponentsInChildren<EnemySpawner>();
    }

    private void UpdateEnemyNumberDisplay(bool start = false)
    {
        string textToSet = "";
        if (start)
        {
            textToSet = "";
            // Call UI set portal icon inactive
        }
        else if(enemyCount == 0)
        {
            textToSet = "Room Clear";
            // Call UI set portal icon activated
        }
        else
        {
            textToSet = String.Format("{0} Enemies remaining", enemyCount);
        }
        //Call UI update display with textToSet


        //For testing
        FindObjectOfType<TempEnemyDisplay>().SetText(textToSet);
       
        
    }

    private void Update()
    {
        
        //For testing start spawning
        if (kb.numpadMinusKey.wasPressedThisFrame)
        {
            StartRoomSpawners();
        }
        
        
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
        UpdateEnemyNumberDisplay();
    }

    internal void DecrementEnemies()
    {
            enemyCount--;
            UpdateEnemyNumberDisplay();
    }
}
