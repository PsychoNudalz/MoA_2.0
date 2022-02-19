using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunDisplayStat : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI DPSValueText;
    [Header("Stats")]

    [SerializeField]
    private TextMeshProUGUI damageValueText;

    [SerializeField]
    private TextMeshProUGUI accValueText;

    [SerializeField]
    private TextMeshProUGUI recoilValueText;

    [SerializeField]
    private TextMeshProUGUI reloadValueText;

    [SerializeField]
    private TextMeshProUGUI fireRateValueText;

    [SerializeField]
    private TextMeshProUGUI magSizeValueText;

    [SerializeField]
    private TextMeshProUGUI rangeValueText;

    [SerializeField]
    private TextMeshProUGUI eleDmgValueText;

    [SerializeField]
    private TextMeshProUGUI eleChanceValueText;

    [SerializeField]
    private TextMeshProUGUI elePotencyValueText;

    [Header("Perks")]
    [SerializeField]
    private List<Image> perkSprites;

    [Header("Colour")]
    [SerializeField]
    private Color betterColour = Color.green;
    [SerializeField]
    private Color worseColour = Color.red;

    private MainGunStatsScript currentStats;

    public void SetStats(MainGunStatsScript mainGunStatsScript)
    {
        currentStats = mainGunStatsScript;
        
        nameText.text = mainGunStatsScript.GetName();
        DPSValueText.text = mainGunStatsScript.CalculateDps().ToString("0");
        DPSValueText.color = UniversalValues.GetColour(mainGunStatsScript.ElementType);
        
        if (mainGunStatsScript.ProjectilePerShot > 1)
        {
            damageValueText.text = mainGunStatsScript.DamagePerProjectile + " x" + mainGunStatsScript.ProjectilePerShot;
        }
        else
        {
            damageValueText.text = mainGunStatsScript.DamagePerProjectile.ToString("0");
        }

        Vector2 accuracy = mainGunStatsScript.GetAccuracy();
        accValueText.text = string.Concat((accuracy.x*100).ToString("0"), "%", "->", (accuracy.y*100).ToString("0"), "%");
        recoilValueText.text = mainGunStatsScript.Recoil.ToString();
        reloadValueText.text = mainGunStatsScript.ReloadSpeed.ToString();
        fireRateValueText.text = mainGunStatsScript.GetRPM + "/Min.";
        magSizeValueText.text = mainGunStatsScript.MagazineSize.ToString();
        rangeValueText.text = mainGunStatsScript.Range.ToString();
        eleDmgValueText.text = mainGunStatsScript.ElementDamage.ToString();
        eleChanceValueText.text = mainGunStatsScript.ElementChance.ToString();
        elePotencyValueText.text = mainGunStatsScript.ElementPotency.ToString();
        
        DisplayPerks(mainGunStatsScript.GunPerkController);
    }

    void DisplayPerks(GunPerkController gunPerkController)
    {
        Color temp;
        int j = 0;
        foreach (Perk perk in gunPerkController.Perks)
        {
            temp = perkSprites[j].color;
            temp.a = 1;
            perkSprites[j].color = temp;

            perkSprites[j].sprite = perk.PerkSprite;
            j++;
        }
        
        for (int i = j; i < perkSprites.Count; i++)
        {
            temp = perkSprites[i].color;
            temp.a = 0;
            perkSprites[i].color = temp;
        }
    }

    public void CompareStats(MainGunStatsScript current, MainGunStatsScript other)
    {
        CompareStats(DPSValueText, current.CalculateDps()>=other.CalculateDps());
        CompareStats(damageValueText, current.DamagePerProjectile*current.ProjectilePerShot>=other.DamagePerProjectile*other.ProjectilePerShot);
        CompareStats(accValueText,current.Recoil_HipFire.magnitude<= other.Recoil_HipFire.magnitude);
        CompareStats(recoilValueText,current.Recoil.magnitude<=other.Recoil.magnitude);
        CompareStats(reloadValueText, current.ReloadSpeed<= other.ReloadSpeed);
        CompareStats(fireRateValueText, current.GetRPM >= other.GetRPM);
        CompareStats(magSizeValueText, current.MagazineSize>= other.MagazineSize);
        CompareStats(rangeValueText, current.Range>= other.Range);
        CompareStats(eleDmgValueText, current.ElementDamage>=other.ElementDamage);
        CompareStats(eleChanceValueText, current.ElementChance>= other.ElementChance);
        CompareStats(elePotencyValueText, current.ElementPotency>=other.ElementPotency);

    }

    public void CompareStats(MainGunStatsScript other)
    {
        if (currentStats)
        {
            CompareStats(currentStats,other);
        }
    }

    void CompareStats(TextMeshProUGUI text, bool b)
    {
        if (b)
        {
            text.color = betterColour;
        }
        else
        {
            text.color = worseColour;
        }
    }
    
    
}