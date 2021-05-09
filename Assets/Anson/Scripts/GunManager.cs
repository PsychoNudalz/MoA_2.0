using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GCSelection
{

    [SerializeField] GunComponent component;
    [SerializeField] bool isSelected;
    [SerializeField] bool isUnlocked;
    [SerializeField] int cost;

    public GCSelection(GunComponent component)
    {
        this.component = component;
        isSelected = false;
        isUnlocked = false;
        cost = component.ComponentCost;
    }

    public bool IsSameType(GunTypes g)
    {
        return component.GTypes.Contains(g);
    }

    public bool IsSamgeType(List<GunTypes> gs)
    {
        foreach (GunTypes g in gs)
        {
            if (IsSameType(g))
            {
                return true;
            }
        }
        return false;
    }

    public string GetGCName()
    {
        return component.name;
    }

    public GunComponent Component { get => component; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }
    public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
    public int Cost { get => cost; }
}


public class GunManager : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] List<GunComponent> gunComponents;
    [SerializeField] List<GunComponent> exoticGunComponents;
    [Header("GCSelections")]
    [SerializeField] List<GCSelection> body;
    [SerializeField] List<GCSelection> grip;
    [SerializeField] List<GCSelection> stock;
    [SerializeField] List<GCSelection> magazine;
    [SerializeField] List<GCSelection> barrel;
    [SerializeField] List<GCSelection> sight;
    [SerializeField] List<GCSelection> muzzle;
    [SerializeField] List<GCSelection> attachment;
    [SerializeField] List<GCSelection> statBoost;
    [SerializeField] List<GCSelection> allGCSelections;
    [Space]
    [Header("Other Components")]
    [SerializeField] GunGeneratorScript gunGenerator;
    [Space]
    [Header("Save")]
    [SerializeField] GCSSaveCollection gCSSaveCollection;
    [Header("Others")]
    [SerializeField] List<GameObject> gunCache;

    public List<GCSelection> AllGCSelections { get => allGCSelections; }
    public List<GCSelection> Body { get => body; }
    public List<GCSelection> Grip { get => grip; }
    public List<GCSelection> Stock { get => stock; }
    public List<GCSelection> Magazine { get => magazine; }
    public List<GCSelection> Barrel { get => barrel; }
    public List<GCSelection> Sight { get => sight; }
    public List<GCSelection> Muzzle { get => muzzle; }
    public List<GCSelection> Attachment { get => attachment; }
    public List<GCSelection> StatBoost { get => statBoost; }
    public GCSSaveCollection GCSSaveCollection { get => gCSSaveCollection; set => gCSSaveCollection = value; }

    private void Awake()
    {
        if (gCSSaveCollection == null)
        {
            AssignGCSelections();
        }
    }



    private void Start()
    {
        if (!gunGenerator)
        {
            gunGenerator = GetComponent<GunGeneratorScript>();
        }
        //Testing
        InitialiseGenerator(allGCSelections);

    }

    void AssignGCSelections()
    {
        foreach (GunComponent gc in gunComponents)
        {
            AssignGC(gc);
        }
        foreach (GunComponent gc in exoticGunComponents)
        {
            AssignGC(gc);
        }
    }

    void AssignGC(GunComponent gunComponent)
    {
        GCSelection gc = new GCSelection(gunComponent);
        switch (gunComponent.ComponentType)
        {
            case (GunComponents.BODY):
                body.Add(gc);
                break;
            case (GunComponents.GRIP):
                grip.Add(gc);
                break;
            case (GunComponents.STOCK):
                stock.Add(gc);
                break;
            case (GunComponents.MAGAZINE):
                magazine.Add(gc);
                break;
            case (GunComponents.BARREL):
                barrel.Add(gc);
                break;
            case (GunComponents.SIGHT):
                sight.Add(gc);
                break;
            case (GunComponents.MUZZLE):
                muzzle.Add(gc);
                break;
            case (GunComponents.ATTACHMENT):
                attachment.Add(gc);
                break;
            case (GunComponents.STATBOOST):
                statBoost.Add(gc);
                break;
        }
        allGCSelections.Add(gc);
    }

    void InitialiseGenerator(List<GCSelection> selectedGCS)
    {
        print("Updatating generator");
        gunGenerator.ResetLists();
        foreach (GCSelection gcs in selectedGCS)
        {
            if (gcs.IsSelected || gcs.Component.name.Contains("_No"))
            {
                gunGenerator.AddComponentToList(gcs.Component);
            }
        }
    }

    public void UpdateManager()
    {
        print("Updatating Gun Manager");
        InitialiseGenerator(AllGCSelections);
        FindObjectOfType<SaveManagerScript>().SaveProcedure();
    }

    public GameObject GenerateGun()
    {
        gunGenerator.ResetLists();
        InitialiseGenerator(AllGCSelections);
        GameObject newGun = gunGenerator.GenerateGun();
        newGun.transform.position += new Vector3(0, 2, 0);
        gunCache.Add(newGun);
        newGun.transform.parent = this.transform;

        return newGun;

    }

    public List<GameObject> GenerateGun(int numberOfGuns, int minRarity, int maxRarity)
    {
        gunGenerator.ResetLists();
        InitialiseGenerator(AllGCSelections);
        List<GameObject> guns = new List<GameObject>();
        if (minRarity < 0)
        {
            minRarity = 0;
        }
        print("Generating guns:" + numberOfGuns + " " + minRarity + " " + maxRarity);
        for (int i = 0; i < numberOfGuns; i++)
        {
            GameObject newGun = gunGenerator.GenerateGun_Rarity(minRarity, maxRarity);
            newGun.transform.position += new Vector3(0, 2, 0);
            guns.Add(newGun);
            gunCache.Add(newGun);
            newGun.transform.parent = this.transform;
        }
        return guns;

    }

    public int LoadSave(GCSSaveCollection gcss)
    {
        AssignGCSelections();
        gCSSaveCollection = gcss;
        GCSSave temp;
        int errorCount = 0;
        foreach (GCSelection gcs in allGCSelections)
        {
            temp = gcss.FindGCSSave(gcs);
            if (temp != null)
            {
                gcs.IsSelected = temp.isSelected;
                gcs.IsUnlocked = temp.isUnloacked;
            }
            else
            {
                errorCount++;
            }
        }


        return errorCount;
    }

    public void UnlockAll()
    {
        foreach (GCSelection g in allGCSelections)
        {
            g.IsUnlocked = true;
            g.IsSelected = true;
        }
    }

    public void ClearGunsOnGround(bool fullClear = false)
    {
        Debug.Log(this + " CLEARING ALL WAEPONS FROM GROUND");
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject g in gunCache)
        {
            try
            {
                if (g.transform.parent == null)
                {
                    if (fullClear)
                    {
                        temp.Add(g);
                    }
                }
                else if ((transform.Equals(g.transform.parent) || fullClear)&& !g.transform.parent.tag.Equals("PlayerInventory") )
                {
                    if (g.activeSelf)
                    {
                        temp.Add(g);
                    }
                }
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("Clear guns null reference: " + g);

            }
        }
        foreach (GameObject g in temp)
        {
            gunCache.Remove(g);
            Destroy(g);
        }

    }
}
