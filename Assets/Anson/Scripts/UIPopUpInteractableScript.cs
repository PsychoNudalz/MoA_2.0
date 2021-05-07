using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPopUpInteractableScript : InteractableScript
{

    [SerializeField] protected GameObject UIElement;
    [SerializeField] GameObject toolTip;
    [SerializeField] PlayerMasterScript playerMasterScript;
    [SerializeField] GameObject defaultButton;

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
        if (defaultButton != null)
        {
            FindObjectOfType<EventSystem>().SetSelectedGameObject(defaultButton);
        }
        print("Active UI");
    }
    public override void deactivate()
    {
        base.deactivate();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UIElement.SetActive(false);
        playerMasterScript.SetControls(true);

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            toolTip.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            toolTip.SetActive(false);
        }
    }

    public void UIExitCallBack()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
