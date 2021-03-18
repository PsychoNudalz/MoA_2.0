using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponentUITypeSectionScript : MonoBehaviour
{
    [SerializeField] GunComponents componentType;
    [SerializeField] List<GCSelection> gcs;
    [SerializeField] TMPro.TextMeshProUGUI textBox;
    [SerializeField] List<GunComponentUIButtonScript> buttons;

    public GunComponents ComponentType { get => componentType; set => componentType = value; }
    public List<GCSelection> Gcs { get => gcs; set => gcs = value; }


    public void Initialize(List<GCSelection> gcs)
    {
        this.gcs = gcs;
        SetText();
        ResetButtons();
    }

    void SetText()
    {
        textBox.text = componentType.ToString();
    }

    public void UpdateButtons(List<GunTypes> gunTypes)
    {
        ResetButtons();
        int i = 0;
        foreach(GCSelection g in gcs)
        {
            if (g.IsSamgeType(gunTypes))
            {
                if (i < buttons.Count)
                {
                    buttons[i].SetGCS(g);
                    buttons[i].gameObject.SetActive(true);
                    i++;
                }
                else
                {
                    Debug.LogError(name + " buttons out of index");
                }
            }
        }
    }

    public void ResetButtons()
    {
        foreach(GunComponentUIButtonScript b in buttons)
        {
            b.SetGCS(null);
            b.gameObject.SetActive(false);
        }
    }

}
