using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMasterScript : MonoBehaviour
{
    [SerializeField] PlayerLifeSystemScript playerLifeSystemScript;
    [SerializeField] PlayerGunDamageScript playerGunDamageScript;
    [SerializeField] PlayerInventorySystemScript playerInventorySystemScript;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerInterationScript playerInterationScript;
    [SerializeField] AnsonTempUIScript ansonTempUIScript;
    [SerializeField] PlayerVolumeControllerScript playerVolumeControllerScript;
    [SerializeField] PlayerSaveStats playerSaveStats;
    [SerializeField] PlayerSaveCollection playerSaveCollection;
    [SerializeField] UnityEngine.InputSystem.PlayerInput playerInput;

    public AnsonTempUIScript AnsonTempUIScript { get => ansonTempUIScript; set => ansonTempUIScript = value; }
    public PlayerSaveStats PlayerSaveStats { get => playerSaveStats;}
    public PlayerSaveCollection PlayerSaveCollection {set => playerSaveCollection = value; }

    private void Awake()
    {
        Initialize();

    }

    void Initialize()
    {
        //Set up
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
        if (ansonTempUIScript == null)
        {
            ansonTempUIScript = GetComponentInChildren<AnsonTempUIScript>();
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

        if (playerInventorySystemScript.GunDamageScript == null)
        {
            playerInventorySystemScript.GunDamageScript = playerGunDamageScript;
        }
        playerLifeSystemScript.PlayerMasterScript = this;
        playerLifeSystemScript.UIScript1 = ansonTempUIScript;
        playerLifeSystemScript.PlayerVolumeControllerScript = playerVolumeControllerScript;
        playerController.PlayerVolumeControllerScript = playerVolumeControllerScript;
        if (playerSaveCollection != null)
        {
            LoadSave(playerSaveCollection);
        }
    }

    public void SetControls(bool b)
    {
        playerController.SetControlLock(b);
    }

    public void GameOver()
    {
        SetControls(false);
        ansonTempUIScript.ShowGameOver();
    }

    /// <summary>
    /// to change mouse sensitivity
    /// </summary>
    /// <param name="amount"> new sensitivity amount</param>
    public void SetSensitivity(float amount)
    {
        playerController.SetSensitivity(amount);
    }
    public int AddCoins(int amount)
    {
        return playerSaveStats.AddCoins(amount);
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
            playerSaveStats.AddCoins(-amount);
            return true;
        }
    }

    public void LoadSave(PlayerSaveCollection psc)
    {
        playerSaveStats.Load(psc);
    }

}
