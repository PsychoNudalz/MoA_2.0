using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GunAlterEmbed : MonoBehaviour
{
    [SerializeField] bool isDebugBuild = true;
    [SerializeField] Button debugBtn;
    [SerializeField] GameObject debugMenu;
    [SerializeField] GameObject helpOverlay;

    void Start() {
        if (!isDebugBuild) debugBtn.SetEnabled(false);
    }
    public void DebugButtonOnClick() {
        debugMenu.SetActive(!debugMenu.activeSelf);
    }

    public void HelpButtonOnClick() {
        helpOverlay.SetActive(true);
    }

    public void HelpCloseOnClick() {
        helpOverlay.SetActive(false);
    }
}
