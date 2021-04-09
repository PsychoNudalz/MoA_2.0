using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveStats : MonoBehaviour
{
    public int coins = 0;
    public int numberOfRuns = 0;
    public int numberOfBossKills = 0;

    public void Load(PlayerSaveStats pss)
    {
        this.coins = pss.coins;
        this.numberOfRuns = pss.numberOfRuns;
        this.numberOfBossKills = pss.numberOfBossKills;
    }
    public void Load(PlayerSaveCollection psc)
    {
        this.coins = psc.coins;
        this.numberOfRuns = psc.numberOfRuns;
        this.numberOfBossKills = psc.numberOfBossKills;
    }




    public int AddCoins(int amount)
    {
        coins += amount;
        return coins;
    }


}
