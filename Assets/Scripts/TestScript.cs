using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GunGeneratorScript generatorScript;
    public GunDamageScript gunDamageScript;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject newGun = generatorScript.GenerateGun();
            gunDamageScript.UpdateGunScript(newGun.GetComponent<MainGunStatsScript>());
        }
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0))
        {
            gunDamageScript.isFiring = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            gunDamageScript.isFiring = false;

        }

        if (Input.GetMouseButtonDown(1))
        {
            gunDamageScript.ADS_On();
        }
        if (Input.GetMouseButtonUp(1))
        {
            gunDamageScript.ADS_Off();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            gunDamageScript.Reload();
        }
    }


}
