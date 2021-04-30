using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunComponentUIButtonScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject lockImage;
    [SerializeField] GameObject SelectedSprite;
    [SerializeField] GCSelection gcs;
    [SerializeField] GunAlterUIHandler gunAlterUIHandler;

    private void Start()
    {
        gunAlterUIHandler = GetComponentInParent<GunAlterUIHandler>();
    }
    public void SetGCS(GCSelection g)
    {
        gcs = g;
        if (g == null)
        {
            SetSelected(false);
            buttonText.text = "NULL";
            return;
        }

        if (g.IsUnlocked)
        {
            lockImage.SetActive(false);
            SetSelected(g.IsSelected);
        }
        else
        {
            lockImage.SetActive(true);
            SetSelected(false);
        }

        buttonText.text = gcs.Component.name;
    }

    public void Select()
    {
        if (gcs != null)
        {
            gunAlterUIHandler.SelectComponent(gcs);
        }
        if (gcs.IsUnlocked)
        {
            SetSelected(gcs.IsSelected);
        }

    }

    public void Deselect()
    {
        if (gcs != null)
        {
            gunAlterUIHandler.SelectComponent(gcs, false);

        }
        if (gcs.IsUnlocked)
        {
            SetSelected(gcs.IsSelected);
        }

    }

    public void ToggleButtone()
    {
        if (gcs.IsSelected)
        {
            Deselect();
        }
        else
        {
            Select();
        }
    }

    void SetSelected(bool setB)
    {
        SelectedSprite.SetActive(setB);
        //GetComponent<Button>().
    }


}
