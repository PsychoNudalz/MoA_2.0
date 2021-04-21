using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    //private float sensitivity;
    [SerializeField] Slider sensitivity;
    [SerializeField] Slider sensitivityADS;
    [SerializeField] Slider masterVolume;
    //private float sensitivityADS;
    //private float masterVolume;
    [SerializeField] private PlayerMasterScript playerMasterScript;
    [SerializeField] private bool isInGame = true;
    public SettingsSaveCollection settingsSaveCollection;
    public SettingsSaveStats settingsSaveStats;
    //[SerializeField] private AudioListener audioListener;


    private void Awake() {
        if (!settingsSaveStats) {
            settingsSaveStats = GetComponent<SettingsSaveStats>();
        }
        if (settingsSaveCollection != null) {
            LoadSave(settingsSaveCollection);
        }
    }
    public void Start() {
        if (playerMasterScript == null) isInGame = false;
        sensitivity.value = settingsSaveStats.sensitivity;
        //sensitivityADS.value = settingsSaveStats.sensitivityADS;
        masterVolume.value = settingsSaveStats.masterVolume;
        // TODO: load default/current value and reflect it on sliders
    }
    public void sensitivityOnChange(float val) {
        if (isInGame) {
            playerMasterScript.SetSensitivity(val);
        }
        settingsSaveStats.sensitivity = val;
    }

    public void sensitivityADSOnChange(float val) {
        if (isInGame) {
            playerMasterScript.SetSensitivity(val);
        }
        settingsSaveStats.sensitivityADS = val;
    }

    public void masterVolumeOnChange(float val) {
        if (isInGame) {
            AudioListener.volume = val;
        }
        settingsSaveStats.masterVolume = val;
    }

    public void LoadSave(SettingsSaveCollection sss) {
        settingsSaveStats.Load(sss);
    }
}
