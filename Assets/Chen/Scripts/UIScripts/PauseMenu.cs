using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] private Button backToBaseBtn;
    bool m_paused = false;
    bool m_popUp = false;
    bool m_settings = false;
    private EventSystem eventSystem;
    private SoundManager soundManager;
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        eventSystem = EventSystem.current;
        keyboard = Keyboard.current;
        if (menuBody == null) menuBody = gameObject.transform.GetChild(0).gameObject;
        if (popUpGroup == null) popUpGroup = gameObject.transform.GetChild(1).gameObject;
        if (menuPrimary == null) menuPrimary = menuBody.transform.GetChild(1).gameObject;
        if (menuSettings == null) menuSettings = menuBody.transform.GetChild(2).gameObject;
        if (SceneManager.GetActiveScene().name.Equals("Base")) {
            backToBaseBtn.enabled = false;
            backToBaseBtn.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        }
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
    public bool TogglePauseMenu() {
        bool isInMenu = true; 

        if (m_popUp) {
            popUpGroup.SetActive(false);
            m_popUp = false;

        } else if (m_settings) {
            menuSettings.SetActive(false);
            menuPrimary.SetActive(true);
            m_settings = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        } else if (m_paused) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1.0f;
            soundManager.ResumeSounds();
            // AudioListener.pause = false;
            menuBody.SetActive(false);
            m_paused = !m_paused;
            isInMenu = false;
        } else {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0.0f;
            soundManager.PauseAllSounds();
            // AudioListener.pause = true;
            menuBody.SetActive(true);
            m_paused = !m_paused;
            isInMenu = true;

        }
        return isInMenu;
        //FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
    }

    public void ContinueOnClick() {
        TogglePauseMenu();
        FindObjectOfType<PlayerMasterScript>().SetControls(true);
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
            Time.timeScale = 1.0f;
        }
        else
        {
            Debug.LogWarning("SceneLoader not found");
            SceneManager.LoadScene("Base");
            Time.timeScale = 1.0f;
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
