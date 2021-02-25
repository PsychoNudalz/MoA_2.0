using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMasterScript : MonoBehaviour
{
    [SerializeField] PlayerLifeSystemScript playerLifeSystemScript;
    [SerializeField] GunDamageScript playerGunDamageScript;
    [SerializeField] PlayerInventorySystemScript playerInventorySystemScript;
    [SerializeField] PlayerController playerController;

    private void Awake()
    {
        //Set up
        if (playerLifeSystemScript == null)
        {
            playerLifeSystemScript = GetComponent<PlayerLifeSystemScript>();
        }
        if (playerGunDamageScript == null)
        {
            playerGunDamageScript = GetComponentInChildren<GunDamageScript>();
        }
        if (playerInventorySystemScript == null)
        {
            playerInventorySystemScript = GetComponent<PlayerInventorySystemScript>();
        }
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }

        if (playerController.GunDamageScript == null)
        {
            playerController.GunDamageScript = playerGunDamageScript;
        }

        if (playerController.PlayerInventorySystemScript == null)
        {
            playerController.PlayerInventorySystemScript = playerInventorySystemScript;
        }

        if (playerInventorySystemScript.GunDamageScript == null)
        {
            playerInventorySystemScript.GunDamageScript = playerGunDamageScript;
        }

    }


    
}
