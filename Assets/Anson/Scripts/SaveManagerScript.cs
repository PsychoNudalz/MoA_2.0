﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GCSSave
{
    public string GCName;
    public bool isSelected;
    public bool isUnloacked;
    public GCSSave(GCSelection gcs)
    {
        GCName = gcs.GetGCName();
        isSelected = gcs.IsSelected;
        isUnloacked = gcs.IsUnlocked;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is string)
        {
            //return GCName.Contains(obj as string) || ((string)obj).Contains(GCName);
            return GCName.Equals(obj as string);
        }
        if (obj is GCSelection)
        {
            return this.Equals(((GCSelection)obj).GetGCName());

        }
        if (!(obj is GCSSave))
        {
            return false;
        }
        return GCName == ((GCSSave)obj).GCName;
    }
}

[Serializable]
public class GCSSaveCollection
{
    public GCSSave[] save;
    public GCSSaveCollection(GCSSave[] s)
    {
        save = s;
    }

    public GCSSave FindGCSSave(GCSelection gcs)
    {
        foreach (GCSSave gs in save)
        {
            if (gs.Equals(gcs.GetGCName()))
            {
                return gs;
            }

        }
        Debug.LogError("failed to load data: " + gcs);
        return null;
    }
}

[Serializable]
public class PlayerSaveCollection
{
    public int profile = 0;
    public int coins = 0;
    public int numberOfRuns = 0;
    public int numberOfBossKills = 0;
    public PlayerSaveCollection(PlayerSaveStats pss)
    {
        profile = (int)pss.profile;
        this.coins = pss.coins;
        this.numberOfRuns = pss.numberOfRuns;
        this.numberOfBossKills = pss.numberOfBossKills;
    }
}

/// <summary>
/// Anson: need to add values for settings
/// </summary>
[Serializable]
public class SettingsSaveCollection
{
    public float sensitivity = 15;
    public float sensitivityADS = 15; // not using
    public float masterVolume = 1;
    public SettingsSaveCollection(SettingsSaveStats sss)
    {
        this.sensitivity = sss.sensitivity;
        this.sensitivityADS = sss.sensitivityADS;
        this.masterVolume = sss.masterVolume;
    }
}

[Serializable]
public class SaveCollection
{
    public PlayerSaveCollection playerSaveCollection;
    public GCSSaveCollection gCSSaveCollection;
    public SettingsSaveCollection settingsSaveCollection;

    public SaveCollection(PlayerSaveCollection psc, GCSSaveCollection gcssc, SettingsSaveCollection ssc)
    {
        playerSaveCollection = psc;
        gCSSaveCollection = gcssc;
        settingsSaveCollection = ssc;
    }
}

/*
public static class SaveManager
{
    static void SaveData()
    {
        Debug.Log("Start save data");
        //Save GCS
        Debug.Log("Start GCS save data");

        List<GCSelection> allGCS = gunManager.AllGCSelections;
        gCSSaves = new List<GCSSave>();
        string saveString = "";
        foreach (GCSelection g in allGCS)
        {
            gCSSaves.Add(new GCSSave(g));
        }
        gCSSaveCollection = new GCSSaveCollection(gCSSaves.ToArray());
        //Save Player
        print("Start player save data");

        playerSaveCollection = new PlayerSaveCollection(playerMasterScript.PlayerSaveStats);


        print("Write save data");
        saveString = JsonUtility.ToJson(new SaveCollection(playerSaveCollection, gCSSaveCollection));
        print(saveString);
        File.WriteAllText(Application.dataPath + "/SaveFiles/GCSSaves.json", saveString);
        print("Write save data complete");

    }

    void LoadData()
    {
        print("load save data");

        string loadString = File.ReadAllText(Application.dataPath + "/SaveFiles/GCSSaves.json");
        saveCollection = JsonUtility.FromJson<SaveCollection>(loadString);
        playerSaveCollection = saveCollection.playerSaveCollection;
        gCSSaveCollection = saveCollection.gCSSaveCollection;
        print("load save data complete");

    }
}

*/

