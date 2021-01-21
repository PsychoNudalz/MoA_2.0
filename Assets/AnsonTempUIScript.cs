using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnsonTempUIScript : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI gunText;


    public void SetAmmoText(string s)
    {
        ammoText.text = "Ammo:" + s;
    }

    public void SetGunText(string s)
    {
        gunText.text = s;
    }

}
