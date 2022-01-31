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
    public PlayerUIScript playerUIScript;

    public PlayerGunDamageScript GunDamageScript { get => gunDamageScript; set => gunDamageScript = value; }
    public int Pointer { get => pointer; set => pointer = value; }

    //[SerializeField] MainGunStatsScript currentGun;


    public void SwapWeapon(MainGunStatsScript newGun, bool isReplace = false, int i = -1)
    {
        MainGunStatsScript currentGun = Weapons[pointer];

        //gunDamageScript.UnequipOldGun();
        if (isReplace)
        {
            /*
            int s = GetNextFreeInventorySlot();
            if ((s >= 0) && (i == -1))
            {
                i = s;
                gunDamageScript.UnequipOldGun();
                stowCurrentGun(currentGun);

            }
            else
            {

                throwOldWeapon();
            }
            */
            ResetGunToWorldLoot();
            gunDamageScript.UnequipOldGun();

            Weapons[pointer] = newGun;
        }else if (i != -1)
        {
            gunDamageScript.UnequipOldGun();
            StowCurrentGun(currentGun);

            pointer = i;
            Weapons[pointer] = newGun;
        }
        else
        {
            gunDamageScript.UnequipOldGun();
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
        playerUIScript.UpdateActiveGun(pointer);
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


    void ResetGunToWorldLoot()
    {
        gunDamageScript.ResetToWorldLoot();
        //Weapons[pointer];

    }
    void StowCurrentGun(MainGunStatsScript currentGun)
    {
        if (currentGun != null)
        {
            gunDamageScript.ADS_Off();
            GameObject o;
            (o = currentGun.gameObject).transform.SetParent(inventoryTransform);
            o.transform.localPosition = new Vector3();
            o.transform.localRotation = Quaternion.identity;
            //currentGun.gameObject.SetActive(false);
        }
    }

}
