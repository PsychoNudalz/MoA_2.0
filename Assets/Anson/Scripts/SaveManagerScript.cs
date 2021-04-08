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
        foreach(GCSSave gs in save)
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
public class SaveManagerScript : MonoBehaviour
{
    public GunManager gunManager;
    [SerializeField] List<GCSSave> gCSSaves;
    [SerializeField] GCSSaveCollection gCSSaveCollection;

    private void Awake()
    {
        LoadData();
        gunManager.GCSSaveCollection = gCSSaveCollection;
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
        List<GCSelection> allGCS = gunManager.AllGCSelections;
        gCSSaves = new List<GCSSave>();
        string saveString = "";
        foreach (GCSelection g in allGCS)
        {
            gCSSaves.Add(new GCSSave(g));
        }
        print(gCSSaves.Count);
        gCSSaveCollection = new GCSSaveCollection(gCSSaves.ToArray());
        saveString = JsonUtility.ToJson(gCSSaveCollection);
        //saveString = JsonUtility.ToJson(gCSSaves[0]);
        print(saveString);
        File.WriteAllText(Application.dataPath + "/SaveFiles/GCSSaves.json", saveString);

    }

    void LoadData()
    {
        string loadString = File.ReadAllText(Application.dataPath + "/SaveFiles/GCSSaves.json");
        gCSSaveCollection = JsonUtility.FromJson<GCSSaveCollection>(loadString);

    }
}
