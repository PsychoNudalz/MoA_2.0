using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunComponentUIButtonScript : UIToggleButtonScript
{
    [SerializeField] TextMeshProUGUI buttonText;
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
        if (g.IsSelected)
        {
            SetButtons(false);
        }
        else
        {
            SetButtons(true);
        }

        buttonText.text = gcs.Component.name;
    }

    public override void Select()
    {
        SetButtons(false);
        gcs.IsSelected = true;
        if (gcs != null)
        {
            gunAlterUIHandler.PreviewComponent(gcs);

        }
    }

    public override void Deselect()
    {
        SetButtons(true);
        gcs.IsSelected = false;
        if (gcs != null)
        {
            gunAlterUIHandler.PreviewComponent(gcs);

        }
    }


}
