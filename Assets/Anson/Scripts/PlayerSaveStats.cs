using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveStats : MonoBehaviour
{
    public int coins = 0;
    public int numberOfRuns = 0;
    public int numberOfBossKills = 0;

    public int AddCoins(int amount)
    {
        coins += amount;
        return coins;
    }

}
