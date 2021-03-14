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


    public void SwapWeapon(MainGunStatsScript newGun, bool isNew = false, int i = -1)
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

                currentGun.gameObject.transform.SetParent(inventoryTransform);
                currentGun.gameObject.transform.localPosition = new Vector3();
                currentGun.gameObject.transform.localRotation = Quaternion.identity;
                //currentGun.gameObject.SetActive(false);
            }
        }
        if (newGun != null)
        {
            if (i == -1)
            {
                i = pointer;
            }
            gunDamageScript.UpdateGunScript(newGun,i);

        }

    }

    public void SwapToWeapon(int i)
    {
        if (i < Weapons.Length && i > -1)
        {

            SwapWeapon(Weapons[i], false, i);
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
            currentGun.transform.position += transform.forward;
            currentGun.GetComponentInChildren<Rigidbody>().isKinematic = false;
            currentGun.GetComponentInChildren<Rigidbody>().AddForce(transform.up * 1000f);
            currentGun.gameObject.transform.parent = null;
            
        }
    }

}
