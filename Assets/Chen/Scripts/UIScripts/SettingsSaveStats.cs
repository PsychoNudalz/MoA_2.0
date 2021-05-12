using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSaveStats : MonoBehaviour
{
    
    public float sensitivity = 15;
    public float sensitivityADS = 1;
    public float masterVolume = 1;

    public void Load(SettingsSaveStats sss)
    {
        this.sensitivity = sss.sensitivity;
        this.sensitivityADS = sss.sensitivityADS;
        this.masterVolume = sss.masterVolume;
    }
    public void Load(SettingsSaveCollection ssc)
    {
        this.sensitivity = ssc.sensitivity;
        this.sensitivityADS = ssc.sensitivityADS;
        this.masterVolume = ssc.masterVolume;
    }
}
