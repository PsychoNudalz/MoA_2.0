using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{    
    private Keyboard keyboard;
    [SerializeField] private GameObject menuBody;
    [SerializeField] private GameObject popUpGroup;
    [SerializeField] private GameObject popUpFocus;
    [SerializeField] private GameObject menuPrimary;
    [SerializeField] private GameObject primaryFocus;
    [SerializeField] private GameObject menuSettings;
    [SerializeField] private GameObject helpPage;
    bool m_paused = false;
    bool m_popUp = false;
    bool m_settings = false;
    private EventSystem eventSystem;
    void Start()
    {
        eventSystem = EventSystem.current;
        keyboard = Keyboard.current;
        if (menuBody == null) menuBody = gameObject.transform.GetChild(0).gameObject;
        if (popUpGroup == null) popUpGroup = gameObject.transform.GetChild(1).gameObject;
        if (menuPrimary == null) menuPrimary = menuBody.transform.GetChild(1).gameObject;
        if (menuSettings == null) menuSettings = menuBody.transform.GetChild(2).gameObject;
    }

    void Update()
    {

    }

    public void HelpButtonOnClick() {
        helpPage.SetActive(true);
    }

    public void HelpCloseOnClick() {
        helpPage.SetActive(false);
    }
    public void TogglePauseMenu() {
        if (m_popUp) {
            popUpGroup.SetActive(false);
            m_popUp = false;
        } else if (m_settings) {
            menuSettings.SetActive(false);
            menuPrimary.SetActive(true);
            m_settings = false;
        } else if (m_paused) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1.0f;
            // AudioListener.pause = false;
            menuBody.SetActive(false);
            m_paused = !m_paused;
        } else {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0.0f;
           // AudioListener.pause = true;
            menuBody.SetActive(true);
            m_paused = !m_paused;
        }
        //FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
    }

    public void ContinueOnClick() {
        TogglePauseMenu();
    }

    public void SettingsOnClick() {
        menuPrimary.SetActive(false);
        menuSettings.SetActive(true);
        m_settings = true;
    }

    public void ExitOnClick() {
        popUpGroup.SetActive(true);
        m_popUp = true;
    }

    public void BackToBaseOnClick() {
        FindObjectOfType<SaveManagerScript>().SaveProcedure();
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader != null)
        {
            loader.LoadWithLoadingScreen("Base");
        }
        else
        {
            Debug.LogWarning("SceneLoader not found");
            SceneManager.LoadScene("Base");
        }
    }

    public void PopUpBackOnClick() {
        popUpGroup.SetActive(false);
        m_popUp = false;
    }

    public void PopUpExitOnClick() {
        FindObjectOfType<SaveManagerScript>().SaveProcedure();
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader != null)
        {
            loader.LoadWithLoadingScreen("MainEntry");
        }
        else
        {
            Debug.LogWarning("SceneLoader not found");
            SceneManager.LoadScene("MainEntry");
        }
    }
}
