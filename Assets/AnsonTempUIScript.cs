using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    //public TextMeshProUGUI healthText;
    public Image enemyHealthBar;
    public Text enemyName;
    public GameObject enemyInfo;
    public Image healthBar;
    public Image dash;
    public Text dashChargeDisplay;
    Sprite dashReady, dashCoolDown;
    public GameObject gameOverScreen;
    public WeaponAmmoPair gun1;
    public WeaponAmmoPair gun2;
    public WeaponAmmoPair gun3;

    [Header("Debug")]
    [SerializeField] bool debugMode;
    [SerializeField] TextMeshProUGUI coinText;
    
    private void Start() {
        dashReady = Resources.Load<Sprite>("Sprites/Skill_Ready");
        dashCoolDown = Resources.Load<Sprite>("Sprites/Skill_CoolDown");
    }

    private void Awake()
    {
        if (debugMode)
        {
            coinText.gameObject.SetActive(true);
        }
        else
        {
            coinText.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (debugMode)
        {
            SetCoins(FindObjectOfType<PlayerSaveStats>().coins);
        }
    }

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
        // TODO: Elements and types
    }

    public void SetHealth(float hp, float hp_Max)
    {
        float hp_ratio = hp / hp_Max;
        RectTransform rt = healthBar.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            478 * hp_ratio);
        if (hp_ratio < 0.3f) {
            healthBar.color = new Color(0.81f, 0.164f, 0.307f, 1);
        } else {
            healthBar.color = new Color(1, 1, 1, 1);
        }
        //healthText.text = "HP:" + hp.ToString("0") + "/" + hp_Max.ToString("0");
    }

    public void SetEnemyHealth(float hp, float hp_Max, string name) {
        enemyInfo.SetActive(true);
        float hp_ratio = hp / hp_Max;
        enemyName.text = name;
        RectTransform rt = enemyHealthBar.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            595 * hp_ratio);
        if (hp_ratio < 0.3f) {
            enemyHealthBar.color = new Color(0.81f, 0.164f, 0.307f, 1);
        } else {
            enemyHealthBar.color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateDashDisplay(int charges) {
        if (charges == 0) {
            dash.sprite = dashCoolDown;
            dashChargeDisplay.text = "0 / 2";
        } else {
            dash.sprite = dashReady;
            dashChargeDisplay.text = charges.ToString("0") + " / 2";
        }
        // TODO: add a progress bar maybe?
    }

    public void SetCoins(int amount)
    {
        coinText.text = "Coins:" + amount;
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
