using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal portalTarget;

    [SerializeField]
    Transform targetSpawner;
    GameObject player;


    [SerializeField]
    RoomEnemySystem currentRoomEnemySystem;
    [SerializeField]
    RoomEnemySystem nextRoomEnemySystem;
    [SerializeField] GunManager gunManager;
    [SerializeField] int lootAmount = 6;
    [SerializeField] int CoinAmount = 2;
    bool rewardLoot;
    [Header("Debug")]
    [SerializeField] bool ignoreSpawner = false;


    public RoomEnemySystem CurrentRoomEnemySystem { get => currentRoomEnemySystem; }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (gunManager == null)
        {
            gunManager = FindObjectOfType<GunManager>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((ignoreSpawner || currentRoomEnemySystem.IsRoomClear()) && !rewardLoot)
        {
            rewardLoot = true;
            player.GetComponent<PlayerMasterScript>().PlayerSaveStats.AddCoins(CoinAmount);
            for (int i = 0; i < lootAmount; i++)
            {
                gunManager.GenerateGun().transform.position = player.transform.position + new Vector3(i*0.1f, 1, 0);
            }

        }
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
