using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnsonTempUIScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void SetText(string s)
    {
        text.text = "Ammo:" + s;
    }
}
