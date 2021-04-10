using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public GunGeneratorScript generatorScript1;
    public GunGeneratorScript generatorScript2;
    public GunGeneratorScript generatorScript3;
    public ShootingRangeScript shootingRangeScript1;
    public ShootingRangeScript shootingRangeScript2;

    public PlayerInventorySystemScript playerInventorySystemScript;
    //public GunDamageScript gunDamageScript;
    Mouse mouse;
    Keyboard keyboard;

    public bool autoGenWeaponsOnAwake = true;

    private void Awake()
    {
        Application.targetFrameRate = 200;

        if (generatorScript1 == null)
        {
            generatorScript1 = FindObjectOfType<GunGeneratorScript>();
        }
        /*
        if (gunDamageScript == null)
        {
            gunDamageScript = FindObjectOfType<GunDamageScript>();
        }
        */
        playerInventorySystemScript = FindObjectOfType<PlayerInventorySystemScript>();


    }

    private void Start()
    {
        if (autoGenWeaponsOnAwake)
        {
            InitializePlayerInventory();
        }
    }

    private void FixedUpdate()
    {
        if (!playerInventorySystemScript)
        {
            playerInventorySystemScript = FindObjectOfType<PlayerInventorySystemScript>();
            if (playerInventorySystemScript)
            {
                if (autoGenWeaponsOnAwake)
                {
                    InitializePlayerInventory();
                }
            }

        }
    }


    public void GenerateGun_Single(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {

            GameObject newGun = generatorScript1.GenerateGun();
            /*
            if (gunDamageScript == null)
            {
                gunDamageScript = FindObjectOfType<GunDamageScript>();
            }
            gunDamageScript.UpdateGunScript(newGun.GetComponent<MainGunStatsScript>());
            */
            if (playerInventorySystemScript == null)
            {
                playerInventorySystemScript = FindObjectOfType<PlayerInventorySystemScript>();
            }
            playerInventorySystemScript.PickUpNewGun(newGun.GetComponent<MainGunStatsScript>());

        }
    }
    public void GenerateGun_Group(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {

            GameObject newGun = generatorScript2.GenerateGun();
            if (playerInventorySystemScript == null)
            {
                playerInventorySystemScript = FindObjectOfType<PlayerInventorySystemScript>();
            }
            playerInventorySystemScript.PickUpNewGun(newGun.GetComponent<MainGunStatsScript>());
        }
    }
    public void GenerateGun_All(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {

            GameObject newGun = generatorScript3.GenerateGun();
            if (playerInventorySystemScript == null)
            {
                playerInventorySystemScript = FindObjectOfType<PlayerInventorySystemScript>();
            }
            playerInventorySystemScript.PickUpNewGun(newGun.GetComponent<MainGunStatsScript>());
        }
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

    public void InitializePlayerInventory()
    {
        if (playerInventorySystemScript == null)
        {
            playerInventorySystemScript = FindObjectOfType<PlayerInventorySystemScript>();
        }
        for (int i = 0; i < 3; i++)
        {
            GameObject newGun = generatorScript3.GenerateGun();
            
            playerInventorySystemScript.SwapToWeapon(i);
            playerInventorySystemScript.SwapWeapon(newGun.GetComponent<MainGunStatsScript>(), true);
        }

        playerInventorySystemScript.SwapToWeapon(0);
    }
}
