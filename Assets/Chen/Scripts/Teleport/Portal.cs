using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal portalTarget;

    [SerializeField]
    Transform targetSpawner;

    [SerializeField]
    RoomEnemySystem currentRoomEnemySystem;
    [SerializeField]
    RoomEnemySystem nextRoomEnemySystem;
    [Header("Debug")]
    [SerializeField] bool ignoreSpawner = false;


    public RoomEnemySystem CurrentRoomEnemySystem { get => currentRoomEnemySystem; }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(RoomEnemySystem r)
    {
        targetSpawner = portalTarget.transform.Find("SpawnPoint");
        nextRoomEnemySystem = r;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (ignoreSpawner || currentRoomEnemySystem.IsRoomClear())
            {
                TeleportPlayer();
            }
        }
    }

    void TeleportPlayer()
    {
        Debug.Log("Ohhhhhhhhhhhhhhhhhhhhhhhhh");
        GameObject player = GameObject.FindWithTag("Player");
        player.SetActive(false);
        player.transform.position = targetSpawner.transform.position;
        player.SetActive(true);
        if (nextRoomEnemySystem != null)
        {

            nextRoomEnemySystem.StartRoomSpawners();
        }
        else
        {
            Debug.LogError("Cannot spawn enemy");
        }
    }
}
