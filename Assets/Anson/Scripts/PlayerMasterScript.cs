using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using UnityEngine;

public class PlayerMasterScript : MonoBehaviour
{
    public static PlayerMasterScript current;
    
    [SerializeField]
    PlayerLifeSystemScript playerLifeSystemScript;

    [SerializeField]
    PlayerGunDamageScript playerGunDamageScript;

    [SerializeField]
    PlayerInventorySystemScript playerInventorySystemScript;

    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    PlayerInterationScript playerInterationScript;

    [SerializeField]
    PlayerUIScript playerUIScript;

    [SerializeField]
    PlayerVolumeControllerScript playerVolumeControllerScript;

    [SerializeField]
    PlayerSaveStats playerSaveStats;

    [SerializeField]
    PlayerGetTargetHealthScript playerGetTargetHealth;

    [SerializeField]
    private PlayerMelee playerMelee;

    [SerializeField]
    PlayerSaveCollection playerSaveCollection;

    [SerializeField]
    PlayerSoundScript playerSoundScript;

    [SerializeField]
    UnityEngine.InputSystem.PlayerInput playerInput;
    
    
    [Header("Cheats")]
    public static bool INFINITEAMMO = false;

    public PlayerUIScript PlayerUIScript
    {
        get => playerUIScript;
        set => playerUIScript = value;
    }

    public PlayerSaveStats PlayerSaveStats
    {
        get => playerSaveStats;
    }

    public PlayerSaveCollection PlayerSaveCollection
    {
        set => playerSaveCollection = value;
    }

    public PlayerLifeSystemScript PlayerLifeSystemScript
    {
        get => playerLifeSystemScript;
        set => playerLifeSystemScript = value;
    }

    private void Awake()
    {
        if (playerSaveCollection == null)
        {
            Initialize();
        }
    }

    [ContextMenu("Initialize")]
    void Initialize()
    {

        if (current)
        {
            Destroy(current.gameObject);
            current = null;
        }

        current = this;
        
        //Set up
        print("Initialise player master");
        if (playerLifeSystemScript == null)
        {
            playerLifeSystemScript = GetComponent<PlayerLifeSystemScript>();
        }

        if (playerGunDamageScript == null)
        {
            playerGunDamageScript = GetComponentInChildren<PlayerGunDamageScript>();
        }

        if (playerInventorySystemScript == null)
        {
            playerInventorySystemScript = GetComponent<PlayerInventorySystemScript>();
        }

        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }

        if (playerUIScript == null)
        {
            playerUIScript = GetComponentInChildren<PlayerUIScript>();
        }

        if (playerVolumeControllerScript == null)
        {
            playerVolumeControllerScript = GetComponentInChildren<PlayerVolumeControllerScript>();
        }

        if (!playerInterationScript)
        {
            playerInterationScript = GetComponent<PlayerInterationScript>();
        }

        if (!playerSaveStats)
        {
            playerSaveStats = GetComponent<PlayerSaveStats>();
        }

        if (!playerGetTargetHealth)
        {
            playerGetTargetHealth = GetComponent<PlayerGetTargetHealthScript>();
        }

        if (!playerSoundScript)
        {
            playerSoundScript = GetComponentInChildren<PlayerSoundScript>();
        }

        if (!playerMelee)
        {
            playerMelee = GetComponentInChildren<PlayerMelee>();
        }


        if (playerController.GunDamageScript == null)
        {
            playerController.GunDamageScript = playerGunDamageScript;
        }

        if (playerController.PlayerInventorySystemScript == null)
        {
            playerController.PlayerInventorySystemScript = playerInventorySystemScript;
        }

        if (!playerController.PlayerInterationScript)
        {
            playerController.PlayerInterationScript = playerInterationScript;
        }

        if (!playerController.PlayerUIScript)
        {
            playerController.PlayerUIScript = playerUIScript;
        }

        if (playerInventorySystemScript.GunDamageScript == null)
        {
            playerInventorySystemScript.GunDamageScript = playerGunDamageScript;
        }

        if (!playerLifeSystemScript.PlayerSoundScript)
        {
            PlayerLifeSystemScript.PlayerSoundScript = playerSoundScript;
        }

        if (!playerController.PlayerSoundScript)
        {
            playerController.PlayerSoundScript = playerSoundScript;
        }

        if (!playerController.PlayerMelee)
        {
            playerController.PlayerMelee = playerMelee;
        }

        playerLifeSystemScript.PlayerMasterScript = this;
        playerLifeSystemScript.UIScript1 = playerUIScript;
        playerInventorySystemScript.playerUIScript = playerUIScript;
        playerLifeSystemScript.PlayerVolumeControllerScript = playerVolumeControllerScript;
        playerController.PlayerVolumeControllerScript = playerVolumeControllerScript;
        
        
    }

    public void SetControls(bool b)
    {
        playerController.SetControlLock(b);
    }

    public void GameOver()
    {
        SetControls(false);
        playerUIScript.ShowGameOver();
    }

    /// <summary>
    /// to change mouse sensitivity
    /// </summary>
    /// <param name="amount"> new sensitivity amount</param>
    public void SetSensitivity(float amount)
    {
        playerController.SetSensitivity(amount);
    }

    public void SetADSMultiplier(float amount)
    {
        playerController.SetADSSensitivity(amount);
    }


    public int AddCoins(int amount)
    {
        int temp = playerSaveStats.AddCoins(amount);
        if (playerUIScript != null)
        {
            playerUIScript.SetCoins(temp);
        }

        return temp;
    }

    /// <summary>
    /// remove coins from player, returns true player has sufficent Coins
    /// </summary>
    /// <param name="amount">amoutn of coins</param>
    /// <returns>true player has sufficent Coins</returns>
    public bool RemoveCoins(int amount)
    {
        if (playerSaveStats.coins < amount)
        {
            return false;
        }
        else
        {
            int temp = playerSaveStats.AddCoins(-amount);
            if (playerUIScript != null)
            {
                playerUIScript.SetCoins(temp);
            }

            return true;
        }
    }

    public void LoadSave(PlayerSaveCollection psc)
    {
        Initialize();
        playerSaveCollection = psc;
        playerSaveStats.Load(psc);
    }

    public void TeleportPlayer(Vector3 pos)
    {
        playerController.Teleport(pos);
        playerUIScript.DisplayNewGunText(false);
    }

    public void IncreamentRun()
    {
        playerSaveStats.numberOfRuns++;
    }

    public void IncreamentBossKill()
    {
        playerSaveStats.numberOfBossKills++;
    }

    public void IncreamentClears()
    {
        playerSaveStats.totalFullClears++;
    }
    
    
    
    
    //Quantum console
    [Command("Player_IA")]
    public static void Player_InfiniteAmmo(bool b)
    {
        INFINITEAMMO = b;
    }
    
    [Command("Player_IA")]
    public static void Player_InfiniteAmmo()
    {
        INFINITEAMMO = !INFINITEAMMO;
    }
}