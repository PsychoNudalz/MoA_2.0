using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSaveProfile
{
    A,
    B,
    C,
    DEFAULT,
    TESTING
}

public class PlayerSaveStats : MonoBehaviour
{
    public PlayerSaveProfile profile = PlayerSaveProfile.A;
    public int coins = 0;
    public int numberOfRuns = 0;
    public int numberOfBossKills = 0;
    public int totalFullClears = 0;

    public void Load(PlayerSaveStats pss)
    {
        profile = pss.profile;
        this.coins = pss.coins;
        this.numberOfRuns = pss.numberOfRuns;
        this.numberOfBossKills = pss.numberOfBossKills;
        this.totalFullClears = pss.totalFullClears;
    }
    public void Load(PlayerSaveCollection psc)
    {
        profile = (PlayerSaveProfile) psc.profile;
        this.coins = psc.coins;
        this.numberOfRuns = psc.numberOfRuns;
        this.numberOfBossKills = psc.numberOfBossKills;
        this.totalFullClears = psc.totalFullClears;
    }




    public int AddCoins(int amount)
    {
        coins += amount;
        return coins;
    }


}
