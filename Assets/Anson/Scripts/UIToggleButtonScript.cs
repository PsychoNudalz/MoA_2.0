using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleButtonScript : MonoBehaviour
{
    [SerializeField] protected GameObject selectButton;
    [SerializeField] protected GameObject deselectButton;
    public virtual void Select()
    {
        SetButtons(false);
    }

    public virtual void Deselect()
    {
        SetButtons(true);

    }
    /// <summary>
    /// false if button is selected already
    /// </summary>
    /// <param name="b"></param>
    protected virtual void SetButtons(bool b)
    {
        selectButton.SetActive(b);
        deselectButton.SetActive(!b);
    }
}
