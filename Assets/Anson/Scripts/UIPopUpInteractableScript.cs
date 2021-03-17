using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUpInteractableScript : InteractableScript
{

    [SerializeField] GameObject UIElement;
    [SerializeField] PlayerMasterScript playerMasterScript;

    // Start is called before the first frame update

    private void Start()
    {
        playerMasterScript = FindObjectOfType<PlayerMasterScript>();
        UIElement.SetActive(false);
    }
    public override void activate()
    {
        base.activate();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        UIElement.SetActive(true);
        playerMasterScript.SetControls(false);
    }
    public override void deactivate()
    {
        base.deactivate();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UIElement.SetActive(false);
        playerMasterScript.SetControls(true);

    }
}