public class SaveManagerScript : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] bool freshSaveData = false;
    public static SaveManagerScript instance;
    [Space]
    [Header("Save Stuff")]
    public GunManager gunManager;
    public PlayerMasterScript playerMasterScript;
    public SettingsMenuManager settingsMenuManager;
    [SerializeField] PlayerSaveProfile playerSaveProfile;
    [SerializeField] List<GCSSave> gCSSaves;
    SaveCollection saveCollection;
    [SerializeField] GCSSaveCollection gCSSaveCollection;
    [SerializeField] PlayerSaveCollection playerSaveCollection;
    [SerializeField] SettingsSaveCollection settingsSaveCollection;

    public PlayerSaveProfile PlayerSaveProfile { get => playerSaveProfile; set => playerSaveProfile = value; }

    private void Awake()
    {
        if (RemoveDuplicateSaveManager())
        {
            return;
        }
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        Initialisation();

    }
    private void Start()
    {
    }

    public void OnEnable()
    {
        Initialisation();
    }

    private void OnDisable()
    {
        SaveData();
    }

    private void OnDestroy()
    {
        SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SetSaveProfile(PlayerSaveProfile profile)
    {
        playerSaveProfile = profile;
    }

    public void Initialisation()
    {
        AssignComponents();
        if (freshSaveData)
        {
            OverrideData();
        }
        LoadData();
        Debug.Log("Loading data");
        try
        {
            gunManager.GCSSaveCollection = gCSSaveCollection;

        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("gun Manager: null on settingsMenu");
        }
        try
        {
            playerMasterScript.PlayerSaveCollection = playerSaveCollection;

        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("player Master: null on settingsMenu");
        }
        try
        {
            settingsMenuManager.settingsSaveCollection = settingsSaveCollection;

        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Save Manager: null on settingsMenu");
        }
    }

    private void AssignComponents()
    {
        if (!gunManager)
        {
            gunManager = FindObjectOfType<GunManager>();
        }
        if (!playerMasterScript)
        {
            playerMasterScript = FindObjectOfType<PlayerMasterScript>();
        }
        if (!settingsMenuManager)
        {
            settingsMenuManager = FindObjectOfType<SettingsMenuManager>();
        }
        if (!settingsMenuManager)
        {
            settingsMenuManager = playerMasterScript.GetComponentInChildren<SettingsMenuManager>();
        }
    }

    bool RemoveDuplicateSaveManager()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            SetSaveProfile(instance.playerSaveProfile);
            Destroy(instance);
            instance = this;
            return true;
        }
        return false;
    }

    void SaveData(string saveFile = "")
    {
        print("Start save data");
        //Save GCS
        print("Start GCS save data");
        string saveString = "";
        if (gunManager != null)
        {
            List<GCSelection> allGCS = gunManager.AllGCSelections;
            gCSSaves = new List<GCSSave>();
            foreach (GCSelection g in allGCS)
            {
                gCSSaves.Add(new GCSSave(g));
            }
            gCSSaveCollection = new GCSSaveCollection(gCSSaves.ToArray());
            //Save Player
            print("Start player save data");
        }

        if (playerMasterScript != null)
        {
            playerSaveCollection = new PlayerSaveCollection(playerMasterScript.PlayerSaveStats);
        }

        //save settings
        try
        {
            settingsSaveCollection = new SettingsSaveCollection(settingsMenuManager.settingsSaveStats);
        }
        catch (NullReferenceException e)
        {

        }

        print("Write save data");
        saveString = JsonUtility.ToJson(new SaveCollection(playerSaveCollection, gCSSaveCollection, settingsSaveCollection));
        print(saveString);
        if (saveFile.Equals(""))
        {
            saveFile = "GCSSaves" + playerSaveCollection.profile + ".json";
        }
        try
        {

            File.WriteAllText(Application.persistentDataPath + "/SaveFiles/" + saveFile, saveString);
        }
        catch (DirectoryNotFoundException e)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveFiles/");
            File.WriteAllText(Application.persistentDataPath + "/SaveFiles/" + saveFile, saveString);

        }
        print("Write save data complete");

    }

    void LoadData(PlayerSaveProfile psp = PlayerSaveProfile.DEFAULT, string saveFile = "")
    {
        print("load save data");

        if (saveFile.Equals(""))
        {
            saveFile = "GCSSaves" + playerSaveCollection.profile + ".json";
        }
        string loadString = "";
        try
        {
             loadString= File.ReadAllText(Application.persistentDataPath + "/SaveFiles/" + saveFile);
        }catch(FileNotFoundException e)
        {
            Debug.LogWarning("Failed to find save file, loading default save");
            loadString = Resources.Load<TextAsset>("DefaultSave").text;

        }
        saveCollection = JsonUtility.FromJson<SaveCollection>(loadString);
        playerSaveCollection = saveCollection.playerSaveCollection;
        gCSSaveCollection = saveCollection.gCSSaveCollection;
        settingsSaveCollection = saveCollection.settingsSaveCollection;
        print("load save data complete");

    }

    void OverrideData()
    {
        string loadString = File.ReadAllText(Application.persistentDataPath + "/SaveFiles/GCSSaves_BASE.json");
        File.WriteAllText(Application.persistentDataPath + "/SaveFiles/GCSSaves.json", loadString);

    }
}


