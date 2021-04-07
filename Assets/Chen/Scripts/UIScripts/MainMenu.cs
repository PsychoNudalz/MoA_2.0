using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Image pressAnykey;
    private GameObject mainMenu;
    private GameObject settingsMenu;
    private GameObject continueMenu;
    private GameObject headTitle;
    private GameObject darken;

    [SerializeField] private float factor = -0.004f;
    private Keyboard keyboard;
    private Mouse mouse;
    void Start() {
        pressAnykey = gameObject.transform.GetChild(4).GetChild(0).gameObject.GetComponent<Image>();
        mainMenu = gameObject.transform.GetChild(5).gameObject;
        settingsMenu = gameObject.transform.GetChild(7).gameObject;
        continueMenu = gameObject.transform.GetChild(6).gameObject;
        headTitle = gameObject.transform.GetChild(3).gameObject;
        darken = gameObject.transform.GetChild(2).gameObject;
        keyboard = Keyboard.current;
        mouse = Mouse.current;
    }

    void Update() {
        pressAnykey.color = new Color(pressAnykey.color.r,
                                      pressAnykey.color.g,
                                      pressAnykey.color.b,
                                      pressAnykey.color.a + factor
                                      );
        if (pressAnykey.color.a < 0.05f) factor = 0.004f;
        if (pressAnykey.color.a > 0.95f) factor = -0.004f;

        if (pressAnykey.gameObject.activeSelf && (keyboard.anyKey.isPressed || mouse.leftButton.isPressed || mouse.rightButton.isPressed || mouse.middleButton.isPressed)) {
            pressAnykey.gameObject.SetActive(false);
            darken.SetActive(true);
            mainMenu.SetActive(true);
        }
    }

    public void ContinueOnClick() {
        headTitle.SetActive(false);
        settingsMenu.SetActive(false);
        continueMenu.SetActive(true);
    }

    public void NewGameOnClick() {
        SceneManager.LoadScene("Tutorial");
    }

    public void SettingsOnClick() {
        headTitle.SetActive(false);
        continueMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ExitOnClick() {
        Application.Quit();
    }

    public void slotsOnClick(int slotIndex) {
        // no SL function currently, will jump in new game instead
        SceneManager.LoadScene("Tutorial");
    }
}
