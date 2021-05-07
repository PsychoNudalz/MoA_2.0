using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class GunAlterUIHandler : MonoBehaviour
{
    [Header("Other Components")]
    [SerializeField] GunManager gunManager;
    [SerializeField] GunGeneratorScript gunGeneratorScript;
    [SerializeField] PlayerMasterScript playerMasterScript;
    [Header("UI Type Sections")]
    [SerializeField] GunComponentUITypeSectionScript UI_Body;
    [SerializeField] GunComponentUITypeSectionScript UI_Grip;
    [SerializeField] GunComponentUITypeSectionScript UI_Magazine;
    [SerializeField] GunComponentUITypeSectionScript UI_Barrel;
    [SerializeField] GunComponentUITypeSectionScript UI_Stock;
    [SerializeField] GunComponentUITypeSectionScript UI_Sight;
    [SerializeField] GunComponentUITypeSectionScript UI_Attachment;
    [SerializeField] GunComponentUITypeSectionScript UI_Muzzle;
    [SerializeField] GunComponentUITypeSectionScript UI_StatBoost;
    [Header("Filter Control")]
    [SerializeField] List<GunTypes> currentGunTypes;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI currentTypeText;
    [SerializeField] TextMeshProUGUI unlockCostText;
    [SerializeField] GameObject unlockButton;
    [Header("Component Preview UI")]
    [SerializeField] GameObject textTemplateGO;
    [SerializeField] TextMeshProUGUI currentComponentText;
    [SerializeField] GridLayoutGroup essentialComponents;
    [SerializeField] GridLayoutGroup potentialComponents;
    [SerializeField] GridLayoutGroup mainStats;
    [SerializeField] GridLayoutGroup elementStats;
    [SerializeField] GridLayoutGroup multiplierStats;
    [Header("Component Preview")]
    [SerializeField] Transform spawnTransform;
    [SerializeField] GCSelection currentGCSelection;
    [SerializeField] GameObject currentGunGO;
    [SerializeField] Camera previewCamera;
    [SerializeField] RawImage previewWindow;
    [SerializeField] RenderTexture renderTexture;


    //[SerializeField] GameObject BaseUIButton;
    //[SerializeField] RectTransform scrollArea;
    //[SerializeField] RectTransform scrollView;
    //[SerializeField] float buttonSize = 120f;
    //[SerializeField] float offset = 300;
    //[SerializeField] Vector2 buttonIndex = new Vector2();

    private void Start()
    {
        //Test button creation
        /*
        GunComponentUIButtonScript newButton = Instantiate(BaseUIButton, scrollArea).GetComponent<GunComponentUIButtonScript>();
        newButton.SetGCS(gunManager.AllGCSelections[0]);
        */
        //CreateNewButton(0);
        if (!gunManager)
        {
            gunManager = FindObjectOfType<GunManager>();
        }
        InitialiseAllButtons();
        if (!playerMasterScript)
        {
            playerMasterScript = FindObjectOfType<PlayerMasterScript>();
        }
        InitializePreviewCamera();

    }

    public void GenerateGun()
    {
        GameObject newGun = gunManager.GenerateGun();
        newGun.transform.position = spawnTransform.position;
    }

    public void InitialiseAllButtons()
    {
        /*
        for(int i = 0; i < Enum.GetNames(typeof(GunComponents)).Length; i++)
        {

        }
        */

        UI_Attachment.Initialize(gunManager.Attachment);
        UI_Body.Initialize(gunManager.Body);
        UI_Barrel.Initialize(gunManager.Barrel);
        UI_Grip.Initialize(gunManager.Grip);
        UI_Magazine.Initialize(gunManager.Magazine);
        UI_Muzzle.Initialize(gunManager.Muzzle);
        UI_Sight.Initialize(gunManager.Sight);
        UI_StatBoost.Initialize(gunManager.StatBoost);
        UI_Stock.Initialize(gunManager.Stock);
    }

    public void UpdateButtons()
    {
        //Anson: i know this is bad code
        UI_Attachment.ResetGridSize();


        UI_Attachment.UpdateButtons(currentGunTypes);
        UI_Body.UpdateButtons(currentGunTypes);
        UI_Barrel.UpdateButtons(currentGunTypes);
        UI_Grip.UpdateButtons(currentGunTypes);
        UI_Magazine.UpdateButtons(currentGunTypes);
        UI_Muzzle.UpdateButtons(currentGunTypes);
        UI_Sight.UpdateButtons(currentGunTypes);
        UI_StatBoost.UpdateButtons(currentGunTypes);
        UI_Stock.UpdateButtons(currentGunTypes);
    }
    public void SetGunType(GunTypes g)
    {
        if (currentGunTypes.Contains(g))
        {
            currentGunTypes.Remove(g);
        }
        else
        {
            currentGunTypes.Add(g);
        }
        UpdateButtons();
        UpdateEnumText();
    }

    void UpdateEnumText()
    {
        string combineText = "";
        foreach (GunTypes g in currentGunTypes)
        {
            combineText += g.ToString() + "\n";
        }
        currentTypeText.text = combineText;
    }

    public void SetGunType(int i)
    {
        SetGunType((GunTypes)i);
    }

    public void SelectComponent(GCSelection gcs, bool isSelected = true)
    {
        currentGCSelection = gcs;
        PreviewComponent(gcs);
        DisplayStats();
        unlockButton.SetActive(false);

        if (!gcs.IsUnlocked)
        {
            UpdateComponentCost(currentGCSelection.Cost);
        }
        else
        {
            currentGCSelection.IsSelected = isSelected;
        }
    }


    public void PreviewComponent(GCSelection gcs)
    {
        if (currentGunGO != null)
        {
            Destroy(currentGunGO);
        }

        currentGunGO = Instantiate(gcs.Component.gameObject, spawnTransform.position, spawnTransform.rotation, spawnTransform);

    }

    void UpdateComponentCost(int amount)
    {
        unlockButton.SetActive(true);
        unlockCostText.text = "Cost:" + amount.ToString();
    }

    public void UnlockComponent()
    {
        if (currentGCSelection != null && playerMasterScript != null)
        {
            if (!currentGCSelection.IsUnlocked)
            {
                if (playerMasterScript.RemoveCoins(currentGCSelection.Cost))
                {
                    print(this + " Unlock component: " + currentGCSelection);
                    currentGCSelection.IsUnlocked = true;
                    currentGCSelection.IsSelected = true;
                    unlockButton.SetActive(false);
                }

            }
        }
        UpdateButtons();
    }


    void InitializePreviewCamera()
    {
        RenderTexture newRenderTexture = new RenderTexture(renderTexture);
        previewCamera.targetTexture = newRenderTexture;
        previewWindow.texture = newRenderTexture;
    }
    /*
    void CreateNewButton(int i)
    {
        buttonIndex = new Vector2(i % 3, i / 3);
        print("Scroll area: " + scrollArea.rect.height);
        GunComponentUIButtonScript newButton = Instantiate(BaseUIButton).GetComponent<GunComponentUIButtonScript>();
        // , new Vector3((scrollArea.rect.width/3f)*buttonIndex.x,-buttonSize* buttonIndex.y),Quaternion.identity,scrollArea);
        newButton.transform.SetParent(scrollArea, false);
        RectTransform temp = newButton.GetComponent<RectTransform>();
        temp.anchoredPosition = new Vector3(buttonSize * buttonIndex.x, -buttonSize * (buttonIndex.y+1)+ scrollArea.rect.height/2);
        newButton.SetGCS(gunManager.AllGCSelections[i]);
    }
    */


    TextMeshProUGUI CreateStatsText(GridLayoutGroup currentGrid)
    {
        GameObject tempText = Instantiate(textTemplateGO, currentGrid.transform);
        tempText.SetActive(true);
        return tempText.GetComponent<TextMeshProUGUI>();
    }


    void DisplayStats()
    {
        foreach (Transform g in mainStats.GetComponentsInChildren<Transform>())
        {
            if (g != mainStats.transform)
            {
                Destroy(g.gameObject);
            }
        }
        foreach (Transform g in elementStats.GetComponentsInChildren<Transform>())
        {
            if (g != elementStats.transform)
            {
                Destroy(g.gameObject);
            }
        }
        foreach (Transform g in multiplierStats.GetComponentsInChildren<Transform>())
        {
            if (g != multiplierStats.transform)
            {
                Destroy(g.gameObject);
            }
        }

        currentComponentText.text = currentGCSelection.GetGCName();
        List<List<string>> statsList = currentGCSelection.Component.GetStats();
        foreach (string s in statsList[0])
        {
            CreateStatsText(mainStats).text = s;
        }
        foreach (string s in statsList[1])
        {
            CreateStatsText(elementStats).text = s;
        }
        foreach (string s in statsList[2])
        {
            CreateStatsText(multiplierStats).text = s;
        }
    }

    void DisplayConnections()
    {
        foreach(KeyValuePair<GunComponents, int> pair in currentGCSelection.Component.GetEssentialDict())
        {
            CreateStatsText(essentialComponents).text = pair.Value + "x " + pair.Key;
        }
    }

    public void SelectAll()
    {
        UI_Attachment.SelectAll(true);
        UI_Body.SelectAll(true);
        UI_Barrel.SelectAll(true);
        UI_Grip.SelectAll(true);
        UI_Magazine.SelectAll(true);
        UI_Muzzle.SelectAll(true);
        UI_Sight.SelectAll(true);
        UI_StatBoost.SelectAll(true);
        UI_Stock.SelectAll(true);
    }
    public void DeselectAll()
    {
        UI_Attachment.SelectAll(false);
        UI_Body.SelectAll(false);
        UI_Barrel.SelectAll(false);
        UI_Grip.SelectAll(false);
        UI_Magazine.SelectAll(false);
        UI_Muzzle.SelectAll(false);
        UI_Sight.SelectAll(false);
        UI_StatBoost.SelectAll(false);
        UI_Stock.SelectAll(false);
    }

    public void GodModeButton()
    {
        playerMasterScript.AddCoins(1000);
        UnlockAll();
        UpdateButtons();
    }

    public void UnlockAll()
    {
        gunManager.UnlockAll();
    }
}
