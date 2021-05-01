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
    [SerializeField] PlayerGunDamageScript gunDamageScript;
    public AnsonTempUIScript ansonTempUIScript;

    public PlayerGunDamageScript GunDamageScript { get => gunDamageScript; set => gunDamageScript = value; }
    public int Pointer { get => pointer; set => pointer = value; }

    //[SerializeField] MainGunStatsScript currentGun;


    public void SwapWeapon(MainGunStatsScript newGun, bool isReplace = false, int i = -1)
    {
        MainGunStatsScript currentGun = Weapons[pointer];
        gunDamageScript.TidyOldGun();

        if (isReplace)
        {
            /*
            int s = GetNextFreeInventorySlot();
            if ((s >= 0) && (i == -1))
            {
                i = s;
                gunDamageScript.TidyOldGun();
                stowCurrentGun(currentGun);

            }
            else
            {

                throwOldWeapon();
            }
            */
            ThrowOldWeapon();

            Weapons[pointer] = newGun;
        }else if (i != -1)
        {
            StowCurrentGun(currentGun);

            pointer = i;
            Weapons[pointer] = newGun;

        }
        else
        {
            StowCurrentGun(currentGun);
        }

        if (i == -1)
        {
            i = pointer;
        }
        //print("Update Gun to: " + newGun.GetName());
        newGun.SetRarityEffect(false);
        gunDamageScript.UpdateGunScript(newGun, i);
        gunDamageScript.UpdateUI();
        ansonTempUIScript.UpdateActiveGun(pointer);
    }

    public void PickUpNewGun(MainGunStatsScript newGun)
    {
        if (GetNextFreeInventorySlot() >= 0)
        {
            print("picking up new gun to slot: " + GetNextFreeInventorySlot());
            SwapWeapon(newGun, false, GetNextFreeInventorySlot());
        }
        else
        {
            SwapWeapon(newGun, true);
        }
    }
    public int GetNextFreeInventorySlot()
    {
        for (int i = 0; i < 3; i++)
        {
            if (Weapons[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public void SwapToWeapon(int i, bool force = false)
    {
        if (i < Weapons.Length && i > -1)
        {
            if (force || Weapons[i] != null)
            {

                SwapWeapon(Weapons[i], false, i);
                pointer = i;
            }
        }
        else
        {
            Debug.LogError("Weapon Swap index Error");
        }
    }

    public void CycleWeapon(bool next)
    {
        if (next)
        {
            SwapToWeapon((pointer + 1) % 3);
        }
        else
        {
            SwapToWeapon((pointer +2) % 3);

        }
    }


    void ThrowOldWeapon()
    {
        gunDamageScript.TidyOldGun();
        MainGunStatsScript currentGun = Weapons[pointer];

        if (currentGun != null)
        {
            currentGun.transform.position += transform.forward;
            currentGun.GetComponentInChildren<Rigidbody>().isKinematic = false;
            currentGun.GetComponentInChildren<Rigidbody>().AddForce(transform.up * 1000f);
            currentGun.gameObject.transform.parent = null;
            currentGun.SetRarityEffect(true);
        }
    }
    void StowCurrentGun(MainGunStatsScript currentGun)
    {
        if (currentGun != null)
        {

            currentGun.gameObject.transform.SetParent(inventoryTransform);
            currentGun.gameObject.transform.localPosition = new Vector3();
            currentGun.gameObject.transform.localRotation = Quaternion.identity;
            //currentGun.gameObject.SetActive(false);
        }
    }

}
