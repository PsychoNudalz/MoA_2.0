using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunComponentUIButtonScript : UIToggleButtonScript
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject lockImage;
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
            SetButtons(false);
            buttonText.text = "NULL";
            return;
        }

        if (g.IsUnlocked)
        {
            lockImage.SetActive(false);
            if (g.IsSelected)
            {
                SetButtons(false);
            }
            else
            {
                SetButtons(true);
            }
        }
        else
        {
            lockImage.SetActive(true);
            SetButtons(true);
        }

        buttonText.text = gcs.Component.name;
    }

    public override void Select()
    {
        if (gcs != null)
        {
            gunAlterUIHandler.SelectComponent(gcs);
        }
        if (gcs.IsUnlocked)
        {
            SetButtons(false);
            gcs.IsSelected = true;
        }
    }

    public override void Deselect()
    {
        SetButtons(true);
        gcs.IsSelected = false;
        if (gcs != null)
        {
            gunAlterUIHandler.SelectComponent(gcs);

        }
    }


}
