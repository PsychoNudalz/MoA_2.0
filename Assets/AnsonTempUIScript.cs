using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct WeaponAmmoPair
{
    public TextMeshProUGUI gunName;
    public TextMeshProUGUI gunAmmo;

}

public class AnsonTempUIScript : MonoBehaviour
{
    public GameObject currentGun;
    public TextMeshProUGUI currentGunText;
    public GameObject newGun;
    public TextMeshProUGUI newGunText;
    public TextMeshProUGUI healthText;
    public GameObject gameOverScreen;
    public WeaponAmmoPair gun1;
    public WeaponAmmoPair gun2;
    public WeaponAmmoPair gun3;


    public void SetAmmoText(string s, int i)
    {
        if (i == 0)
        {
            gun1.gunAmmo.text = s;
        }
        if (i == 1)
        {
            gun2.gunAmmo.text = s;
        }
        if (i == 2)
        {
            gun3.gunAmmo.text = s;
        }
    }

    public void SetGunText(string s)
    {
        currentGunText.text = s;
    }

    public void DisplayNewGunText(bool b, string newGunS = "")
    {
        currentGun.SetActive(b);
        newGun.SetActive(b);
        if (b)
        {
            //currentGunText.text = currentGunS;
            newGunText.text = newGunS;
        }
    }

    public void SetGunName(string n, int i)
    {
        if (i == 0)
        {
            gun1.gunName.text = n;
        }
        if (i == 1)
        {
            gun2.gunName.text = n;
        }
        if (i == 2)
        {
            gun3.gunName.text = n;
        }
    }

    public void SetHealth(float hp, float hp_Max)
    {
        healthText.text = "HP:" + hp.ToString("0") + "/" + hp_Max.ToString("0");
    }

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadToBase()
    {
        SceneManager.LoadScene("Base");

    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
