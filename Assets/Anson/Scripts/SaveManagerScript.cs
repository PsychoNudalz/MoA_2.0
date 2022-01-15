using System;
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
            return this.Equals(((GCSelection) obj).GetGCName());
        }

        if (!(obj is GCSSave))
        {
            return false;
        }

        return GCName == ((GCSSave) obj).GCName;
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

        Debug.LogError("failed to load data: " + gcs.GetGCName());
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
    public int totalFullClears = 0;

    public PlayerSaveCollection(PlayerSaveStats pss)
    {
        profile = (int) pss.profile;
        this.coins = pss.coins;
        this.numberOfRuns = pss.numberOfRuns;
        this.numberOfBossKills = pss.numberOfBossKills;
        this.totalFullClears = pss.totalFullClears;
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
    [SerializeField]
    bool UseTestSave = false;

    [SerializeField]
    private bool DisplayFullDebug = false;

    public static SaveManagerScript instance;

    [Space]
    [Header("Save Stuff")]
    [SerializeField]
    bool IsLoaded;

    [SerializeField]
    bool IsSaved;

    public GunManager gunManager;
    public PlayerMasterScript playerMasterScript;
    public SettingsMenuManager settingsMenuManager;

    [SerializeField]
    PlayerSaveProfile playerSaveProfile;

    [SerializeField]
    List<GCSSave> gCSSaves;

    SaveCollection saveCollection;
    GCSSaveCollection gCSSaveCollection;
    PlayerSaveCollection playerSaveCollection;
    SettingsSaveCollection settingsSaveCollection;

    public PlayerSaveProfile PlayerSaveProfile
    {
        get => playerSaveProfile;
        set => playerSaveProfile = value;
    }

    private void Awake()
    {
        if (!RemoveDuplicateSaveManager())
        {
        }
    }

    public void LoadProcedure()
    {
        InitialisationData(playerSaveProfile);
        LoadSettings();
        IsLoaded = true;
        IsSaved = false;
    }

    public void LoadProcedure(PlayerSaveProfile psp)
    {
        InitialisationData(psp);
        LoadSettings();
        IsLoaded = true;
        IsSaved = false;
    }

    private void OnApplicationQuit()
    {
        if (!IsSaved)
        {
            SaveProcedure();
        }
    }

    public void SaveProcedure()
    {
        SaveData();
        SaveSettings();
        IsSaved = true;
        IsLoaded = false;
    }


    public void SetSaveProfile(PlayerSaveProfile profile)
    {
        SaveProcedure();
        print("Setting save profile: " + profile.ToString());
        playerSaveProfile = profile;
        LoadProcedure();
    }

    public void SetSaveProfile(int profile)
    {
        SetSaveProfile((PlayerSaveProfile) profile);
    }

    public void InitialisationData(PlayerSaveProfile psp = PlayerSaveProfile.DEFAULT)
    {
        LoadData(psp);
        Debug.Log("Loading data");
        LoadDataToGunAndPlayer();
    }


    private void LoadDataToGunAndPlayer()
    {
        try
        {
            gunManager.LoadSave(gCSSaveCollection);
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Save Manager: null on gun manager");
        }

        try
        {
            playerMasterScript.LoadSave(playerSaveCollection);
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Save Master: null on player ");
        }
    }

    private void AssignComponents()
    {
        /*
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
                */
        gunManager = FindObjectOfType<GunManager>();
        playerMasterScript = FindObjectOfType<PlayerMasterScript>();
        settingsMenuManager = FindObjectOfType<SettingsMenuManager>();
    }

    /// <summary>
    /// return if a duplicate old one is found
    /// </summary>
    /// <returns></returns>
    bool RemoveDuplicateSaveManager()
    {
        //if there is not other save manager
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            AssignComponents();
            LoadProcedure();
            return false;
        }
        //if there is another save manager
        else
        {
            /*
            SetSaveProfile(instance.playerSaveProfile);
            //instance.SaveProcedure();
            //instance.LoadProcedure();
            Destroy(instance.gameObject);
            instance = this;
            print("Removed duplicate");
            */
            instance.SaveProcedure();
            Destroy(gameObject);
            instance.AssignComponents();
            instance.LoadProcedure();
            return true;
        }
    }

    void SaveData()
    {
        string saveFile = "";
        print("Start save data");
        //Save GCS
        if(DisplayFullDebug) {print("Start GCS save data");}
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
            if(DisplayFullDebug){print("Start player save data");}
        }

        if (playerMasterScript != null)
        {
            //print("Creating save");
            playerSaveCollection = new PlayerSaveCollection(playerMasterScript.PlayerSaveStats);
        }


        if(DisplayFullDebug){print("Write save data");}
        saveString =
            JsonUtility.ToJson(new SaveCollection(playerSaveCollection, gCSSaveCollection, settingsSaveCollection));
        if(DisplayFullDebug){print(saveString);}
        if (saveFile.Equals(""))
        {
            saveFile = "GCSSaves_" + playerSaveProfile.ToString() + ".json";
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
        if (UseTestSave)
        {
            psp = PlayerSaveProfile.TESTING;
        }

        playerSaveProfile = psp;
        if (saveFile.Equals(""))
        {
            saveFile = "GCSSaves_" + psp.ToString() + ".json";
        }

        string loadString = "";
        try
        {
            loadString = File.ReadAllText(Application.persistentDataPath + "/SaveFiles/" + saveFile);
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning("Failed to find save file, loading default save");
            loadString = LoadDefaultData();
            saveCollection = JsonUtility.FromJson<SaveCollection>(loadString);
            playerSaveCollection = saveCollection.playerSaveCollection;
            gCSSaveCollection = saveCollection.gCSSaveCollection;
            //SaveData();
            return;
        }

        saveCollection = JsonUtility.FromJson<SaveCollection>(loadString);
        playerSaveCollection = saveCollection.playerSaveCollection;
        gCSSaveCollection = saveCollection.gCSSaveCollection;
        print("load save data complete");
    }

    string LoadDefaultData()
    {
        string loadString = Resources.Load<TextAsset>("DefaultSave").text;
        saveCollection = JsonUtility.FromJson<SaveCollection>(loadString);
        saveCollection.playerSaveCollection.profile = (int) playerSaveProfile;
        loadString = JsonUtility.ToJson(saveCollection);
        return loadString;
    }

    public void SaveSettings()
    {
        if (settingsMenuManager == null)
        {
            settingsMenuManager = FindObjectOfType<SettingsMenuManager>();
        }
        else
        {
            //save settings
            try
            {
                settingsSaveCollection = new SettingsSaveCollection(settingsMenuManager.settingsSaveStats);
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("Failed to save setting save file");
                return;
            }
        }

        string saveString = JsonUtility.ToJson(settingsSaveCollection);
        string settingSaveFile = "Settings.json";
        try
        {
            File.WriteAllText(Application.persistentDataPath + "/SaveFiles/" + settingSaveFile, saveString);
        }
        catch (DirectoryNotFoundException e)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveFiles/");
            File.WriteAllText(Application.persistentDataPath + "/SaveFiles/" + settingSaveFile, saveString);
        }

        print("Write save setting complete");
    }

    public void LoadSettings()
    {
        string loadString = "";

        try
        {
            loadString = File.ReadAllText(Application.persistentDataPath + "/SaveFiles/Settings.json");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning("Failed to find save setting, loading default setting");
            loadString = Resources.Load<TextAsset>("DefaultSetting").text;
            settingsSaveCollection = JsonUtility.FromJson<SettingsSaveCollection>(loadString);
            try
            {
                settingsMenuManager.SetSettingSaveCollection(settingsSaveCollection);
            }
            catch (NullReferenceException i)
            {
                Debug.LogWarning("Save Manager: null on settingsMenu");
            }

            SaveSettings();
            return;
        }

        settingsSaveCollection = JsonUtility.FromJson<SettingsSaveCollection>(loadString);
        try
        {
            settingsMenuManager.SetSettingSaveCollection(settingsSaveCollection);
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Save Manager: null on settingsMenu");
        }
    }

    void OverrideData()
    {
        string loadString = File.ReadAllText(Application.persistentDataPath + "/SaveFiles/GCSSaves_BASE.json");
        File.WriteAllText(Application.persistentDataPath + "/SaveFiles/GCSSaves.json", loadString);
    }
}