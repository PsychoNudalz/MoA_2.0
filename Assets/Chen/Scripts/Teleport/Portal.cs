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
    [SerializeField] GameObject VFXPane;


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
            GameObject newGun;
            for (int i = 0; i < lootAmount; i++)
            {
                newGun = gunManager.GenerateGun();
                newGun.transform.position = player.transform.position + new Vector3(i*0.3f, 1, i * 0.3f);
                newGun.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(30*i, Vector3.up) * Quaternion.AngleAxis(30, Vector3.right) * (new Vector3(0,1000,0)));
            }

        }
        if (currentRoomEnemySystem != null)
        {
            VFXPane.SetActive(currentRoomEnemySystem.IsRoomClear());
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
        /*
        player.SetActive(false);
        player.transform.position = targetSpawner.transform.position;
        player.SetActive(true);
        */
        player.GetComponent<PlayerMasterScript>().TeleportPlayer(targetSpawner.transform.position);
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
