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
    [Header("Gun Stats")]
    public GameObject currentGun;
    public TextMeshProUGUI currentGunText;
    public GameObject newGun;
    public TextMeshProUGUI newGunText;
    //public TextMeshProUGUI healthText;
    [Header("Enemy info")]
    public Image enemyHealthBar;
    public TextMeshProUGUI enemyName;
    public GameObject enemyInfo;
    [Header("Player info")]
    public Image healthBar;
    public Image dash;
    public Image winScr;
    public TextMeshProUGUI dashChargeDisplay;
    Sprite dashReady, dashCoolDown;
    [Header("UI Elements")]
    public GameObject gameOverScreen;
    //public GameObject crossAim;
    public Animator crossAimator;
    [Header("Inventory")]
    public WeaponAmmoPair gun1;
    public WeaponAmmoPair gun2;
    public WeaponAmmoPair gun3;
    // private WeaponAmmoPair activeGun;
    [Header("Level info")]
    private Sprite portalInactive;
    private Sprite portalActive;
    public Animator inventoryAnimator;
    // private Sprite weaponOnSlot1, weaponOnSlot2, weaponOnSlot3;
    // [SerializeField] private Image inventoryBackground;
    [SerializeField] private TextMeshProUGUI enemiesRemainingText;
    [SerializeField] private TextMeshProUGUI enemiesRemainingNumber;
    [SerializeField] private Image portalIcon;
    [Header("Pause Menu")]
    [Tooltip("0:sensitivity 1:ADS 2:Master Volume")]
    [SerializeField] List<Slider> pauseSlider;

    [Header("Debug")]
    [SerializeField] TextMeshProUGUI coinText;

    [SerializeField] Material enemiesBehindObjectMaterial;
    [SerializeField] int enemiesVisibleValue = 5;
    

    public List<Slider> PauseSlider { get => pauseSlider; set => pauseSlider = value; }

    private void Start()
    {
        dashReady = Resources.Load<Sprite>("Sprites/Skill_Ready");
        dashCoolDown = Resources.Load<Sprite>("Sprites/Skill_CoolDown");
        portalInactive = Resources.Load<Sprite>("Sprites/Portal_Inactive");
        portalActive = Resources.Load<Sprite>("Sprites/Portal_Active");
        // weaponOnSlot1 = Resources.Load<Sprite>("Sprites/Weapon Reel");
        // weaponOnSlot2 = Resources.Load<Sprite>("Sprites/Weapon Reel 2");
        // weaponOnSlot3 = Resources.Load<Sprite>("Sprites/Weapon Reel 3");
        // activeGun = gun1;
    }

    private void Awake()
    {
        coinText.gameObject.SetActive(true);
        SetCoins(FindObjectOfType<PlayerSaveStats>().coins);

    }

    public void SetEnemiesVisibleBehindObjects(bool areVisible)
    {
        if (areVisible)
        {
            enemiesBehindObjectMaterial.SetFloat("_Alpha", 0.4f);
        }
        else
        {
            enemiesBehindObjectMaterial.SetFloat("_Alpha", 1f);
        }
    }

    public void UpdateActiveGun(int gunIndex) {
        inventoryAnimator.SetInteger("GunIndex", gunIndex);
        //     activeGun.gunAmmo.fontSize = 16;
        //     activeGun.gunName.fontSize = 16;
        //     if (gunIndex == 0) {
        //         inventoryBackground.sprite = weaponOnSlot1;
        //         activeGun = gun1;
        //     }
        //     if (gunIndex == 1) {
        //         inventoryBackground.sprite = weaponOnSlot2;
        //         activeGun = gun2;
        //     }
        //     if (gunIndex == 2) {
        //         inventoryBackground.sprite = weaponOnSlot3;
        //         activeGun = gun3;
        //     }
        //     activeGun.gunAmmo.fontSize = 20;
        //     activeGun.gunName.fontSize = 20;
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
        if (hp_ratio < 0.3f)
        {
            healthBar.color = new Color(0.81f, 0.164f, 0.307f, 1);
        }
        else
        {
            healthBar.color = new Color(1, 1, 1, 1);
        }
        //healthText.text = "HP:" + hp.ToString("0") + "/" + hp_Max.ToString("0");
    }

    public void SetEnemyHealth(bool b)
    {
        enemyInfo.SetActive(false);
    }

    public void SetEnemyHealth(LifeSystemScript ls)
    {
        if (ls != null)
        {
            SetEnemyHealth(ls.Health_Current, ls.Health_Max, ls.gameObject.name);
        }
    }

    public void SetEnemyHealth(float hp, float hp_Max, string name)
    {
        enemyInfo.SetActive(true);
        float hp_ratio = hp / hp_Max;
        enemyName.text = name;
        RectTransform rt = enemyHealthBar.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            595 * hp_ratio);
        if (hp_ratio < 0.3f)
        {
            enemyHealthBar.color = new Color(0.81f, 0.164f, 0.307f, 1);
        }
        else
        {
            enemyHealthBar.color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateDashDisplay(int charges)
    {
        if (charges == 0)
        {
            dash.sprite = dashCoolDown;
            dashChargeDisplay.text = "0 / 2";
        }
        else
        {
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
        FindObjectOfType<SaveManagerScript>().SaveProcedure();
        SceneManager.LoadScene("Base");

    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetEnemiesRemainingText(int numberOfEnemies, bool roomClear)
    {
        
        SetEnemiesVisibleBehindObjects(numberOfEnemies <= enemiesVisibleValue);
        if (roomClear)
        {
            enemiesRemainingNumber.text = "";
            enemiesRemainingText.text = "Room Clear!";
            SetPortalIconActive(true);
        }
        else
        {
            enemiesRemainingNumber.text = numberOfEnemies.ToString();
            enemiesRemainingText.text = "Enemies Remaining";
            if (numberOfEnemies == 1)
            {
                enemiesRemainingText.text = "Enemy Remaining";
            }
        }
    }

    public void SetPortalIconActive(bool isActive)
    {
        if (isActive)
        {
            portalIcon.sprite = portalActive;
        }
        else
        {
            portalIcon.sprite = portalInactive;
        }
    }

    public void SetCrossair(bool b)
    {
        crossAimator.SetBool("ADS", b);
    }

    public void FireCrossair()
    {
        crossAimator.SetTrigger("Shoot");
    }
    public void CloseAllMenus()
    {
        foreach (UIPopUpInteractableScript i in FindObjectsOfType<UIPopUpInteractableScript>())
        {
            if (i is GunAlterInteractableScript)
            {
                ((GunAlterInteractableScript)i).deactivate();
            }
            else
            {
            i.deactivate();

            }
        }
    }

    public void WinScreen() {
        winScr.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
