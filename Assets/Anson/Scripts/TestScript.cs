using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public GunGeneratorScript generatorScript;
    public GunGeneratorScript generatorScript2;
    public GunGeneratorScript generatorScript3;
    public ShootingRangeScript shootingRangeScript1;
    public ShootingRangeScript shootingRangeScript2;

    public GunDamageScript gunDamageScript;
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
    }

    public void Reload()
    {
        gunDamageScript.Reload();
    }

    public void RemoveAllGuns()
    {
        MainGunStatsScript[] AllGuns = FindObjectsOfType<MainGunStatsScript>();
        foreach (MainGunStatsScript m in AllGuns)
        {
            Destroy(m.gameObject);
        }
    }

    public void StartShootingRange1(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            shootingRangeScript1.StartShootCourse();

        }
    }

    public void StartShootingRange2(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            shootingRangeScript2.StartShootCourse();

        }
    }
}
