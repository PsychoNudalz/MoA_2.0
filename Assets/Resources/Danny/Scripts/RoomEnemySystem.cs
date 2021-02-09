using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomEnemySystem : MonoBehaviour
{
    
    Keyboard kb;
    EnemySpawner[] roomSpawners;

    private void Awake()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        roomSpawners = GetComponentsInChildren<EnemySpawner>();
    }

    private void Update()
    {
        
        //For testing start spawning
        if (kb.numpadMinusKey.wasPressedThisFrame)
        {
            StartRoomSpawners();
        }
        if (kb.numpadPlusKey.wasPressedThisFrame)
        {
            print("Room Clear = " + IsRoomClear());
        }
        
    }

    public void StartRoomSpawners()
    {
        if(roomSpawners.Length > 0)
        {
            foreach(EnemySpawner spawner in roomSpawners)
            {
                spawner.StartSpawning();
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
        if(spawnersLeft > 0)
        {
            print(spawnersLeft + " Spawners left");
        }
        return spawnersLeft == 0;
    }
}
