using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempEnemyDisplay : MonoBehaviour
{
    private Text enemiesText;

    private void Start()
    {
        enemiesText = GetComponentInChildren<Text>();
    }

    public void SetText(string s)
    {
        enemiesText.text = s;
    }
}
