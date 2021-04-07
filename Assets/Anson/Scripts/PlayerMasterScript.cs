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
    [SerializeField] UnityEngine.InputSystem.PlayerInput playerInput;

    public AnsonTempUIScript AnsonTempUIScript { get => ansonTempUIScript; set => ansonTempUIScript = value; }

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

        if (!playerInterationScript)
        {
            playerInterationScript = GetComponent<PlayerInterationScript>();
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

    
}
