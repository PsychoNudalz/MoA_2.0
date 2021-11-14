using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunComponentUITypeSectionScript : MonoBehaviour
{
    [SerializeField] GunComponents componentType;
    [SerializeField] List<GCSelection> gcs;
    [SerializeField] TMPro.TextMeshProUGUI textBox;
    [SerializeField] GridLayoutGroup buttonGrid;
    [SerializeField] GridLayoutGroup parentGrid;
    [SerializeField] Vector2 gridCellSize;
    [SerializeField] GunComponentUIButtonScript baseComponentButton;
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
        GunComponentUIButtonScript newButton;
        foreach (GCSelection g in gcs)
        {
            if (g.IsSamgeType(gunTypes))
            {
                /*
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
                */
                newButton = Instantiate(baseComponentButton.gameObject, buttonGrid.transform).GetComponent<GunComponentUIButtonScript>();
                newButton.SetGCS(g);
                newButton.gameObject.SetActive(true);
                buttons.Add(newButton);
            }
        }
        //SetGridSize();
    }

    public void ResetButtons()
    {
        foreach (GunComponentUIButtonScript b in buttons)
        {
            /*
            b.SetGCS(null);
            b.gameObject.SetActive(false);
            */
            Destroy(b.gameObject);
        }
        buttons = new List<GunComponentUIButtonScript>();

    }

    public void ResetGridSize()
    {
        parentGrid.cellSize = new Vector2(parentGrid.cellSize.x, 50);
    }

    public void SetGridSize()
    {
        gridCellSize = new Vector2(gridCellSize.x, (Mathf.RoundToInt(buttons.Count / 8f + 1)) * 85 + 50);
        //print(buttons.Count);
        print(componentType.ToString() + "  " + (Mathf.FloorToInt(buttons.Count / 8f + 1) * 85 + 50));
        //Vector2 cellSize = parentGrid.cellSize;
        if (gridCellSize.y > parentGrid.cellSize.y)
        {
            print(componentType.ToString() + gridCellSize.y + " replace with " + parentGrid.cellSize.y);
            parentGrid.cellSize = new Vector2(gridCellSize.x, gridCellSize.y);
        }
        else
        {
            print(componentType.ToString() + gridCellSize.y + " smaller than " + parentGrid.cellSize.y);

        }
        buttonGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(gridCellSize.x, (Mathf.RoundToInt(buttons.Count / 8f) + 1) * 85);
    }

    public void SelectAll(bool b)
    {
        foreach (GunComponentUIButtonScript button in buttons)
        {
            if (b)
            {
                button.Select();
            }
            else
            {
                button.Deselect();
            }
        }
    }

}
