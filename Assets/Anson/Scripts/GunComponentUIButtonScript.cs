using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunComponentUIButtonScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject selectButton;
    [SerializeField] GameObject deselectButton;
    [SerializeField] GCSelection gcs;

    public void SetGCS( GCSelection g)
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

    public void Select()
    {
        SetButtons(false);
        gcs.IsSelected = true;
    }

    public void Deselect()
    {
        SetButtons(true);
        gcs.IsSelected = false;

    }

    void SetButtons(bool b)
    {
        selectButton.SetActive(b);
        deselectButton.SetActive(!b);
    }

}
