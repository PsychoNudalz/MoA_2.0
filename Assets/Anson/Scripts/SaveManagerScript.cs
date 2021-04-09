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
            return GCName.Contains(obj as string) || ((string)obj).Contains(GCName);
            //return GCName.Equals(obj as string);
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

[Serializable]
public class SaveCollection
{
    public PlayerSaveCollection playerSaveCollection;
    public GCSSaveCollection gCSSaveCollection;

    public SaveCollection(PlayerSaveCollection psc, GCSSaveCollection gcssc)
    {
        playerSaveCollection = psc;
        gCSSaveCollection = gcssc;
    }
}


public class SaveManagerScript : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] bool freshSaveData = false;
    [Space]
    public GunManager gunManager;
    public PlayerMasterScript playerMasterScript;
    [SerializeField] List<GCSSave> gCSSaves;
    SaveCollection saveCollection;
    [SerializeField] GCSSaveCollection gCSSaveCollection;
    [SerializeField] PlayerSaveCollection playerSaveCollection;

    private void Awake()
    {
        if (!freshSaveData)
        {
            LoadData();
            gunManager.GCSSaveCollection = gCSSaveCollection;
            if (!playerMasterScript)
            {
                playerMasterScript = FindObjectOfType<PlayerMasterScript>();
            }
            playerMasterScript.PlayerSaveCollection = playerSaveCollection;
        }
    }
    private void Start()
    {
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    void SaveData()
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
