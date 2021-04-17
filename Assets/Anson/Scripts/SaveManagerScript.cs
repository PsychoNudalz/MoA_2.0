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
    public int coins = 0;
    public int numberOfRuns = 0;
    public int numberOfBossKills = 0;
    public PlayerSaveCollection(PlayerSaveStats pss)
    {
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
    public SettingsSaveCollection()
    {

    }
}

[Serializable]
public class SaveCollection
{
    public PlayerSaveCollection playerSaveCollection;
    public GCSSaveCollection gCSSaveCollection;
    public SettingsSaveCollection settingsSaveCollection;

    public SaveCollection(PlayerSaveCollection psc, GCSSaveCollection gcssc)
    {
        playerSaveCollection = psc;
        gCSSaveCollection = gcssc;
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
    public GunManager gunManager;
    public PlayerMasterScript playerMasterScript;
    [SerializeField] List<GCSSave> gCSSaves;
    SaveCollection saveCollection;
    [SerializeField] GCSSaveCollection gCSSaveCollection;
    [SerializeField] PlayerSaveCollection playerSaveCollection;

    private void Awake()
    {
        if (RemoveDuplicateSaveManager())
        {
            return;
        }

        //DontDestroyOnLoad(gameObject);
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

    public void Initialisation()
    {
        if (!gunManager)
        {
            gunManager = FindObjectOfType<GunManager>();
        }
        if (!playerMasterScript)
        {
            playerMasterScript = FindObjectOfType<PlayerMasterScript>();
        }
        if (freshSaveData)
        {
            OverrideData();
        }
        LoadData();
        Debug.Log("Loading data");
        gunManager.GCSSaveCollection = gCSSaveCollection;
        playerMasterScript.PlayerSaveCollection = playerSaveCollection;
    }

    bool RemoveDuplicateSaveManager()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    void SaveData(string saveFile = "")
    {
        print("Start save data");
        //Save GCS
        print("Start GCS save data");

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
        if (saveFile.Equals(""))
        {
            saveFile = "GCSSaves.json";
        }
        try
        {

        File.WriteAllText(Application.persistentDataPath + "/SaveFiles/" + saveFile, saveString);
        } catch(DirectoryNotFoundException e)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveFiles/");
            File.WriteAllText(Application.persistentDataPath + "/SaveFiles/" + saveFile, saveString);

        }
        print("Write save data complete");

    }

    void LoadData(string saveFile = "")
    {
        print("load save data");

        if (saveFile.Equals(""))
        {
            saveFile = "GCSSaves.json";
        }
        string loadString = File.ReadAllText(Application.persistentDataPath + "/SaveFiles/" + saveFile);
        saveCollection = JsonUtility.FromJson<SaveCollection>(loadString);
        playerSaveCollection = saveCollection.playerSaveCollection;
        gCSSaveCollection = saveCollection.gCSSaveCollection;
        print("load save data complete");

    }

    void OverrideData()
    {
        string loadString = File.ReadAllText(Application.persistentDataPath + "/SaveFiles/GCSSaves_BASE.json");
        File.WriteAllText(Application.persistentDataPath + "/SaveFiles/GCSSaves.json", loadString);

    }
}


