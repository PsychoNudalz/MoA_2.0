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

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (gunManager == null)
        {
            gunManager = FindObjectOfType<GunManager>();
        }
        GenerateRewardLoot();

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
            gunCache[i].GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(30 * i, Vector3.up) * Quaternion.AngleAxis(30, Vector3.right) * (new Vector3(0, 3000f, 0)));

        }
    }

    private void GenerateRewardLoot()
    {
        List<GameObject> gunList = gunManager.GenerateGun(lootAmount, spawnLevel - 1, spawnLevel + 1);



        GameObject newGun;
        for (int i = 0; i < gunList.Count; i++)
        {
            newGun = gunList[i];
            if (gunSpawnTransform != null)
            {
                newGun.transform.position = gunSpawnTransform.position + new Vector3(i * 0.3f - (lootAmount / 2f), 1, i * 0.3f - (lootAmount / 2f));

            }
            else
            {
                newGun.transform.position = player.transform.position + new Vector3(i * 0.3f - (lootAmount / 2f), 1, i * 0.3f - (lootAmount / 2f));
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
        gunManager.ClearGunsOnGround(false);
        player.GetComponent<PlayerMasterScript>().PlayerLifeSystemScript.healHealth_Percentage(.2f);
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
