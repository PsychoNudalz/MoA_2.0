using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunAlterUIHandler : MonoBehaviour
{
    [Header("Other Components")]
    [SerializeField] GunManager gunManager;
    [SerializeField] GunGeneratorScript gunGeneratorScript;
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
    [SerializeField] TMPro.TextMeshProUGUI currentTypeText;

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
        InitialiseAllButtons();

    }

    public void GenerateGun()
    {
        gunManager.GenerateGun();
    }

    public


    void InitialiseAllButtons()
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
        foreach(GunTypes g in currentGunTypes)
        {
            combineText += g.ToString() + "\n";
        }
        currentTypeText.text = combineText;
    }

    public void SetGunType(int i)
    {
        SetGunType((GunTypes) i);
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
}
