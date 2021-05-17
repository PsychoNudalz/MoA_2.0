using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplayScript : MonoBehaviour
{
    private PlayerSaveStats playerStats;
    private TMP_Text statsText;

    private void Awake()
    {
        playerStats = GameObject.FindObjectOfType<PlayerSaveStats>();
        statsText = GetComponentInChildren<TMP_Text>();
        if(playerStats != null)
        {
            statsText.text = string.Format("<b>PROFILE {0}</b>\n" +
                                      "Coins Collected = {1}\n" +
                                      "Number of Runs = {2}\n" +
                                      "Boss Levels Cleared = {3}\n" +
                                      "Full Completed Runs = {4}",
                                      playerStats.profile,
                                      playerStats.coins,
                                      playerStats.numberOfRuns,
                                      playerStats.numberOfBossKills,
                                      playerStats.totalFullClears);
        }
    }

}
