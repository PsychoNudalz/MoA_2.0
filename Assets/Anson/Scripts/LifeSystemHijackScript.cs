using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifeSystemHijackScript : MonoBehaviour
{
    [SerializeField] LifeSystemScript ls;
    [SerializeField] TextMeshProUGUI dmgText;
    [SerializeField] TextMeshProUGUI dpsText;
    [SerializeField] TextMeshProUGUI maxdpsText;
    [SerializeField] float updateRate;
    [SerializeField] List<float> allHealth;
    [SerializeField] float logTime;
    [SerializeField] float dpsTrackRange = 15f;
    [SerializeField] float dps;
    [SerializeField] float maxDPS;


    private void FixedUpdate()
    {
        if (Time.time - logTime > updateRate)
        {

            AddHealthToQueue(ls.Health_Current);
        }
        CalculateDPS();
        UpdateDMGText();
        UpdateDPSText();
        UpdateMaxDPSText();
    }

    void AddHealthToQueue(float hp)
    {
        if (allHealth.Count >= dpsTrackRange * (1 / updateRate) && allHealth.Count>0)
        {
            allHealth.RemoveAt(0);
        }
        allHealth.Add(hp);
        logTime = Time.time;
    }

    private void UpdateDMGText()
    {
        dmgText.text = "Total damage: " + (ls.Health_Max - ls.Health_Current);
    }

    private void UpdateDPSText()
    {
        dpsText.text = "DPS: " + dps;
    }
    private void UpdateMaxDPSText()
    {
        maxdpsText.text = "Max DPS: " + maxDPS;
    }

    float CalculateDPS()
    {

        if (allHealth.Count < 2)
        {
            dps = 0;
        }
        else
        {
        dps = (allHealth[0] - allHealth[allHealth.Count - 1]) / dpsTrackRange;
        }
        if (dps > maxDPS)
        {
            maxDPS = dps;
        }
        return dps;
    }
}
