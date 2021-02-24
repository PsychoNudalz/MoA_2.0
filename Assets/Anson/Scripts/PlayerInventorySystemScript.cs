using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventorySystemScript : MonoBehaviour
{
    [SerializeField] PlayerMasterScript playerMasterScript;
    [Space]
    [SerializeField] MainGunStatsScript[] Weapons = new MainGunStatsScript[3];
    [SerializeField] int pointer = 0;
    [SerializeField] Transform inventoryTransform;
    [SerializeField] GunDamageScript gunDamageScript;

    public GunDamageScript GunDamageScript { get => gunDamageScript; set => gunDamageScript = value; }
    public int Pointer { get => pointer; set => pointer = value; }

    //[SerializeField] MainGunStatsScript currentGun;


    public void SwapWeapon(MainGunStatsScript newGun, bool isNew = false)
    {
        MainGunStatsScript currentGun = Weapons[pointer];
        gunDamageScript.TidyOldGun();

        if (isNew)
        {
            throwOldWeapon();
            Weapons[pointer] = newGun;
        }
        else
        {
            if (currentGun != null)
            {

                //currentGun.gameObject.transform.SetParent(inventoryTransform);
                currentGun.gameObject.SetActive(false);
            }
        }
        if (newGun != null)
        {
            gunDamageScript.UpdateGunScript(newGun);

        }

    }

    public void SwapToWeapon(int i)
    {
        if (i < Weapons.Length && i > -1)
        {

            SwapWeapon(Weapons[i]);
            pointer = i;
        }
        else
        {
            Debug.LogError("Weapon Swap index Error");
        }
    }


    void throwOldWeapon()
    {
        gunDamageScript.TidyOldGun();
        MainGunStatsScript currentGun = Weapons[pointer];

        if (currentGun != null)
        {
            currentGun.transform.position += transform.right;
            currentGun.GetComponentInChildren<Rigidbody>().isKinematic = false;
            currentGun.GetComponentInChildren<Rigidbody>().AddForce(transform.up * 1000f);
            currentGun.gameObject.transform.parent = null;
        }
    }



}
