using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image pressAnykey;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject continueMenu;
    [SerializeField] private GameObject headTitle;
    [SerializeField] private GameObject darken;

    [SerializeField] private float factor = -0.004f;
    [SerializeField] private AudioSource audioSource;
    private Keyboard keyboard;
    private Mouse mouse;
    void Start() {
        if (pressAnykey == null) pressAnykey = gameObject.transform.GetChild(4).GetChild(0).gameObject.GetComponent<Image>();
        if (mainMenu == null) mainMenu = gameObject.transform.GetChild(5).gameObject;
        if (settingsMenu == null) settingsMenu = gameObject.transform.GetChild(7).gameObject;
        if (continueMenu == null) continueMenu = gameObject.transform.GetChild(6).gameObject;
        if (headTitle == null) headTitle = gameObject.transform.GetChild(3).gameObject;
        if (darken == null) darken = gameObject.transform.GetChild(2).gameObject;
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
            audioSource.Play();
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
        FindObjectOfType<SaveManagerScript>().SetSaveProfile(slotIndex - 1);
        SceneManager.LoadScene("Tutorial");
    }
}
