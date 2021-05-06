using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Portal : InteractableScript
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
    [SerializeField] List<GameObject> gunCache;
    [SerializeField] int lootAmount = 6;
    [SerializeField] int coinAmount = 2;
    bool rewardLoot;
    [SerializeField] Transform gunSpawnTransform;
    [SerializeField] int spawnLevel = 0;

    [Header("Debug")]
    [SerializeField] bool ignoreSpawner = false;
    [SerializeField] GameObject VFXPane;


    public RoomEnemySystem CurrentRoomEnemySystem { get => currentRoomEnemySystem; }

    void Awake()
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
            SpawnRewardLoot();

        }
        if (currentRoomEnemySystem != null)
        {
            VFXPane.SetActive(currentRoomEnemySystem.IsRoomClear());
        }
    }

    private void SpawnRewardLoot()
    {
        rewardLoot = true;
        player.GetComponent<PlayerMasterScript>().PlayerSaveStats.AddCoins(coinAmount);
        for (int i = 0; i < gunCache.Count; i++)
        {
            gunCache[i].SetActive(true);
            gunCache[i].GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(45 * i, Vector3.up) * (new Vector3(2000f, 4000f, 0)));

        }
    }

    private void GenerateRewardLoot()
    {
        print("Spawn weapon:" + spawnLevel);
        List<GameObject> gunList = gunManager.GenerateGun(lootAmount, spawnLevel - 1, spawnLevel + 1);



        GameObject newGun;
        float offset = 0.3f;
        for (int i = 0; i < gunList.Count; i++)
        {
            newGun = gunList[i];
            if (gunSpawnTransform != null)
            {
                newGun.transform.position = gunSpawnTransform.position + new Vector3(i * offset - (lootAmount / 2f) * offset, 1, i * offset - (lootAmount / 2f)) * offset;
                //newGun.transform.position = gunSpawnTransform.position;

            }
            else
            {
                newGun.transform.position = player.transform.position + new Vector3(i * offset - (lootAmount / 2f) * offset, 1, i * offset - (lootAmount / 2f) * offset);
                //newGun.transform.position = player.transform.position;
            }
            gunCache.Add(newGun);
            newGun.SetActive(false);
        }
    }

    public void Setup(RoomEnemySystem r,int level)
    {
        targetSpawner = portalTarget.transform.Find("SpawnPoint");
        nextRoomEnemySystem = r;
        spawnLevel = level;
        GenerateRewardLoot();

    }
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         if (ignoreSpawner || currentRoomEnemySystem.IsRoomClear())
    //         {
    //             TeleportPlayer();
    //         }
    //     }
    // }
    public override void activate()
    {
        base.activate();
        TeleportPlayer();
    }

    public override void deactivate()
    {
        base.deactivate();
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
        gunManager.ClearGunsOnGround(false);
        player.GetComponent<PlayerMasterScript>().PlayerLifeSystemScript.healHealth_PercentageMissing(.2f);
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
