﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public GunGeneratorScript generatorScript;
    public GunDamageScript gunDamageScript;
    public PlayerController playerController;
    Mouse mouse;
    Keyboard keyboard;

    private void Awake()
    {
        if (generatorScript == null)
        {
            generatorScript = FindObjectOfType<GunGeneratorScript>();
        }
        if (gunDamageScript == null)
        {
            gunDamageScript = FindObjectOfType<GunDamageScript>();
        }
        playerController = FindObjectOfType<PlayerController>();
    }


    public void GenerateGun(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {

            GameObject newGun = generatorScript.GenerateGun();
            gunDamageScript.UpdateGunScript(newGun.GetComponent<MainGunStatsScript>());
        }
    }

    public void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            gunDamageScript.Fire(true);
        }
        else if (callbackContext.canceled)
        {
            gunDamageScript.Fire(false);
        }
    }


    public void Aim(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            gunDamageScript.ADS_On();
        }
        else if (callbackContext.canceled)
        {
            gunDamageScript.ADS_Off();

        }
    }

    public void MovePlayer(InputAction.CallbackContext callbackContext)
    {
        playerController.Move(callbackContext);
    }

    public void Reload()
    {
        gunDamageScript.Reload();
    }

    public void RemoveAllGuns()
    {
        MainGunStatsScript[] AllGuns = FindObjectsOfType<MainGunStatsScript>();
        foreach(MainGunStatsScript m in AllGuns)
        {
            Destroy(m.gameObject);
        }
    }


}
