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
        return spawnersLeft == 0;
    }
}
